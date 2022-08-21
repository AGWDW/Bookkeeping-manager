using System.Globalization;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Rules
{
    internal class Numeric_Rule : ValidationRule
    {
        public int Limit { get; set; } = -1;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool succ = int.TryParse(value.ToString(), out int ans);
            if (succ && (ans < Limit || Limit == -1))
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Not an integer or value of string too great");
        }
    }
}
