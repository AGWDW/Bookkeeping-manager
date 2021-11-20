using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.Tasks;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Bookkeeping_manager.Windows
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public string Today => DateTime.Today.GetString();
        public string Next { get; set; }
        private IndexSet<TaskGroup> Tasks
        {
            get => DataHandler.AllTasks;
            set => DataHandler.AllTasks = value;
        }

        public Home()
        {
            Next = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");
            // Events = events;
            InitializeComponent();
            DataContext = this;
            CreateTodaysTasks();
            CreateNextTasks();
            NextDate.Text = Next;

            HorizontalSepration.SetBinding(Line.Y1Property, new Binding("ActualHeight")
            {
                Source = PrimaryGrid
            });
        }
        public void CreateTodaysTasks()
        {
            TodaysTasks.Children.Clear();
            for (int i = 0; i < Tasks.Length; i++)
            {
                TaskGroup group = Tasks[i];
                for (int j = 0; j < group.Length; j++)
                {
                    Task task = group[j];
                    if (task.Date > Today.ToDate())
                    {
                        continue;
                    }
                    DockPanel dock = group.GetDockPanel(j, this, PrimaryGrid);
                    dock.MouseUp += (o, e) =>
                    {
                        UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(group, task);
                        viewer.ShowDialog();

                        CreateTodaysTasks();
                        CreateNextTasks();
                    };
                    Button button = dock.Children[2] as Button;
                    button.Click += (o, e) =>
                    {

                        if (task.CanAdvance)
                        {
                            group.Advance(task);
                        }
                        else
                        {
                            DataHandler.RemoveTask(group);
                        }

                        CreateTodaysTasks();
                        CreateNextTasks();
                    };
                    TodaysTasks.Children.Add(dock);
                }
            }
        }
        public void CreateNextTasks()
        {
            NextTasks.Children.Clear();
            for (int i = 0; i < Tasks.Length; i++)
            {
                TaskGroup group = Tasks[i];
                for (int j = 0; j < group.Length; j++)
                {
                    Task task = group[j];
                    if (task.Date != Next.ToDate())
                    {
                        continue;
                    }
                    DockPanel dock = group.GetDockPanel(j, this, PrimaryGrid);
                    dock.MouseUp += (o, e) =>
                    {
                        UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(group, task);
                        viewer.ShowDialog();

                        CreateTodaysTasks();
                        CreateNextTasks();
                    };
                    Button button = dock.Children[2] as Button;
                    button.Click += (o, e) =>
                    {

                        if (task.CanAdvance)
                        {
                            group.Advance(task);
                        }
                        else
                        {
                            DataHandler.RemoveTask(group);
                        }

                        CreateTodaysTasks();
                        CreateNextTasks();
                    };
                    NextTasks.Children.Add(dock);
                }
            }
            /*foreach (Event e in Events)
            {
                if (e.Date != Next.ToDate() || e.Delete)
                {
                    continue;
                }
                DockPanel dockH = new DockPanel()
                {
                    Margin = new Thickness(8, 0, 0, 2)
                };
                dockH.MouseUp += (o, e_) =>
                {
                    UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(e);
                    viewer.ShowDialog();

                    CreateTodaysTasks();
                    CreateNextTasks();
                };
                Ellipse ellipse = new Ellipse()
                {
                    Fill = e.Colour.ToColour(),
                    Height = 25,
                    Width = 25,
                    Margin = new Thickness(0, 4, 5, 0)
                };
                DockPanel.SetDock(ellipse, Dock.Left);

                TextBlock tb = new TextBlock()
                {
                    Text = e.DisplayName,
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                DockPanel.SetDock(tb, Dock.Left);

                Button button = new Button()
                {
                    Style = FindResource("ActionButton") as Style,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 0, 10, 0),
                    Content = e.CanBeEdited ? "Remove" : "Advance"
                };
                button.Click += (o, e_) =>
                {
                    if (!e.Advance(1))
                    {
                        e.Delete = true;
                    }

                    CreateTodaysTasks();
                    CreateNextTasks();
                };
                DockPanel.SetDock(button, Dock.Right);

                dockH.Children.Add(ellipse);
                dockH.Children.Add(tb);
                dockH.Children.Add(button);

                dockH.SetBinding(WidthProperty, new Binding("ActualWidth")
                {
                    Source = PrimaryGrid.RowDefinitions[0]
                });

                NextTasks.Children.Add(dockH);
            }*/
        }

        /// <summary>
        /// used to update the task lists when u click away from the date box above the future tasks cant select past
        /// </summary>
        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DateTime t = DataEnforce.Date(Next, NextDate.Text.Trim(), false).ToDate();
            if (t < DateTime.Now.Date)
            {
                MessageBox.Show("Cannot view the past");
                NextDate.Text = Next;
                return;
            }
            Next = t.GetString();
            NextDate.Text = t.GetString();
            Keyboard.ClearFocus();

            CreateTodaysTasks();
            CreateNextTasks();
        }

        /// <summary>
        /// opens event vier cand creates the event updates lists adds the Events
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*Event @event = new Event(name: "", canBeEdited: true, initalDate: DateTime.Today);
            UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(@event, true);
            viewer.ShowDialog();
            if (@event.Delete)
                return;
            Events.Add(@event);

            CreateTodaysTasks();
            CreateNextTasks();*/
            TaskGroup task = TaskGroup.CreateCustom("", DateTime.Today, "#FF000000", "");
            Tasks.Add(task);
            UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(task, task[0], creating: true);
            viewer.ShowDialog();

            CreateTodaysTasks();
            CreateNextTasks();
        }

    }
}
