using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.src.Tasks
{
    internal class StaticTask : Task
    {
        public string Comment { get;set; }
        public StaticTask() : base()
        {
            Comment = "";
        }
        public override void Advance()
        {
            base.Advance();
            TaskManager.DeleteTask(UID);
        }
    }
}
