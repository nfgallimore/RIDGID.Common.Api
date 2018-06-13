﻿using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RIDGID.Common.Api.Core.Utilities
{
    public class FormatResponseMessage
    {
        public static string CreateJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, JsonSerializerSetting());
        }

        public static HttpResponseMessage CreateMessage(object responseBody, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = statusCode
            };

            if (responseBody != null)
            {
                response.Content = new StringContent(CreateJson(responseBody), Encoding.UTF8, "application/json");
            }
            return response;
        }

        public static T DeserializeMessage<T>(string content, bool defaultSnakeCaseSetting = false)
        {
            return JsonConvert.DeserializeObject<T>(content, JsonSerializerSetting());
        }

        public static bool IsSnakeCase()
        {
            if (!bool.TryParse(ConfigurationManager.AppSettings["snakecase"], out var setting))
            {
                setting = false;
            }
            return setting;
        }

        public static JsonSerializerSettings JsonSerializerSetting()
        {
            return IsSnakeCase()
                ? new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy(true, true) }
                }
                : new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver(),
                };
        }
    }
}