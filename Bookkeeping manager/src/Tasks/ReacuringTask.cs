using Bookkeeping_manager.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.src.Tasks
{
    internal class ReacuringTask : Task
    {
        public DateTimeInterval Offset { get; set; }
        public ReacuringTask() : base()
        {

        }
        public ReacuringTask(DateTime date) : base(date)
        {
        }
        public override void Advance()
        {
            base.Advance();
            date = date.AddOffset(Offset);
            UpdateState();
            Save();
        }
    }
}
