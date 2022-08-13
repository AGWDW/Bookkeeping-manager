using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.src.Tasks
{
    internal abstract class Task
    {
        public int UID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        protected TaskState state;
        protected DateTime date;
        protected List<Task> children;

        public Task()
        {
            UID = -1;
            state = TaskState.Due;
            children = new List<Task>();
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

        protected void UpdateState()
        {
            if (date < DateTime.Today)
            {
                state = TaskState.Future;
            }
            else if (date > DateTime.Today)
            {
                state = TaskState.Late;
            }
            else
            {
                state = TaskState.Due;
            }
        }
        public void SetDate(DateTime date)
        {
            this.date = date;
        }
    }
}
