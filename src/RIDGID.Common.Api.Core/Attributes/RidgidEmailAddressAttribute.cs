﻿using RIDGID.Common.Api.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RIDGID.Common.Api.Core.Attributes
{
    public class RidgidEmailAddressAttribute : RidgidValidationAttribute
    {
        public RidgidEmailAddressAttribute(int errorId, string customErrorMessage) : base(errorId, customErrorMessage)
        {

        }

        public RidgidEmailAddressAttribute(int errorId) : base(errorId)
        {

        }

        public override bool IsValid(object value)
        {
            return new EmailAddressAttribute().IsValid(value);
        }

        public override string FormatErrorMessage(string fieldName)
        {
            var errorMessage = CustomErrorMessage ?? new EmailAddressAttribute().FormatErrorMessage(fieldName);
            return ModelStateCustomErrorMessage.Create(ErrorId, errorMessage);
        }
    }
}