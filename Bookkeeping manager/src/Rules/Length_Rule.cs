using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Rules
{
    internal enum LengthType
    {
        Exact, Range, Min, Max, Either

    }
    internal class Length_Rule : ValidationRule
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public LengthType Type { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool good = false;
            int l = value.ToString().Length;
            switch(Type)
            {
                case LengthType.Exact:
                    good = l == Max;
                    break;
                case LengthType.Range:
                    good = l >= Min && l <= Max;
                    break;
                case LengthType.Min:
                    good = l >= Min;
                    break;
                case LengthType.Max:
                    good = l <= Max;
                    break;
                case LengthType.Either:
                    good = l == Max || l == Min;
                    break;
            }
            if (good)
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Length Invalid");
        }
    }
}
