using Bookkeeping_manager.Scripts;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Bookkeeping_manager.src.Tasks
{
    public abstract class Task : MongoObject
    {
        public int UID { get; set; }
        public string Date
        {
            get => date.GetString();
            set => date = value.ToDate();
        }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (!DatabaseConnection.Deserilazing)
                {
                    name = value;
                }
            }
        }
        [BsonIgnore]
        public SolidColorBrush ColourIndicator { get; set; }
        public string ColourIndicatorString
        {
            get => ColourIndicator.ToString();
            set => ColourIndicator = new SolidColorBrush((Color)ColorConverter.ConvertFromString(value));
        }
        public TaskState State { get; protected set; }
        private int advanceCount;
        protected DateTime date { get; set; }
        public List<int> ChildrenUIDs { get; set; }

        public Task()
        {
            UID = -1;
            State = TaskState.Due;
            ChildrenUIDs = new List<int>();
            Name = "";
            date = DateTime.Today;
            ColourIndicator = Constants.DEFAULT_COLOUR;
            advanceCount = 0;
        }

        public Task(DateTime DateRaw) : this()
        {
            this.date = DateRaw.Date;
        }

        /// <summary>
        /// if static will mark for deletion /*/
        /// if reacuring will advance the DateRaw and change state appropriatly /*/
        /// will advance all children as well
        /// </summary>
        public virtual void Advance()
        {
            advanceCount++;
            foreach (int uid in ChildrenUIDs)
            {
                Task child = TaskManager.GetTask(uid);
                if(child.advanceCount < advanceCount)
                {
                    child.Advance();
                }
            }
        }

        public virtual void UpdateState()
        {
            if (DatabaseConnection.Deserilazing)
            {
                return;
            }
            if (date < DateTime.Today)
            {
                State = TaskState.Late;
            }
            else if (date > DateTime.Today)
            {
                State = TaskState.Future;
            }
            else
            {
                State = TaskState.Due;
            }
        }
        public void SetDate(DateTime DateRaw)
        {
            if (DatabaseConnection.Deserilazing)
            {
                return;
            }
            this.date = DateRaw;
            UpdateState();
        }
        public void SetDate(int year,int month, int day)
        {
            SetDate(new DateTime(year, month, day));
        }

        public string GetDate()
        {
            return $"Due: {date:dd/MM/yyyy}";
        }

        public bool TryAddChild(Task child)
        {
            if(ChildrenUIDs.Contains(child.UID))
            {
                return false;
            }
            ChildrenUIDs.Add(child.UID);
            return true;
        }

        /// <summary>
        /// will upload to database
        /// </summary>
        public void Save()
        {
            DatabaseConnection.UpdateTask(UID);
        }

        public override int GetHashCode()
        {
            return UID;
        }

        public override bool Equals(object obj)
        {
            return obj is Task && this == obj as Task;
        }
        public static bool operator == (Task t1, Task t2)
        {
            if(t1 is null && t2 is null)
            {
                return true;
            }
            return !(t1 is null || t2 is null) && t1.UID == t2.UID;
        }
        public static bool operator != (Task t1, Task t2)
        {
            return !(t1 == t2);
        }
    }
}
