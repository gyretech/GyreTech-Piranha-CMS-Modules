using System;
using System.ComponentModel.DataAnnotations;

namespace Piranha.AspNetCore.Identity.EF.Manager.Validation
{
    public class RequiredIfTrueAttribute: ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public RequiredIfTrueAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if(property == null)
                throw new ArgumentException("Poperty with this name is not found");

            var comparisonValue = (bool) property.GetValue(validationContext.ObjectInstance);

            if (!comparisonValue && value == null)
            {
                return ValidationResult.Success;
            }
            if (comparisonValue && value != null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }

        
    }
}