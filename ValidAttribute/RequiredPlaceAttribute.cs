using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DailyPlanner.ValidAttribute
{
    public class RequiredPlaceAttribute : ValidationAttribute
    {
        private string TypeEntries;

        private const string NullValueErrorMesage = "Обязательно для заполнения.";

        public RequiredPlaceAttribute(string TypeEntries)
        {
            this.TypeEntries = TypeEntries;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var TypeEntries = validationContext.ObjectType.GetProperty(this.TypeEntries).GetValue(validationContext.ObjectInstance, null);
            if ((string)TypeEntries == "Встреча" && value == null)
                return new ValidationResult(NullValueErrorMesage);
            return ValidationResult.Success;
        }
    }
}