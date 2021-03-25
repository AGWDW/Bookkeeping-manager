using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bookkeeping_manager.Scripts
{
    public static class DataEnforce
    {
        public static string Integer(string fist, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (int.TryParse(next, out int res))
            {
                return next;
            }
            return fist;
        }
        public static string Length(string fist, string next, int len, bool crop = false, bool extend = false, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if(next.Length > len)
            {
                if (crop)
                {
                    return next.Substring(0, len);
                }
                else
                {
                    return fist;
                }
            }
            else if(next.Length < len)
            {
                if (extend)
                {
                    while (next.Length != len){
                        next += "#";
                    }
                }
                else
                {
                    return fist;
                }
            }
            return next;
        }
        public static string Date(string first, string next, bool canBeEmpty = true)
        {
            if(next == "" && canBeEmpty)
            {
                return next;
            }

            try
            {
                if(next is null)
                {
                    return first;
                }
                next = next.Trim().Replace("/", "");
                if (!next.IsAlphaNumeric())
                    return first;
                if (int.TryParse(next, out _) && next.Length >= 4)
                {
                    if (next.Length == 4)
                    {
                        next += DateTime.Now.ToString("yyyy");
                    }
                    else if (next.Length == 6)
                    {
                        next = next.Insert(4, "20");
                    }
                    int d = int.Parse($"{next[0]}{next[1]}");
                    int m = int.Parse($"{next[2]}{next[3]}");
                    int y = int.Parse($"{next.Substring(4, 4)}");
                    if (m > 12)
                    {
                        return first;
                    }
                    if (d > DateTime.DaysInMonth(y, m))
                    {
                        return first;
                    }
                    return $"{d:00}/{m:00}/{y}";
                }
                else
                {
                    return first;
                }
            }
            catch
            {
                return first;
            }
        }
        public static DateTime InFuture(DateTime first, DateTime next, bool canBeToday = true)
        {
            if (canBeToday)
            {
                return next >= DateTime.Now.Date ? next : first;
            }
            return next > DateTime.Now.Date ? next : first;
        }
        public static string LastDay(string first, string next)
        {
            if (next == "")
                return "";
            return next.ToDate().GetLastDay().GetString();
        }
        public static string Day(string first, int day)
        {
            if (first == "")
                return "";
            return first.ToDate().SetDay(day).GetString();
        }
        public static string Friday(string first)
        {
            DateTime res = first.ToDate();
            while(res.GetDayOfWeek() != 5)
            {
                res = res.AddDays(1);
            }
            return res.GetString();
        }
        public static string LastFriday(string first)
        {
            DateTime res = first.ToDate().GetLastDay();
            while (res.GetDayOfWeek() != 5)
            {
                res = res.AddDays(-1);
            }
            return res.GetString();
        }
        public static string Money(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            next = next.Replace("£", "");
            string[] split = next.Split('.');
            if (split.Length == 0 || split.Length > 2 || !next.Replace(".", "").IsNumeric())
                return first;
            foreach(string t in split)
            {
                if(!int.TryParse(t, out _))
                {
                    return first;
                }
            }
            int pounds = int.Parse(split[0]);
            int pence = 0;
            if(split.Length == 1)
            {
                
            }if(split.Length == 2)
            {
                pence = int.Parse(split[1]);
                while(pence >= 100)
                {
                    pence -= 100;
                    pounds++;
                }
            }
            if(pounds == 0 && pence == 0)
            {
                return first;
            }
            return $"£{pounds:00}.{pence:00}";
        }
        public static string Email(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
            {
                return next;
            }
            return first;
        }
        public static string PhoneNumber(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"^(?:(?:\(?(?:0(?:0|11)\)?[\s-]?\(?|\+)44\)?[\s-]? (?:\(?0\)?[\s-]?)?)|(?:\(?0))(?:(?:\d{5}\)?[\s -]?\d{4,5})|(?:\d{4}\)?[\s -]? (?:\d{5}|\d{3}[\s-]?\d{3}))|(?:\d{3}\)?[\s -]?\d{3}[\s-]?\d{3,4})|(?:\d{2}\)?[\s -]?\d{4}[\s-]?\d{4}))(?:[\s-]? (?:x|ext\.?|\#)\d{3,4})?$", RegexOptions.IgnoreCase))
            {
                return next;
            }
            return first;
        }
        public static string UTR(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"^[0-9]{10}$", RegexOptions.IgnoreCase))
            {
                return next;
            }
            return first;
        }
        public static string PAYE(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"^[0-9]{3}P[A-Z][0-9]{7}[0-9X]$"))
            {
                return next;
            }
            return first;
        }
        public static string PAYEEmployer(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"^[0-9]{3}\/?[A-Z]{2}[0-9]{5}$"))
            {
                return next;
            }
            return first;
        }
        public static string VATNumber(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"^(GB)?([0-9]{9}([0-9]{3})?|[A-Z]{2}[0-9]{3})$"))
            {
                return next;
            }
            return first;
        }
        public static string CompanysHouseNumber(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"^[0-9]{8}$"))
            {
                return next;
            }
            return first;
        }
        public static string CharityNumber(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"^[0-9]{7}$"))
            {
                return next;
            }
            return first;
        }
        // #############################################################################################
        public static string CompanysHouseAuthCode(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, "^[0-9]{3}[-|/]*[0-9]{6}[-|/]*[0-9]{6}$"))
            {
                return next;
            }
            return first;
        }
        // #############################################################################################
        public static string GovGateway(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"^[0-9]{12}$"))
            {
                return next;
            }
            return first;
        }
        public static string NI(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"^(?!BG)(?!GB)(?!NK)(?!KN)(?!TN)(?!NT)(?!ZZ)(?:[A-CEGHJ-PR-TW-Z][A-CEGHJ-NPR-TW-Z])(?:\s*\d\s*){6}([A-D]|\s)$"))
            {
                return next;
            }
            return first;
        }
        public static string SICCode(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"^[0-9]{5}$"))
            {
                return next;
            }
            return first;
        }
        public static string PostCode(string first, string next, bool canBeEmpty = true)
        {
            if (next == "" && canBeEmpty)
            {
                return next;
            }
            if (Regex.IsMatch(next, @"^([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([AZa-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]))))[0-9][A-Za-z]{2})$"))
            {
                return next;
            }
            return first;
        }
    }
}
