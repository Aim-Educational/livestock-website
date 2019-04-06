using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Website.Other
{
    public class MustBeTrueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var val = value as bool?;
            if(val == null || (!val ?? false))
                return new ValidationResult($"{validationContext.DisplayName} must be ticked.");

            return ValidationResult.Success;
        }
    }
}
