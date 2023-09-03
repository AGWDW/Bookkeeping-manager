using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Bookkeeping_manager.Scripts
{
    public static class Extensions
    {
        #region To Dictionary
        public static Dictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static Dictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);
            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }
        #endregion

        public static bool IsUppder(this string obj)
        {
            return obj == obj.ToUpper();
        }
        public static string Split(this string obj, bool t)
        {
            string res = "";
            obj = obj.Replace("_0", " /");
            for (int i = 0; i < obj.Length - 1; i++)
            {
                string n = obj[i + 1].ToString();
                string c = obj[i].ToString();

                if (c.IsUppder() && !n.IsUppder())
                {
                    res += " " + c;
                }
                else
                {
                    res += c;
                }
            }
            res += obj.Last() == '_' ? ' ' : obj.Last();
            return res.Trim();
        }
        public static string RandomString(this Random rnd, int length)
        {
            StringBuilder str_build = new StringBuilder();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = rnd.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }
        public static T GetProperty<T>(this object obj, string propName)
        {
            return (T)obj.GetType().GetProperty(propName).GetValue(obj, null);
        }
        public static void SetProperty<T>(this object obj, string propName, T value)
        {
            obj.GetType().GetProperty(propName).SetValue(obj, value);
        }
        public static void SetVariable<T>(this object obj, string varName, T value)
        {
            obj.GetType().GetField(varName).SetValue(obj, value);
        }
        public static bool IsAlphaNumeric(this string obj)
        {
            return Regex.IsMatch(obj, "^[a-zA-Z0-9]*$");
        }
        public static bool IsAlpha(this string obj)
        {
            return Regex.IsMatch(obj, "^[a-zA-Z]*$");
        }
        public static bool IsNumeric(this string obj)
        {
            return Regex.IsMatch(obj, "^[0-9]*$");
        }
        public static bool IsHex(this string obj)
        {
            return Regex.IsMatch(obj, "^[a-f0-9]*$");
        }
        public static int[] SplitDate(this string obj)
        {
            int[] res = new int[3];
            int i = 0;
            foreach (string s in obj.Split('/'))
            {
                res[i++] = int.Parse(s);
            }
            return res;
        }
        /// <summary>
        /// Converts the string to date format asums at least 2 digits for day month and year
        /// </summary>
        /// <param name="str"></param>
        /// <returns>the converted string or new date time</returns>
        public static DateTime ToDate(this string str)
        {
            DateTime default_ = DateTime.Today;
            if (str is null)
            {
                return default_;
            }
            str = str.Trim();
            str = str.ToLower();
            string[] invalidChars = {
                "/", "-", ".", ",", " ", "\\", ":", ";", "|"
            };
            foreach (string invalid in invalidChars)
            {
                str = str.Replace(invalid, "");
            }
            if (!int.TryParse(str, out _))
            {
                return default_;
            }
            int day, month, year;
            string d, m, y = DateTime.Today.ToString("yyyy");
            switch (str.Length)
            {
                case 4:
                    d = str.Substring(0, 2);
                    m = str.Substring(2, 2);
                    break;
                case 6:
                    d = str.Substring(0, 2);
                    m = str.Substring(2, 2);
                    y = str.Substring(4, 2);
                    y = $"20{y}";
                    break;
                case 8:
                    d = str.Substring(0, 2);
                    m = str.Substring(2, 2);
                    y = str.Substring(4, 4);
                    break;
                default:
                    return default_;
            }
            day = int.Parse(d);
            month = int.Parse(m);
            year = int.Parse(y);

            return new DateTime(year, month, day);
        }

        public static UIElement GetAtGridPos(this Grid obj, int row, int column)
        {
            foreach (UIElement child in obj.Children)
            {
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column)
                {
                    return child;
                }
            }
            return null;
            //return obj.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
        }
        public static int GetDayOfWeek(this DateTime obj)
        {
            return (obj.DayOfWeek == 0) ? 7 : (int)obj.DayOfWeek; ;
        }
        public static SolidColorBrush ToColour(this string obj)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom($"#{obj.Replace("#", "")}");
        }
        public static System.Windows.Media.Color ToColour(this string obj, bool c)
        {
            return (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString($"#FF{obj.Replace("#", "")}");
        }
        public static string AsMonth(this int obj)
        {
            string[] months = new string[12]
            {
                "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            };
            return months[obj - 1];
        }
        public static string ToString(this System.Windows.Media.Color obj, string format)
        {
            return obj.ToString().Substring(3);
        }
        public static void ToFile(this byte[] obj, string filePath)
        {
            using (Stream file = File.OpenWrite(filePath))
            {
                file.Write(obj, 0, obj.Length);
            }
        }
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        public static ImageSource ToImageSource(this Icon icon)
        {
            Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            return wpfBitmap;
        }
        public static string GetString(this DateTime obj)
        {
            return obj.ToString("dd/MM/yyyy");
        }
        public static DateTime GetLastDay(this DateTime obj)
        {
            return new DateTime(obj.Year, obj.Month, DateTime.DaysInMonth(obj.Year, obj.Month));
        }
        public static DateTime GetFirstDay(this DateTime obj)
        {
            return obj.SetDay(1);
        }
        public static DateTime SetDay(this DateTime obj, int day)
        {
            return new DateTime(obj.Year, obj.Month, day);
        }
        public static DateTime AddOffset(this DateTime obj, DateTimeInterval offset)
        {
            DateTime res = obj.AddDays(offset.Day);
            res = res.AddMonths(offset.Month);
            res = res.AddYears(offset.Year);
            switch (offset.AssertDate)
            {
                case AssertDate.FirstOfMonth:
                    res = res.GetFirstDay();
                    break;
                case AssertDate.LastofMonth:
                    res = res.GetLastDay();
                    break;
                case AssertDate.LastFridayOfMonth:
                    res = res.GetLastDay();
                    while (res.GetDayOfWeek() != 5)
                    {
                        res = res.AddDays(-1);
                    }
                    break;
                case AssertDate.Month28th:
                    res = res.SetDay(28);
                    break;
            }

            return res;
        }
    }
}
