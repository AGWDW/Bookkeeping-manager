using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Rules
{
    internal class NumericRule : ValidationRule
    {
        /// <summary>
        /// -1 if no restriction
        /// </summary>
        public int Length { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool succ = int.TryParse(value.ToString(), out int ans);
            if (succ && (value.ToString().Length < Length || Length == -1))
            {
                return ValidationResult.ValidResult;
            }
            return ValidationResult.ValidResult;
            // return new ValidationResult(false, "Not an integer or length of string too great");
        }
    }
}
