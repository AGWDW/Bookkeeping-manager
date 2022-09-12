using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Tasks;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Bookkeeping_manager.src.Clients
{
    public class CIS_Infomation_Data : ClientData
    {
        public void Initalize()
        {
            WithheldEnabled = !WithheldEnabled;
            WithheldEnabled = !WithheldEnabled;

            SufferedEnabled = !SufferedEnabled;
            SufferedEnabled = !SufferedEnabled;
        }

        public CIS_Infomation_Data(string name) : base(name)
        {
        }
        public override void ReName(string name)
        {
            TaskManager.RenameTask(withheld_UID, parentName, name);
            TaskManager.RenameTask(suffered_UID, parentName, name);
            base.ReName(name);
        }

        private int withheld_UID, suffered_UID;
        private bool withheldEnabled;
        public bool WithheldEnabled
        {
            get => withheldEnabled;
            set
            {
                if (withheldEnabled != value)
                {
                    withheldEnabled = value;
                    if (value)
                    {

                        ReacuringTask task = (ReacuringTask)
                            TaskManager.GetOrCreate(withheld_UID, TaskType.Reacuring, out withheld_UID);
                        task.Name = $"CIS Withheld for {parentName}";
                        task.Offset = Constants.MONTH;
                        task.SetDate(DateTime.Today.SetDay(19));
                    }
                    else
                    {
                        TaskManager.DeleteTask(withheld_UID);
                    }
                }
            }
        }
        private bool sufferedEnabled;
        public bool SufferedEnabled
        {
            get => sufferedEnabled;
            set
            {
                if (sufferedEnabled != value)
                {
                    sufferedEnabled = value;
                    if (value)
                    {
                        ReacuringTask task = (ReacuringTask)
                            TaskManager.GetOrCreate(suffered_UID, TaskType.Reacuring, out suffered_UID);
                        task.Name = $"CIS Suffered for {parentName}";
                        task.Offset = Constants.MONTH;
                        task.SetDate(DateTime.Today.SetDay(19));
                    }
                    else
                    {
                        TaskManager.DeleteTask(suffered_UID);
                    }
                }
            }
        }
    }
}
