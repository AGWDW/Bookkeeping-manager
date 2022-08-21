using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Tasks;
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
        private List<Task> Tasks
        {
            get => TaskManager.AllTasks;
        }
        public YearView()
        {
            year = DateTime.Now.ToString("yyyy");
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
            foreach (Task t in Tasks)
            {
                DateTime date = t.GetDate().Replace("Due: ", "").ToDate();
                if (date.ToString("yyyy") != CurrentYear)
                {
                    continue;
                }
                int column = date.Month + 11;
                int row = date.Day - 1;
                StackPanel stackH = (YearGrid.Children[column] as StackPanel).Children[row] as StackPanel;
                Ellipse ellipse = new Ellipse()
                {
                    Fill = new SolidColorBrush(Colors.Red),
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(0, 4, 5, 0)
                };
                ellipse.MouseUp += (o, e_) =>
                {
                    UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(t, false);
                    viewer.ShowDialog();

                    ReDraw();
                };
                stackH.Children.Add(ellipse);
            }

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
        private void AddNewTask(object sender, RoutedEventArgs e)
        {
            UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(null, true);
            viewer.ShowDialog();

            ReDraw();
        }
    }
}
