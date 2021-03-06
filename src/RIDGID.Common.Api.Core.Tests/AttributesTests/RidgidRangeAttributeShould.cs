﻿using NUnit.Framework;
using RIDGID.Common.Api.Core.Attributes;
using RIDGID.Common.Api.Core.Utilities;
using Shouldly;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RIDGID.Common.Api.Core.Tests.AttributesTests
{
    internal class ModelWithRangeFieldWithoutCustomErrorMessage
    {
        [RidgidRange(1, typeof(int), "1", "4")]
        public int? MultipleWordedField { get; set; }
    }

    internal class ModelWithRangeFieldWithCustomErrorMessage
    {
        [RidgidRange(1, typeof(int), "1", "4", "CustomMessage")]
        public int? Field { get; set; }
    }

    [TestFixture]
    public class RidgidRangeAttributeShould
    {
        [Test]
        public void InvalidateCorrectlyWithoutCustomErrorMessage()
        {
            //--Arrange
            var model = new ModelWithRangeFieldWithoutCustomErrorMessage
            {
                MultipleWordedField = 0
            };
            var validationContext = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            FormatResponseMessage.SetSnakeCaseSetting(false);

            //--Act
            var valid = Validator.TryValidateObject(model, validationContext, result, true);

            //--Assert
            valid.ShouldBeFalse();
            result.Count.ShouldBe(1);
            const string defaultErrorMsg = "The value of the 'MultipleWordedField' field must be between '1' and '4'.";
            result[0].ErrorMessage
                .ShouldBe(ModelStateCustomErrorMessage.Create(1, defaultErrorMsg));
        }

        [Test]
        public void UseSnakeCaseInDefaultErrorMessageWhenSet()
        {
            //--Arrange
            var model = new ModelWithRangeFieldWithoutCustomErrorMessage
            {
                MultipleWordedField = 0
            };
            var validationContext = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            FormatResponseMessage.SetSnakeCaseSetting(true);

            //--Act
            var valid = Validator.TryValidateObject(model, validationContext, result, true);

            //--Assert
            valid.ShouldBeFalse();
            result.Count.ShouldBe(1);
            const string defaultErrorMsg = "The value of the 'multiple_worded_field' field must be between '1' and '4'.";
            result[0].ErrorMessage
                .ShouldBe(ModelStateCustomErrorMessage.Create(1, defaultErrorMsg));
        }

        [Test]
        public void InvalidateCorrectlyWithCustomErrorMessage()
        {
            //--Arrange
            var model = new ModelWithRangeFieldWithCustomErrorMessage
            {
                Field = 0
            };

            var validationContext = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            //--Act
            var valid = Validator.TryValidateObject(model, validationContext, result, true);

            //--Assert
            valid.ShouldBeFalse();
            result.Count.ShouldBe(1);
            result[0].ErrorMessage
                .ShouldBe(ModelStateCustomErrorMessage.Create(1, "CustomMessage"));
        }

        [Test]
        public void ValidateCorrectly()
        {
            //--Arrange
            var model = new ModelWithRangeFieldWithoutCustomErrorMessage
            {
                MultipleWordedField = 1
            };
            var validationContext = new ValidationContext(model, null, null);
            var result = new List<ValidationResult>();

            //--Act
            var valid = Validator.TryValidateObject(model, validationContext, result, true);

            //--Assert
            valid.ShouldBeTrue();
            result.Count.ShouldBe(0);
        }
    }
}