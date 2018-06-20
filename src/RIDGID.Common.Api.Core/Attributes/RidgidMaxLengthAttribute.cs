﻿using RIDGID.Common.Api.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RIDGID.Common.Api.Core.Attributes
{
    public class RidgidMaxLengthAttribute : RidgidValidationAttribute
    {
        public int Length { get; set; }

        public RidgidMaxLengthAttribute(int errorId, int length) : base(errorId)
        {
            Length = length;
        }

        public RidgidMaxLengthAttribute(int errorId, int length, string customErrorMessage) : base(errorId, customErrorMessage)
        {
            Length = length;
        }

        public override bool IsValid(object value)
        {
            return new MaxLengthAttribute(Length).IsValid(value);
        }

        public override string FormatErrorMessage(string fieldName)
        {
            var errorMessage = CustomErrorMessage ?? CreateErrorMessage(fieldName);
            return ModelStateCustomErrorMessage.Create(ErrorId, errorMessage);
        }

        private string CreateErrorMessage(string fieldName)
        {
            return $"The '{fieldName}' field cannot be greater than '{Length}' characters long.";
        }
    }
}