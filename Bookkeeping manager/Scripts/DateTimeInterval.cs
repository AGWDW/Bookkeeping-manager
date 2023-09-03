using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.Scripts
{

    public enum AssertDate
    {
        None, LastofMonth, FirstOfMonth, LastFridayOfMonth, Month28th
    }
    public class DateTimeInterval
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public AssertDate AssertDate { get; set; }
        public DateTimeInterval(int y, int m, int d)
        {
            Year = y;
            Month = m;
            Day = d;
            AssertDate = AssertDate.None;
        }
        public DateTime ToDateTime()
        {
            return new DateTime(Year, Month, Day);
        }
        public override string ToString() => $"{Year}:{Month}:{Day}";

        /// <summary>
        /// Multiplys all propitys by the mult
        /// </summary>
        /// <param name="mult"></param>
        /// <returns></returns>
        public static DateTimeInterval operator *(DateTimeInterval obj, int mult)
        {
            DateTimeInterval res = new DateTimeInterval(obj.Year * mult, obj.Month * mult, obj.Day * mult);
            return res;
        }
        public DateTimeInterval Copy()
        {
            return new DateTimeInterval(Year, Month, Day)
            {
                AssertDate = AssertDate
            };
        }
    }
}
