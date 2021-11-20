using Bookkeeping_manager.Scripts;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Bookkeeping_manager.Tasks
{
    public class Task
    {
        private string hexColour;
        public string HexColour
        {
            get
            {
                return hexColour;
            }
            set
            {
                if (hexColour != value)
                {
                    hexColour = value;
                }
            }
        }
        public bool ShouldDelete { get; set; }
        public int ShowPeriod { get; set; }
        public int Index { get; private set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public SolidColorBrush Colour => HexColour.ToColour();
        public bool IsLate
        {
            get
            {
                if (!CanBeLate)
                {
                    return false;
                }
                TimeSpan delta = DateTime.Today - Date;
                int days = (int)Math.Round((float)delta.TotalDays);
                int d_days = ShowPeriod - days;

                return d_days <= 0;
            }
        }
        public string Name { get; set; }
        public bool CanBeEdited => !CanAdvance;
        public bool CanAdvance { get; internal set; }
        public bool CanBeLate { get; set; }
        public Task()
        {
            Date = new DateTime();
            Name = "";
            hexColour = "#00ff00";
            Comment = "";
            ShouldDelete = false;
            CanAdvance = true;
            CanBeLate = true;
            ShowPeriod = 1;
        }
        public Task(string name, DateTime date, int index, bool canBeLate = true, int showPeriod = 1) : this()
        {
            Name = name;
            Date = date;
            Index = index;
            ShowPeriod = showPeriod;
        }
        public void Advance(Interval interval)
        {
            Date += interval;
        }
    }
    public class TaskGroup : MongoObject
    {
        public string PayRollPeriod { get; set; }
        public string PayRollInterval { get; set; }
        public string TaskComment
        {
            get => Parent.Comment;
            set => Parent.Comment = value;
        }
        public string Colour
        {
            get => Parent.HexColour;
            set => Parent.HexColour = value;
        }
        public string AMLContactName { get; set; }
        public int VATPeriod { get; set; }
        public string BaseDate { get; set; }
        public string Type { get; set; }
        [BsonIgnore]
        public string[] Names { get; set; } // ignored
        [BsonIgnore]
        public Task[] Tasks { get; set; } // ignored
        [BsonIgnore]
        public Interval[] Intervals { get; set; } // ignored
        public int[] AdvanceCounts { get; set; }
        [BsonIgnore]
        public Task Parent { get => Tasks[0]; set => Tasks[0] = value; } // ignored
        public string ClientName { get; set; }
        [BsonIgnore]
        public int Length { get; protected set; } // ignored
        [BsonIgnore]
        public bool HasChanged { get; set; } // ignored
        [BsonIgnore]
        public Task this[int index]
        {
            get => Tasks[index];
            set => Tasks[index] = value;
        } // ignored
        /// <summary>
        /// initalises values to defaults
        /// </summary>
        public TaskGroup()
        {
            Tasks = new Task[1];
            ClientName = "";
            Length = 0;
            AdvanceCounts = new int[1];
            HasChanged = true;
            Type = "Unknown";
            VATPeriod = 0;
            AMLContactName = "";
            PayRollPeriod = "";
            PayRollInterval = "";
        }
        /// <summary>
        /// calls the parameter less constreuctor then sets the client name
        /// </summary>
        public TaskGroup(string clientName) : this()
        {
            ClientName = clientName;
        }
        /// <summary>
        /// advanaces all tasks including the parent task
        /// </summary>
        public void AdvanceAll()
        {
            Tasks[0].Advance(Intervals[0]);
            int parentAdvCnt = ++AdvanceCounts[0];
            for (int i = 1; i < Length; i++)
            {
                if (AdvanceCounts[i] < parentAdvCnt)
                {
                    Tasks[i].Advance(Intervals[i]);
                    AdvanceCounts[i]++;
                }
            }
            HasChanged = true;
        }
        /// <summary>
        /// advances the task at the given index if index == 0 thein advance all is count the parent isnt advanced twice
        /// </summary>
        public void Advance(int index)
        {
            if (index == 0)
            {
                AdvanceAll();
            }
            else
            {
                Tasks[index].Advance(Intervals[index]);
                AdvanceCounts[index]++;
            }
            HasChanged = true;
        }
        /// <summary>
        /// calls advance with the index of the object given
        /// </summary>
        public void Advance(Task task)
        {
            Advance(task.Index);
            HasChanged = true;
        }
        /// <summary>
        /// appends the client name in () to the tasks name and returns donet effect the task its self
        /// </summary>
        public string GetDisplayName(Task task)
        {
            if (Type == "Custom")
            {
                return task.Name;
            }
            return $"{task.Name} ({ClientName})";
        }
        /// <summary>
        /// used for todays task
        /// </summary>
        public DockPanel GetDockPanel(int index, Page page, Grid pGrid)
        {
            Task task = Tasks[index];
            DockPanel dockH = new DockPanel()
            {
                Margin = new Thickness(3, 0, 0, 10),
                Background = task.IsLate/* && e.ShowLate()*/ ? Brushes.Red : Brushes.Transparent
            };
            dockH.ToolTip = new TextBlock()
            {
                Text = $"{task.Date:dd/MM/yyyy}"
            };
            Ellipse ellipse = new Ellipse()
            {
                Fill = task.Colour,
                Height = 25,
                Width = 25,
                Margin = new Thickness(2, 4, 5, 0)
            };
            DockPanel.SetDock(ellipse, Dock.Left);

            TextBlock tb = new TextBlock()
            {
                Text = GetDisplayName(task),
                Background = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            DockPanel.SetDock(tb, Dock.Left);

            Button button = new Button()
            {
                Style = page.FindResource("ActionButton") as Style,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 2, 10, 0),
                Content = task.CanBeEdited ? "Remove" : "Advance"
            };
            DockPanel.SetDock(button, Dock.Right);

            dockH.Children.Add(ellipse);
            dockH.Children.Add(tb);
            dockH.Children.Add(button);

            dockH.SetBinding(FrameworkElement.WidthProperty, new Binding("ActualWidth")
            {
                Source = pGrid.RowDefinitions[0]
            });
            return dockH;
        }
        public StackPanel GetMonthStack(int index, out Ellipse ellipse, out TextBlock text)
        {
            Task task = Tasks[index];
            StackPanel stack = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, -11, 0, 10)
            };

            ellipse = new Ellipse()
            {
                Fill = task.Colour,
                Width = 20,
                Height = 20,
                Margin = new Thickness(0, 4, 5, 0),
            };
            ellipse.ToolTip = new TextBlock()
            {
                Text = GetDisplayName(task),
                FontSize = 18,
                Background = Brushes.AliceBlue
            };

            text = new TextBlock()
            {
                Text = GetDisplayName(task),
            };
            text.ToolTip = ellipse.ToolTip;
            if (task.Comment != "")
            {
                text.ToolTip = new TextBlock()
                {
                    Text = task.Comment,
                    FontSize = 18,
                    Background = Brushes.AliceBlue
                };
            }

            stack.Children.Add(ellipse);
            stack.Children.Add(text);
            return stack;
        }
        public void GetYearStack(int index, out Ellipse ellipse)
        {
            Task task = Tasks[index];
            ellipse = new Ellipse()
            {
                Fill = task.Colour,
                Width = 20,
                Height = 20,
                Margin = new Thickness(0, 4, 5, 0)
            };
            ellipse .ToolTip = new TextBlock()
            {
                Text = GetDisplayName(task),
                FontSize = 18,
                Background = Brushes.AliceBlue
            };
        }
        /// <summary>
        /// appends all names and the client name then hases that
        /// </summary>
        public override int GetHashCode()
        {
            string msg = "";
            foreach (string name in Names)
            {
                msg += name;
            }
            msg += ClientName;
            return msg.GetHashCode();
        }
        /// <summary>
        /// compares the hashes
        /// </summary>
        public override bool Equals(object obj)
        {
            TaskGroup group;
            try
            {
                group = obj as TaskGroup;
            }
            catch
            {
                return false;
            }
            int hash1 = GetHashCode();
            int hash2 = group.GetHashCode();
            return hash1 == hash2;
        }
        /// <summary>
        /// advances all tasks to the advance numbers specified doesnt call advance all
        /// </summary>
        public void AdvanceTo(int[] advCnt)
        {
            AdvanceCounts = advCnt;
            for (int i = 0; i < AdvanceCounts.Length; i++)
            {
                for (int j = 0; j < AdvanceCounts[i]; j++)
                {
                    Tasks[i].Advance(Intervals[i]);
                }
            }
        }
        /// <summary>
        /// Renams the task given it is an AML sets the tasks name and AMLContactName to name and Name[0] doesnt do anything else
        /// </summary>
        public void RenameAMLTask(string name)
        {
            if(Type != "AML")
            {
                return;
            }
            AMLContactName = name;
            Parent.Name = $"AML review due for {name}";
            Names[0] = Parent.Name;
            HasChanged = true;
        }

        #region Create Presets
        public static TaskGroup CreateCustom(string name, DateTime date, string colour, string comment)
        {
            TaskGroup group = new TaskGroup()
            {
                Type = "Custom",
                BaseDate = date.GetString(),
                Length = 1,
                ClientName = name,
                Names = new string[1]
                {
                    name
                },
                Intervals = new Interval[1]
                {
                    new Interval(0, 0, 0)
                },
                AdvanceCounts = new int[1]
                {
                    0
                }
            };
            group.Tasks = new Task[1]
            {
                new Task(group.Names[0], date, 0)
                {
                    CanAdvance = false,
                    HexColour = colour,
                },
            };
            group.Colour = colour;
            group.TaskComment = comment;
            return group;
        }
        public static TaskGroup CreateAPE(string clientName, DateTime baseDate)
        {
            TaskGroup group = new TaskGroup(clientName)
            {
                Type = "APE",
                BaseDate = baseDate.GetString(),
                Length = 8,
                Names = new string[8]
                {
                    "Year end",
                    "Request accounts info",
                    "Start to prepare accounts",
                    "Urgent prepare accounts",
                    "VERY urgent prepare accounts",
                    "CT600 due",
                    "Last date for filing accounts",
                    "Tax due HMRC",
                },
                Intervals = new Interval[8]
                {
                    new Interval(1, 0, 0),
                    new Interval(1, 0, 0)
                    {
                        ForceLastDayOfMonth = true
                    },
                    new Interval(1, 0, 0)
                    {
                        ForceDate = 14
                    },
                    new Interval(1, 0, 0)
                    {
                        ForceLastDayOfMonth = true
                    },
                    new Interval(1, 0, 0)
                    {
                        ForceLastDayOfMonth = true
                    },
                    new Interval(1, 0, 0),
                    new Interval(1, 0, 0)
                    {
                        ForceLastDayOfMonth = true
                    },
                    new Interval(1, 0, 0)
                    {
                        ForceLastDayOfMonth = true
                    },
                },
                AdvanceCounts = new int[8]
                {
                    0, 0, 0, 0, 0, 0, 0, 0
                }
            };
            group.Tasks = new Task[8]
            {
                new Task(group.Names[0], baseDate, 0, showPeriod: 1)
                {
                    HexColour = "#0000ff"
                },
                new Task(group.Names[1], baseDate.AddYears(-1).AddMonths(1).GetLastDay(), 1, showPeriod: 1)
                {
                    HexColour = "#0000ff"
                },
                new Task(group.Names[2], baseDate.AddYears(-1).AddMonths(4).SetDay(14),   2, showPeriod: 1)
                {
                    HexColour = "#0000ff"
                },
                new Task(group.Names[3], baseDate.AddYears(-1).AddMonths(7).GetLastDay(), 3, showPeriod: 1)
                {
                    HexColour = "#0000ff"
                },
                new Task(group.Names[4], baseDate.AddYears(-1).AddMonths(8).GetLastDay(), 4, showPeriod: 1)
                {
                    HexColour = "#0000ff"
                },
                new Task(group.Names[5], baseDate.AddYears(-1).AddDays(-1).GetLastDay(),  5, showPeriod: 1)
                {
                    HexColour = "#0000ff"
                },
                new Task(group.Names[6], baseDate.AddYears(-1).GetLastDay(),              6, showPeriod: 1)
                {
                    HexColour = "#0000ff"
                },
                new Task(group.Names[7], baseDate.AddMonths(-2).GetFirstDay(),            7, showPeriod: 1)
                {
                    HexColour = "#0000ff"
                }
            };
            return group;
        }
        public static TaskGroup CreateCA(string clientName, DateTime baseDate)
        {
            TaskGroup group = new TaskGroup(clientName)
            {
                Type = "CA",
                BaseDate = baseDate.GetString(),
                Length = 2,
                Names = new string[2]
                {
                    "Confirmation statement due",
                    "Submit Confirmation"
                },
                Intervals = new Interval[2]
                {
                    new Interval(1, 0, 0),
                    new Interval(1, 0, 0)
                },
                AdvanceCounts = new int[2]
                {
                    0, 0
                },
            };
            group.Tasks = new Task[2]
            {
                new Task(group.Names[0], baseDate, 0, false, showPeriod: 1)
                {
                    HexColour = "#00ffff"
                },
                new Task(group.Names[1], baseDate, 1, showPeriod: 10)
                {
                    HexColour = "#00ffff"
                }
            };
            return group;
        }
        public static TaskGroup CreateVATPE(string clientName, DateTime baseDate, int period)
        {
            TaskGroup group = new TaskGroup(clientName)
            {
                Type = "VATPE",
                BaseDate = baseDate.GetString(),
                Length = 3,
                Names = new string[3]
                {
                    "VAT period end",
                    "Request VAT info",
                    "File VAT"
                },
                Intervals = new Interval[3]
                {
                    new Interval(0, period, 0)
                    {
                        ForceLastDayOfMonth = true
                    },
                    new Interval(0, period, 0)
                    {
                        ForceDate = 5
                    },
                    new Interval(0, period, 0)
                    {
                        ForceDate = 7
                    },
                },
                AdvanceCounts = new int[3]
                {
                    0, 0, 0
                },
                VATPeriod = period,
            };
            group.Tasks = new Task[3]
            {
                new Task(group.Names[0], baseDate, 0, false, showPeriod: 1)
                {
                    HexColour = "#5555ff"
                },
                new Task(group.Names[1], baseDate.AddMonths(-2).SetDay(5), 1, showPeriod: 1)
                {
                    HexColour = "#FF0000"
                },
                new Task(group.Names[2], baseDate.AddMonths(-1).SetDay(7), 2, showPeriod: 1)
                {
                    HexColour = "#FF0000"
                },
            };
            return group;
        }
        public static TaskGroup CreateAML(string clientName, DateTime baseDate, string name)
        {
            TaskGroup group = new TaskGroup(clientName)
            {
                AMLContactName = name,
                Type = "AML",
                BaseDate = baseDate.GetString(),
                Length = 1,
                Names = new string[1]
                {
                    $"AML review due for {name}",
                },
                Intervals = new Interval[1]
                {
                    new Interval(1, 0, 0)
                },
                AdvanceCounts = new int[1]
                {
                    0
                },
            };
            group.Tasks = new Task[1]
            {
                new Task(group.Names[0], baseDate, 0, showPeriod: 1)
                {
                    HexColour = "#AAAAFF"
                }
            };
            return group;
        }
        public static TaskGroup CreateSA(string clientName)
        {
            DateTime baseDate = new DateTime(DateTime.Today.Year, 1, 31);
            TaskGroup group = new TaskGroup(clientName)
            {
                Type = "SA",
                BaseDate = baseDate.GetString(),
                Length = 4,
                Names = new string[4]
                {
                    "Self assessment due",
                    "File S.A",
                    "Request Info for S.A",
                    "Prepare S.A"
                },
                Intervals = new Interval[4]
                {
                    new Interval(1, 0, 0),
                    new Interval(1, 0, 0),
                    new Interval(1, 0, 0),
                    new Interval(1, 0, 0)
                },
                AdvanceCounts = new int[4]
                {
                    0, 0, 0, 0
                },
            };
            group.Tasks = new Task[4]
            {
                new Task(group.Names[0], baseDate, 0, showPeriod: 1, canBeLate: false)
                {
                    HexColour = "#FFFFFF00"
                },
                new Task(group.Names[1], baseDate.SetDay(5), 1, showPeriod: 1)
                {
                    HexColour = "#FFFFFF00"
                },
                new Task(group.Names[2], baseDate.AddYears(-1).SetMonth(5).SetDay(15), 2, showPeriod: 1)
                {
                    HexColour = "#FFFFFF00"
                },
                new Task(group.Names[3], baseDate.AddYears(-1).SetMonth(9).SetDay(15), 3, showPeriod: 1)
                {
                    HexColour = "#FFFFFF00"
                },
            };
            return group;
        }
        public static TaskGroup CreateP11D(string clientName)
        {
            DateTime baseDate = new DateTime(DateTime.Today.Year, 7, 6);
            TaskGroup group = new TaskGroup(clientName)
            {
                Type = "P11D",
                BaseDate = baseDate.GetString(),
                Length = 2,
                Names = new string[2]
                {
                    "P11D due",
                    "Prepare P11D"
                },
                Intervals = new Interval[2]
                {
                    new Interval(1, 0, 0),
                    new Interval(1, 0, 0)
                },
                AdvanceCounts = new int[2]
                {
                    0, 0
                },
            };
            group.Tasks = new Task[2]
            {
                new Task(group.Names[0], baseDate, 0, showPeriod: 1, canBeLate: false)
                {
                    HexColour = "#FFFF00FF"
                },
                new Task(group.Names[1], baseDate.SetMonth(5).SetDay(6), 1, showPeriod: 1)
                {
                    HexColour = "#FFFFFF00"
                }
            };
            return group;
        }
        public static TaskGroup CreateCISW(string clientName)
        {
            DateTime baseDate = DateTime.Today.SetDay(19);
            TaskGroup group = new TaskGroup(clientName)
            {
                Type = "CISW",
                BaseDate = baseDate.GetString(),
                Length = 1,
                Names = new string[1]
                {
                    "CIS Withheld",
                },
                Intervals = new Interval[1]
                {
                    new Interval(0, 1, 0)
                },
                AdvanceCounts = new int[1]
                {
                    0
                },
            };
            group.Tasks = new Task[1]
            {
                new Task(group.Names[0], baseDate, 0, showPeriod: 13)
                {
                    HexColour = "#FFAAAAAA"
                }
            };
            return group;
        }
        public static TaskGroup CreateCISS(string clientName)
        {
            DateTime baseDate = DateTime.Today.SetDay(19);
            TaskGroup group = new TaskGroup(clientName)
            {
                Type = "CISS",
                BaseDate = baseDate.GetString(),
                Length = 1,
                Names = new string[1]
                {
                    "CIS Suffered",
                },
                Intervals = new Interval[1]
                {
                    new Interval(0, 1, 0)
                },
                AdvanceCounts = new int[1]
                {
                    0
                },
            };
            group.Tasks = new Task[1]
            {
                new Task(group.Names[0], baseDate, 0, showPeriod: 13)
                {
                    HexColour = "#FFFFAAFF"
                }
            };
            return group;
        }
        public static TaskGroup CreatePayroll(string clientName, DateTime baseDate, string period, string interval)
        {
            Interval inter1 = new Interval();
            Interval inter2 = new Interval();
            switch (period)
            {
                case "Weekly":
                    inter1 = new Interval(0, 0, 7);
                    inter2 = new Interval(0, 0, 7);
                    break;
                case "2Weekly":
                    inter1 = new Interval(0, 0, 14);
                    inter2 = new Interval(0, 0, 14);
                    break;
                case "Monthly":
                    inter1 = new Interval(0, 1, 0);
                    inter2 = new Interval(0, 1, 0);
                    break;
            }
            switch (interval)
            {
                case "1": // 28th
                    inter1.ForceDate = 28;
                    break;
                case "2": // every other friday
                    break;
                case "3": // last day of month
                    inter1.ForceLastDayOfMonth = true;
                    break;
                case "4": // last friday of month
                    inter1.LastFriday = true;
                    break;
            }
            TaskGroup group = new TaskGroup(clientName)
            {
                Type = "Payroll",
                PayRollPeriod = period,
                PayRollInterval = interval,
                BaseDate = baseDate.GetString(),
                Length = 2,
                Names = new string[2]
                {
                    $"Payroll {period}", $"Prepare Payroll {period}"
                },
                Intervals = new Interval[2]
                {
                    inter1,
                    inter2,
                },
                AdvanceCounts = new int[2]
                {
                    0, 0
                },
            };
            group.Tasks = new Task[2]
            {
                new Task(group.Names[0], baseDate, 0, canBeLate: false)
                {
                    HexColour = "#FF00FF00"
                },
                new Task(group.Names[1], baseDate.AddDays(-2), 1)
                {
                    HexColour = "#FF00FF00"
                }
            };
            return group;
        }
        #endregion
    }
}
