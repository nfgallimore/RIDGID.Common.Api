﻿using RIDGID.Common.Api.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RIDGID.Common.Api.Core.Attributes
{
    public class RidgidMinLengthAttribute : RidgidValidationAttribute
    {
        public int Length { get; set; }

        public RidgidMinLengthAttribute(int errorId, int length) : base(errorId)
        {
            Length = length;
        }

        public RidgidMinLengthAttribute(int errorId, int length, string customErrorMessage) : base(errorId, customErrorMessage)
        {
            Length = length;
        }

        public override bool IsValid(object value)
        {
            return new MinLengthAttribute(Length).IsValid(value);
        }

        public override string FormatErrorMessage(string fieldName)
        {
            var errorMessage = CustomErrorMessage ?? new MinLengthAttribute(Length).FormatErrorMessage(fieldName);
            return ModelStateCustomErrorMessage.Create(ErrorId, errorMessage);
        }
    }
}