using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.src.Tasks
{
    internal class ReacuringTask : Task
    {
        TimeSpan offset;
        public ReacuringTask(DateTime date) : base(date)
        {
        }
        public override void Advance()
        {
            base.Advance();
            date += offset;
            UpdateState();
        }
    }
}
