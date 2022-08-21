using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Tasks;
using Task = Bookkeeping_manager.src.Tasks.Task;

namespace Bookkeeping_manager.Windows
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public string Today { 
            get
            {
                return DateTime.Now.ToString("dd/MM/yyyy");
            } 
        }

        public Home(List<Event> events)
        {
            InitializeComponent();
            DataContext = this;
            PopulateWithTasks();

           HorizontalSepration.SetBinding(Line.Y1Property, new Binding("ActualHeight")
           {
               Source = PrimaryGrid
           });
        }
        internal void PopulateWithTasks()
        {
            TodaysTasks.Children.Clear();
            NextTasks.Children.Clear();

            foreach(Task task in TaskManager.AllTasks)
            {
                bool isLate = task.State == TaskState.Late;
                DockPanel dockH = new DockPanel()
                {
                    Margin = new Thickness(3, 0, 0, 10),
                    Background = isLate ? Brushes.Red : Brushes.Transparent
                };
                TextBlock tb = new TextBlock()
                {
                    Text = task.Name,
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };
                DockPanel.SetDock(tb, Dock.Left);
                Button button = new Button()
                {
                    Style = FindResource("ActionButton") as Style,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Margin = new Thickness(0, 2, 10, 0),
                    Content = task is StaticTask ? "Remove" : "Advance"
                };
                button.Click += (o, e_) =>
                {
                    task.Advance();
                    PopulateWithTasks();
                };
                DockPanel.SetDock(button, Dock.Right);

                dockH.ToolTip = task.GetDate();

                if(task is StaticTask)
                {
                    dockH.MouseUp += (o, e_) =>
                    {
                        UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(task, false);
                        viewer.ShowDialog();
                        PopulateWithTasks();
                    };
                }
                
                dockH.Children.Add(tb);
                dockH.Children.Add(button);

                dockH.SetBinding(WidthProperty, new Binding("ActualWidth")
                {
                    Source = PrimaryGrid.RowDefinitions[0]
                });
                if (task.State == TaskState.Future)
                {
                    // check if for the next category
                    NextTasks.Children.Add(dockH);
                }
                else
                {
                    // put today
                    TodaysTasks.Children.Add(dockH);
                }
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            /*var t = DataEnforce.Date(Next, NextDate.Text.Trim(), false).ToDate();
            if (t < DateTime.Now.Date)
            {
                MessageBox.Show("Cannot view the past");
                NextDate.Text = Next;
                return;
            }
            Next = t.ToString("dd/MM/yyyy");
            NextDate.Text = t.ToString("dd/MM/yyyy");
            Keyboard.ClearFocus();

            CreateTodaysTasks();
            CreateNextTasks();*/
        }

        private void CreateTaskClick(object sender, RoutedEventArgs e)
        {
            UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(null, true);
            viewer.ShowDialog();
            PopulateWithTasks();
        }

    }
}
