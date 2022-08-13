using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.src.Tasks
{
    internal class StaticTask : Task
    {
        string comment;
        public override void Advance()
        {
            base.Advance();
            TaskManager.DeleteTask(UID);
        }
    }
}
