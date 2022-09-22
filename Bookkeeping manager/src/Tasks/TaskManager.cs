using Bookkeeping_manager.Scripts;
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
    public static class TaskManager
    {
        internal static int uid_counter { get; private set; } = 1;
        /// <summary>
        /// all tasks including the children
        /// </summary>
        public static List<Task> AllTasks { get; private set; } =  new List<Task>();
        /// <summary>
        /// Deletes the given task from allTasks doenst effect any parents (eg if it is a child it will remain one but not be listed in allTasks)
        /// </summary>
        /// <param name="task">The task to be deleted</param>
        /// <returns>true if deleted false otherwise</returns>
        public static bool DeleteTask(int uid)
        {
            DatabaseConnection.DeleteTask(uid);
            int deleted = AllTasks.RemoveAll((Task t) => t.UID == uid);
            Debug.Assert(deleted <= 1);
            return deleted > 0;
        }

        /// <summary>
        /// calls delete task on all tasks that match the mask
        /// </summary>
        /// <param name="mask"></param>
        /// <returns>true if all found are deleted false otherwise</returns>
        public static bool DeleteTasksWhere(Predicate<Task> mask)
        {
            List<int> uids = new List<int>();
            foreach(Task t in AllTasks)
            {
                if (mask(t))
                {
                    uids.Add(t.UID);
                }
            }
            bool res = true;
            foreach(int uid in uids)
            {
                res &= DeleteTask(uid);
            }
            return res;
        }

        /// <summary>
        /// adds the task to the list of tasks and asigns it a uid (then inccrements the uid) 
        /// doesnt do it if deserializing
        /// </summary>
        /// <param name="task">the task to add</param>
        /// <param name="uid">the uid assigned</param>
        /// <returns>true if success false otherwise</returns>
        public static bool AddTask(Task task, out int uid)
        {
            if (DatabaseConnection.Deserilazing)
            {
                uid = uid_counter;
                return false;
            }
            uid = uid_counter++;
            task.UID = uid;
            AllTasks.Add(task);

            DatabaseConnection.AddTask(uid);

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

        /// <summary>
        /// will return the task found or create and add it to all tasks
        /// </summary>
        /// <param name="uid">the uid of the task to find</param>
        /// <param name="type">the type of the task to create</param>
        /// <param name="actualUID">the resulting uid</param>
        /// <returns>the task found or created</returns>
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
                if (DatabaseConnection.Deserilazing)
                {
                    actualUID = uid;
                    switch (type)
                    {
                        case TaskType.Static:
                            return new StaticTask();
                        case TaskType.Reacuring:
                            return new ReacuringTask();
                        case TaskType.TimeLimited:
                            return new TimeLimitedTask();
                    }
                }
                actualUID = uid;
            }
            return t;
        }

        /// <summary>
        /// will replace the prevName with newName in the Name property
        /// </summary>
        /// <param name="uid">uid of the task to find</param>
        /// <param name="prevName">the name to replace</param>
        /// <param name="newName">the new name</param>
        /// <returns>true if task found false otherwise</returns>
        public static bool RenameTask(int uid, string prevName, string newName)
        {
            Task t = GetTask(uid);
            if(t is null)
            {
                return false;
            }
            t.Name = t.Name.Replace(prevName, newName);
            return true;
        }

        internal static void SetUID_Counter(int counter)
        {
            uid_counter = counter;
        }

        internal static void SetTasks(List<Task> tasks)
        {
            AllTasks = tasks;
        }

        public static void UpdateValue(int uid, ref string value)
        {
            UpdateValue(uid, new DateTimeInterval(0, 0, 0), ref value);
        }
        public static void UpdateValue(int uid, DateTimeInterval interval, ref string value)
        {
            Task t = GetTask(uid);
            if (t is null)
            {
                return;
            }
            DateTime d = t.Date.ToDate();
            d = d.AddOffset(interval);
            value = d.GetString();
            t.Save();
        }
    }
}
