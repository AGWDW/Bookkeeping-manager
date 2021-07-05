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
        public string Next { get; set; }
        private List<Event> Events { get; set; }

        public Home(List<Event> events)
        {
            Next = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy");
            Events = events;
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
            for(int j = 0; j < Events.Count; j++)
            {
                Event e = Events[j];
                bool isLate = e.IsLate();
                if (e.Date != DateTime.Now.Date && !isLate || e.Delete)
                {
                    continue;
                }
                DockPanel dockH = new DockPanel()
                {
                    Margin = new Thickness(3, 0, 0, 10),
                    Background = isLate && e.ShowLate() ? Brushes.Red : Brushes.Transparent
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
                    Margin = new Thickness(2, 4, 5, 0)
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
                    Margin = new Thickness(0, 2, 10, 0),
                    Content = e.CanBeEdited ? "Remove" : "Advance"
                };
                button.Click += (o, e_) =>
                {
                    if (!e.Advance(1))
                    {
                        // Events.Remove(e);
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

                TodaysTasks.Children.Add(dockH);
            }
        }
        public void CreateNextTasks()
        {
            NextTasks.Children.Clear();

            foreach (Event e in Events)
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
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var t = DataEnforce.Date(Next, NextDate.Text.Trim(), false).ToDate();
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
            CreateNextTasks();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Event @event = new Event(name:"", canBeEdited:true, initalDate:DateTime.Today);
            UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(@event, true);
            viewer.ShowDialog();
            if (@event.Delete)
                return;
            Events.Add(@event);

            CreateTodaysTasks();
            CreateNextTasks();
        }

    }
}
