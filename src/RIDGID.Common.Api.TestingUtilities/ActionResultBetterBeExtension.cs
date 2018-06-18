﻿using Newtonsoft.Json;
using RIDGID.Common.Api.Core.Objects;
using Shouldly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Web.Http;

namespace RIDGID.Common.Api.TestingUtilities
{
    public static class ActionResultBetterBeExtension
    {
        public static void BetterBeNull(this IHttpActionResult actionResult, HttpStatusCode expectedStatusCode,
            string expectedLocationHeader = null)
        {
            Content(actionResult, expectedStatusCode, true);
            CheckLocationHeader(actionResult, expectedLocationHeader);
        }

        public static void BetterBe(this IHttpActionResult actionResult, HttpStatusCode expectedStatusCode,
            string expectedDebugErrorMessage, int expectedErrorId)
        {
            BetterBe(actionResult, expectedStatusCode, new ErrorsResponse(expectedDebugErrorMessage, expectedErrorId));
        }
        // Checks that an actionresult is equal to the expected result, and that the status code and optional location
        // header are what is expected
        public static void BetterBe<TModelType>(this IHttpActionResult actionResult,
            HttpStatusCode expectedStatusCode, TModelType expectedResult, string expectedLocationHeader = null)
        {
            var contentAsString = Content(actionResult, expectedStatusCode, false);
            TModelType returnedModel;
            try
            {
                returnedModel = JsonConvert.DeserializeObject<TModelType>(contentAsString);
            }
            catch (JsonSerializationException)
            {
                throw new JsonSerializationException($"{contentAsString} could not be deserialized into the expected object.");
            }
            returnedModel.ShouldNotBeNull();
            var expectedFieldValues = GetFieldValuesForModel(expectedResult).ToList();
            var returnedFieldValues = GetFieldValuesForModel(returnedModel).ToList();

            for (var fieldIndex = 0; fieldIndex < expectedFieldValues.Count; fieldIndex++)
            {
                var field = expectedFieldValues[fieldIndex];
                if (FieldIsEnumerable(field))
                {
                    CheckValuesOfEnumerableField(expectedFieldValues, fieldIndex, returnedFieldValues);
                }
                else
                {
                    field.ShouldBe(returnedFieldValues[fieldIndex]);
                }
            }
            CheckLocationHeader(actionResult, expectedLocationHeader);
        }

        private static void CheckLocationHeader(IHttpActionResult actionResult, string expectedLocationHeader)
        {
            if (expectedLocationHeader != null)
            {
                actionResult.ExecuteAsync(new CancellationToken()).Result.Content.Headers.ContentLocation
                    .ShouldBe(new Uri(expectedLocationHeader));
            }
        }

        private static string Content(IHttpActionResult actionResult, HttpStatusCode expectedStatusCode, bool contentShouldBeNull)
        {
            var result = actionResult.ExecuteAsync(new CancellationToken()).Result;
            result.StatusCode.ShouldBe(expectedStatusCode);
            if (contentShouldBeNull)
            {
                result.Content.ShouldBeNull();
                return null;
            }
            var contentAsString = result.Content.ReadAsStringAsync().Result;
            contentAsString.ShouldNotBeNull();
            contentAsString.Length.ShouldBeGreaterThan(0);
            return contentAsString;
        }

        private static bool FieldIsEnumerable(object field)
        {
            if (field is string || field == null)
            {
                return false;
            }
            var type = field.GetType();
            var interfaces = type.GetInterfaces();
            var isEnumerable = interfaces.Contains(typeof(IEnumerable));
            return isEnumerable;
        }

        private static void CheckValuesOfEnumerableField(IEnumerable<object> expectedFieldValues, int fieldIndex,
            IEnumerable<object> returnedFieldValues)
        {
            var expectedFieldAsList = ((IEnumerable<object>)expectedFieldValues.ToList()[fieldIndex]).ToList();
            var returnedFieldAsList = ((IEnumerable<object>)returnedFieldValues.ToList()[fieldIndex]).ToList();
            for (var i = 0; i < expectedFieldAsList.Count; i++)
            {
                var field = expectedFieldAsList[i];
                if (FieldIsEnumerable(field))
                {
                    CheckValuesOfEnumerableField((IEnumerable<object>)field, i,
                        (IEnumerable<object>)returnedFieldAsList[i]);
                }
                else
                {
                    var expectedFieldSubFieldList = GetFieldValuesForModel(field).ToList();
                    var returnedFieldSubFieldList = GetFieldValuesForModel(returnedFieldAsList[i]).ToList();
                    for (var j = 0; j < expectedFieldSubFieldList.Count(); j++)
                    {
                        var expectedFieldSubField = expectedFieldSubFieldList[j];
                        var returnedFieldSubField = returnedFieldSubFieldList[j];
                        expectedFieldSubField.ShouldBe(returnedFieldSubField);
                    }
                }
            }
        }

        private static IEnumerable<object> GetFieldValuesForModel<TModelType>(TModelType model)
        {
            var declaredFields = model.GetType().GetTypeInfo().DeclaredFields.ToList();
            var baseTypeInfo = model.GetType().GetTypeInfo().BaseType.GetTypeInfo();

            while (baseTypeInfo.DeclaredFields.ToList().Count > 0)
            {
                declaredFields.AddRange(baseTypeInfo.DeclaredFields.ToList());
                baseTypeInfo = baseTypeInfo.GetTypeInfo().BaseType.GetTypeInfo();
            };

            var fieldValues = new List<object>();
            foreach (var field in declaredFields)
            {
                fieldValues.Add(field.GetValue(model));
            }
            return fieldValues;
        }
    }
}