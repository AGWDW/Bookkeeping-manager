using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.src.Tasks
{
    public enum TaskType
    {
        Static, Reacuring, TimeLimited
    }
    internal static class TaskManager
    {
        static int uid_counter = 1;
        /// <summary>
        /// all tasks including the children
        /// </summary>
        public static List<Task> AllTasks { get; set; } =  new List<Task>();
        /// <summary>
        /// Deletes the given task from allTasks doenst effect any parents (eg if it is a child it will remain one but not be listed in allTasks)
        /// </summary>
        /// <param name="task">The task to be deleted</param>
        /// <returns>true if deleted false otherwise</returns>
        public static bool DeleteTask(int uid)
        {
            int deleted = AllTasks.RemoveAll((Task t) => t.UID == uid);
            Debug.Assert(deleted <= 1);
            return deleted > 0;
        }

        /// <summary>
        /// adds the task to the list of tasks and asigns it a uid (then inccrements the uid)
        /// </summary>
        /// <param name="task">the task to add</param>
        /// <param name="uid">the uid assigned</param>
        /// <returns>true if success false otherwise</returns>
        public static bool AddTask(Task task, out int uid)
        {
            uid = uid_counter++;
            task.UID = uid;
            AllTasks.Add(task);
            return true;
        }

        /// <summary>
        /// Gets the task withe the specified uid null if it doesnt exist
        /// </summary>
        /// <param name="uid">uid of the to get</param>
        /// <returns>true if success false otherwise</returns>
        public static Task GetTask(int uid)
        {
            foreach (Task t in AllTasks)
            {
                if(t.UID == uid)
                {
                    return t;
                }
            }
            return null;
        }

        public static Task GetOrCreate(int uid, TaskType type, out int actualUID)
        {
            actualUID = -1;
            Task t = GetTask(uid);
            if(t is null)
            {
                switch (type)
                {
                    case TaskType.Static:
                        t = new StaticTask();
                        break;
                    case TaskType.Reacuring:
                        t = new ReacuringTask();
                        break;
                    case TaskType.TimeLimited:
                        t = new TimeLimitedTask();
                        break;
                }
                AddTask(t, out actualUID);
            }
            else
            {
                actualUID = uid;
            }
            return t;
        }
    }
}
