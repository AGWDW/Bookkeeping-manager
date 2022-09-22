using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bookkeeping_manager.Scripts
{
    public class Event : MongoObject
    {
        [BsonIgnore]
        public object Binding;
        public string BindingProperty { get; set; }
        public string BindingName { get; set; }
        public string BindingType { get; set; }
        public bool CanBeEdited { get; set; }
        [BsonIgnore]
        public DateTime Date { get; internal set; }
        public string DateRaw
        {
            get
            {
                return Date.GetString();
            }
            set
            {
                Date = DateTime.Parse(value);
            }
        }
        public string DisplayName { get; set; }
        public string Comment { get; set; }
        public string ColourType { get; set; }
        [BsonIgnore]
        public string Colour
        {
            get
            {
                if (!ColourType.IsHex())
                {
                    return DataHandler.EventColours[ColourType];
                }
                return ColourType;
            }
        }
        /// <summary>
        /// The period of time that a late task wont show as late
        /// </summary>
        public int ShowPeriod { get; set; }
        public bool CanBeLate { get; set; }
        private int lifeSpan;
        public int LifeSpan
        {
            get => lifeSpan;
            set
            {
                UpdateBinding = () =>
                {
                    Binding.SetProperty(BindingProperty, Date.GetString());
                };
                lifeSpan = value;
            }
        }
        public bool IsLate()
        {
            bool res = Date < DateTime.Now.Date;
            if (!CanBeLate || (DateTime.Today - Date).TotalDays > LifeSpan)
            {
                Advance(-1);
                return false;
            }
            return res;
        }
        public List<Interval> Intervals { get; set; }
        /// <summary>
        /// The next interval to be added
        /// </summary>
        private int CurrentInterval { get; set; }
        public bool Advance(int intervals = -1)
        {
            if (Intervals.Count == 0)
                return false;
            void adv()
            {
                Date += Intervals[CurrentInterval];
                CurrentInterval = CurrentInterval++ % Intervals.Count;
            }
            if (intervals >= 0)
            {
                for (int i = 0; i < intervals; i++)
                {
                    adv();
                }
                UpdateBindingBase();
                return true;
            }
            while (Date < DateTime.Today)
            {
                adv();
            }
            UpdateBindingBase();
            return true;
        }
        public bool Advance(DateTime date)
        {
            if (Intervals.Count == 0)
                return false;
            void adv()
            {
                Date += Intervals[CurrentInterval];
                CurrentInterval = CurrentInterval++ % Intervals.Count;
            }
            while (Date < date)
            {
                adv();
            }
            UpdateBindingBase();
            return true;
        }
        public bool ShowLate()
        {
            if (ShowPeriod == 0)
                return true;
            return (DateTime.Today - Date).TotalDays > ShowPeriod;
        }
        public bool Delete { get; set; }
        private bool changed = false;
        [BsonIgnore]
        public bool Changed
        {
            get => changed;
            set
            {
                if (value)
                {
                    _ = 0;
                }
                else
                {
                    _ = 0;
                }
                changed = value;
            }
        }
        public Event(DateTime initalDate, string name = "NULL", string comment = "", bool canBeLate = true, bool canBeEdited = false, string colourType = "NULL", int life = 365, int showPeriod = 0)
        {
            Date = initalDate.Date;
            DisplayName = name;
            Comment = comment;
            CanBeLate = canBeLate;
            Intervals = new List<Interval>();
            CurrentInterval = 0;
            CanBeEdited = canBeEdited;
            ColourType = colourType;
            UpdateBinding = () =>
            {
                Binding.SetProperty(BindingProperty, Date.GetString());
            };
            LifeSpan = life;
            ShowPeriod = showPeriod;
            Changed = true;
            Delete = false;
        }
        public void SetIntervals(params Interval[] intervals)
        {
            Intervals = intervals.OfType<Interval>().ToList();
        }
        private void UpdateBindingBase()
        {
            Changed = true;
            if (Binding is null) return;
            if (UpdateBinding is null)
                return;
            UpdateBinding();
        }
        [BsonIgnore]
        public Action UpdateBinding;

        public void SetBinding(object binding, string property, string type, string name)
        {
            Binding = binding;
            BindingProperty = property;
            BindingType = type;
            BindingName = name;
        }
        public override string ToString()
        {
            return DisplayName + " : " + Delete.ToString() + " : " + Changed.ToString();
        }
    }
    public class Interval
    {
        public DateTimeInterval IntervalRaw { get; set; }
        private string forceDay;
        internal int forceDate;
        public string ForceDay
        {
            get => forceDay;
            set
            {
                forceDay = value;
                forceDate = -1;
            }
        }
        public int ForceDate
        {
            get => forceDate;
            set
            {
                forceDate = value;
                forceDay = "";
            }
        }
        public bool ForceLastDayOfMonth { get; set; }
        public bool ForceFirstDayOfMonth { get; set; }
        public bool LastFriday { get; set; }
        private bool DayUp { get; set; }
        private bool DateUp { get; set; }
        public Interval(DateTimeInterval raw)
        {
            IntervalRaw = raw;
            DayUp = false;
            DateUp = false;
            forceDay = "";
            forceDate = -1;
        }
        public Interval(int y = 1, int m = 0, int d = 0)
        {
            IntervalRaw = new DateTimeInterval(y, m, d);
            DayUp = false;
            DateUp = false;
            forceDay = "";
            forceDate = -1;
        }
        public static DateTime operator +(DateTime a, Interval b)
        {
            DateTime res = a.AddOffset(b.IntervalRaw);
            if (b.ForceDay != "")
            {
                int increment = b.DayUp ? 1 : -1;
                while (res.DayOfWeek.ToString() != b.ForceDay)
                {
                    res.AddDays(increment);
                }
            }
            else if (b.ForceDate != -1)
            {
                int increment = b.DateUp ? 1 : -1;
                while (res.Day != b.ForceDate)
                {
                    res.AddDays(increment);
                }
            }
            else if (b.ForceLastDayOfMonth)
            {
                res = res.GetLastDay();
            }
            else if (b.ForceLastDayOfMonth)
            {
                res = res.GetFirstDay();
            }
            else if (b.LastFriday)
            {
                res = res.GetLastDay();
                while (res.GetDayOfWeek() != 5)
                {
                    res = res.AddDays(-1);
                }
            }
            return res;
        }
        public static DateTime operator +(Interval a, DateTime b)
        {
            return b + a;
        }

    }

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
