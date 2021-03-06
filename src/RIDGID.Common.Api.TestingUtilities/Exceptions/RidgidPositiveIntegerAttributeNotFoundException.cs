﻿using System;
using RIDGID.Common.Api.Core.Attributes;

namespace RIDGID.Common.Api.TestingUtilities.Exceptions
{
    public class RidgidPositiveIntegerAttributeNotFoundException : Exception
    {
        public override string Message { get; }

        public RidgidPositiveIntegerAttributeNotFoundException(string fieldName)
        {
            Message = CreateErrorMessage(fieldName);
        }

        private static string CreateErrorMessage(string fieldName)
        {
            return $"The {nameof(RidgidPositiveIntegerAttribute)} could not be found on the {fieldName} field.";
        }
    }
}
