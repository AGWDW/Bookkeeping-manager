using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.src.Tasks
{
    public abstract class Task
    {
        public int UID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskState State { get; protected set; }
        protected DateTime date;
        protected List<Task> children;

        public Task()
        {
            UID = -1;
            State = TaskState.Due;
            children = new List<Task>();
            Name = "";
            Description = "";
            date = DateTime.Today;
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
            foreach(Task child in children)
            {
                child.Advance();
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

        public string GetDate()
        {
            return $"Due: {date:dd/MM/yyyy}";
        }
    }
}
