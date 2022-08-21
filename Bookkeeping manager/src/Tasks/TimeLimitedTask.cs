using System;

namespace Bookkeeping_manager.src.Tasks
{
    internal class TimeLimitedTask : ReacuringTask
    {
        private int timesShown = 0;
        /// <summary>
        /// How many days to show for
        /// </summary>
        public int ShowFor { get; set; }

        public TimeLimitedTask() : base()
        {

        }
        public TimeLimitedTask(DateTime date) : base(date)
        {
        }


        public override void UpdateState()
        {
            TaskState prev = State;
            base.UpdateState();
            // then still show
            if (prev == TaskState.Due && State == TaskState.Late)
            {
                if (timesShown < ShowFor)
                {
                    timesShown++;
                    State = TaskState.Due;
                }
                else
                {
                    TaskManager.DeleteTask(UID);
                }
            }
        }
    }
}
