using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Rules
{
    internal class Alpha_Rule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if(Regex.IsMatch(value.ToString(), @"^[a-zA-Z]$"))
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Text Contains Invalid Characters");
        }
    }
}
