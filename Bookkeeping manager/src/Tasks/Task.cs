using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Bookkeeping_manager.src.Tasks
{
    public abstract class Task
    {
        public int UID { get; set; }
        public string Name { get; set; }
        public SolidColorBrush ColourIndicator { get; set; }
        public TaskState State { get; protected set; }
        private int advanceCount;
        protected DateTime date;
        protected List<Task> children;

        public Task()
        {
            UID = -1;
            State = TaskState.Due;
            children = new List<Task>();
            Name = "";
            date = DateTime.Today;
            ColourIndicator = Constants.DEFAULT_COLOUR;
            advanceCount = 0;
        }

        public Task(DateTime date) : this()
        {
            this.date = date.Date;
        }

        /// <summary>
        /// if static will mark for deletion /*/
        /// if reacuring will advance the date and change state appropriatly /*/
        /// will advance all children as well
        /// </summary>
        public virtual void Advance()
        {
            advanceCount++;
            foreach (Task child in children)
            {
                if(child.advanceCount < advanceCount)
                {
                    child.Advance();
                }
            }
        }

        public virtual void UpdateState()
        {
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
        public void SetDate(DateTime date)
        {
            this.date = date;
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
            if(children.Contains(child))
            {
                return false;
            }
            children.Add(child);
            return true;
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
            return t1.UID == t2.UID;
        }
        public static bool operator != (Task t1, Task t2)
        {
            return !(t1 == t2);
        }
    }
}
