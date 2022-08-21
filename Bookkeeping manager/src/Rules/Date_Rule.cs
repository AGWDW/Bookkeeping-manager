using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Rules
{
    internal class Date_Rule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string data = value as string;
            bool good = data.Length == 6 || data.Length == 0;
            good = good && int.TryParse(data, out _);
            if (good)
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Not a valid input for dates");
        }
    }
}
