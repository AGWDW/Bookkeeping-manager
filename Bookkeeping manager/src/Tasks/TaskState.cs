using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.src.Tasks
{
    internal enum TaskState
    {
        /// <summary>
        /// Due on a previouse day
        /// </summary>
        Late, 
        /// <summary>
        /// Due today
        /// </summary>
        Due, 
        /// <summary>
        /// Due in the future
        /// </summary>
        Future
    }
}
