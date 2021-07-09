using Bookkeeping_manager.Scripts;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Bookkeeping_manager.Windows
{
    /// <summary>
    /// Interaction logic for YearView.xaml
    /// </summary>
    public partial class YearView : Page
    {
        private string year;
        public string CurrentYear
        {
            get => year;
            set
            {
                if (int.Parse(value) <= 0)
                    return;
                year = DataEnforce.Integer(year, value, false);
                ReDraw();
            }
        }
        private IndexSet<Tasks.TaskGroup> Tasks
        {
            get => DataHandler.AllTasks;
            set => DataHandler.AllTasks = value;
        }
        private List<Event> Events
        {
            get => DataHandler.AllEvents;
            set => DataHandler.AllEvents = value;
        }
        public YearView(List<Event> events)
        {
            year = DateTime.Now.ToString("yyyy");
            Events = events;
            DataContext = this;
            InitializeComponent();
            CreateGrid();
            PopulateGrid();
        }
        public void CreateGrid()
        {
            for (byte i = 0; i < 12; i++)
            {
                StackPanel stackV = new StackPanel()
                {
                    Orientation = Orientation.Vertical
                };
                Grid.SetColumn(stackV, i);
                Grid.SetRow(stackV, 1);
                for (byte j = 0; j < DateTime.DaysInMonth(int.Parse(CurrentYear), i + 1); j++)
                {
                    StackPanel stackH = new StackPanel()
                    {
                        Background = DateTime.Today == new DateTime(int.Parse(CurrentYear), i + 1, j + 1) ? Brushes.PaleVioletRed : Brushes.White,
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(2)
                    };

                    stackH.Children.Add(new TextBlock()
                    {
                        Text = (j + 1).ToString()
                    });
                    stackV.Children.Add(stackH);
                }
                YearGrid.Children.Add(stackV);
            }
        }
        public void PopulateGrid()
        {
            for (int i = 0; i < Tasks.Length; i++)
            {
                Tasks.TaskGroup group = Tasks[i];
                for (int j = 0; j < group.Length; j++)
                {
                    Tasks.Task task = group[j];
                    if (task.Date.ToString("yyyy") != CurrentYear/* || e.Delete*/)
                    {
                        continue;
                    }

                    int column = task.Date.Month + 11;
                    int row = task.Date.Day - 1;

                    StackPanel stackH = (YearGrid.Children[column] as StackPanel).Children[row] as StackPanel;
                    group.GetYearStack(j, out Ellipse ellipse);
                    ellipse.MouseUp += (o, e_) =>
                    {
                        UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(group, task);
                        viewer.ShowDialog();

                        ReDraw();
                    };
                    stackH.Children.Add(ellipse);
                }
            }
            /*foreach (Event e in Events)
            {
                if (e.Date.ToString("yyyy") != CurrentYear || e.Delete)
                    continue;
                int column = e.Date.Month + 11;
                int row = e.Date.Day - 1;
                StackPanel stackH = (YearGrid.Children[column] as StackPanel).Children[row] as StackPanel;
                Ellipse ellipse = new Ellipse()
                {
                    Fill = e.Colour.ToColour(),
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(0, 4, 5, 0)
                };
                ellipse.MouseUp += (o, e_) =>
                {
                    UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(e);
                    viewer.ShowDialog();

                    ReDraw();
                };
                stackH.Children.Add(ellipse);
            }*/

        }
        public void ReDraw()
        {
            YearGrid.Children.RemoveRange(12, YearGrid.Children.Count);
            CreateGrid();
            PopulateGrid();
        }
        /// <summary>
        /// Adds new event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Event @event = new Event(name: "", canBeEdited: true, initalDate: DateTime.Today);
            UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(@event, true);
            viewer.ShowDialog();
            if (!@event.Delete)
                Events.Add(@event);

            ReDraw();
        }
    }
}
