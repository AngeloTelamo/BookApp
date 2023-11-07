using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ASI.Basecode.Resources.Messages;

namespace ASI.Basecode.Services.Annotations
{
    public class CustomRequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        public CustomRequiredAttribute() : base()
        {
            ErrorMessageResourceType = typeof(ASI.Basecode.Resources.Messages.Errors);
            ErrorMessageResourceName = "RequiredAttribute_ValidationError";
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(this.ErrorMessageString, name);
        }
    }

    public class ConditionalRequiredAttribute : ValidationAttribute
    {
        private readonly string _flagName;

        public ConditionalRequiredAttribute(string flagName)
        {
            _flagName = flagName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var flagPropertyInfo = validationContext.ObjectType.GetProperty(_flagName);
            if (flagPropertyInfo == null)
            {
                return new ValidationResult("Property not found.");
            }

            var flagPropertyValue = flagPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (flagPropertyValue is bool && (bool)flagPropertyValue)
            {
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    ErrorMessageResourceType = typeof(ASI.Basecode.Resources.Messages.Errors);
                    ErrorMessageResourceName = "RequiredAttribute_ValidationError";

                    return new ValidationResult(string.Format(this.ErrorMessageString, validationContext.DisplayName));
                }
            }
            return ValidationResult.Success;
        }
    }
}