using MongoDB.Driver;
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
        /// <summary>
        /// adds the days months and years using DateTime's methods
        /// </summary>
        public static DateTime Add(this DateTime a, DateTimeInterval b)
        {
            return a.AddDays(b.Day).AddMonths(b.Month).AddYears(b.Year);
        }
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

        /// <summary>
        /// obj == obj.ToUpper()
        /// </summary>
        public static bool IsUppder(this string obj)
        {
            return obj == obj.ToUpper();
        }
        /// <summary>
        /// seperates the obj based on the character order and grammer eg _ = ' '
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="t"></param>
        /// <returns></returns>
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
        /// <summary>
        /// not sure if it returns the actual object or a copy
        /// </summary>
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
        /// <summary>
        /// true if the string conatsions only a-z A-Z 0-9
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsAlphaNumeric(this string obj)
        {
            return Regex.IsMatch(obj, "^[a-zA-Z0-9]*$");
        }
        /// <summary>
        /// true if only a-z A-Z
        /// </summary>
        public static bool IsAlpha(this string obj)
        {
            return Regex.IsMatch(obj, "^[a-zA-Z]*$");
        }
        /// <summary>
        /// true if only 0-9
        /// </summary>
        public static bool IsNumeric(this string obj)
        {
            return Regex.IsMatch(obj, "^[0-9]*$");
        }
        /// <summary>
        /// true if a-f 0-9
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsHex(this string obj)
        {
            return Regex.IsMatch(obj, "^[a-f0-9]*$");
        }
        /// <summary>
        /// { day, month, year} removes nothing splits on / can throw error
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        /// converts the string to a DateTime calls the extension mentod SplitDate
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime ToDate(this string obj)
        {
            var split = obj.SplitDate();
            return new DateTime(split[2], split[1], split[0]);
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
        /// <summary>
        /// day of week 1 - 7 mon - sun 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetDayOfWeek(this DateTime obj)
        {
            return (obj.DayOfWeek == DayOfWeek.Sunday) ? 7 : (int)obj.DayOfWeek; ;
        }
        /// <summary>
        /// hex to solid brush doens matter about #
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        /// <summary>
        /// retuns obj.ToString("dd/MM/yyyy")
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetString(this DateTime obj)
        {
            return obj.ToString("dd/MM/yyyy");
        }
        /// <summary>
        /// returns last day of the month 28 in case of leep year 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetLastDay(this DateTime obj)
        {
            // returns 28 in case of a leep year
            return new DateTime(
                obj.Year, 
                obj.Month, 
                DateTime.DaysInMonth(obj.Year, obj.Month) 
                - (DateTime.IsLeapYear(obj.Year) && obj.Month == 2 ? 1 : 0));
        }
        /// <summary>
        /// returns first day of month
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetFirstDay(this DateTime obj)
        {
            return obj.SetDay(1);
        }
        /// <summary>
        /// calls a construct with the given day
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime SetDay(this DateTime obj, int day)
        {
            return new DateTime(obj.Year, obj.Month, day);
        }
        /// <summary>
        /// calls a construct with the given month
        /// </summary>
        public static DateTime SetMonth(this DateTime obj, int month)
        {
            return new DateTime(obj.Year, month, obj.Day);
        }
    }
}
