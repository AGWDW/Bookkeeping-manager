﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src.Tasks;

namespace Bookkeeping_manager.Windows
{
    /// <summary>
    /// Interaction logic for MonthView.xaml
    /// </summary>
    public partial class MonthView : Page
    {
        string sDay, eDay;
        private List<Task> Tasks
        {
            get => TaskManager.AllTasks;
        }
        public string StartDate
        {
            get => sDay;
            set
            {
                sDay = DataEnforce.Date(sDay, value, false);
                CreateGrid();
            }
        }
        public string EndDate
        {
            get => eDay;
            set
            {
                eDay = DataEnforce.Date(eDay, value, false);
                CreateGrid();
            }
        }
        public MonthView()
        {
            sDay = DateTime.Today.GetString();
            eDay = DateTime.Today.AddMonths(1).GetString();
            InitializeComponent();
            DataContext = this;
            CreateGrid();
        }
        private void CreateGrid()
        {
            MonthGrid.Children.Clear();
            int row = 1;
            DateTime current = sDay.ToDate();
            DateTime end = eDay.ToDate();
            int j = (current.DayOfWeek == 0) ? 7 : (int)current.DayOfWeek;
            j--;
            int col = j % 7;
            bool nextMonth = false;
            while (current <= end)
            {
                StackPanel dayGrid = new StackPanel()
                {
                    Margin = new Thickness(3),
                    Orientation = Orientation.Vertical,
                    Background = Brushes.White
                };

                TextBlock date = new TextBlock()
                {
                    Text = $"{current.Day} {current.Month.AsMonth()}",
                    FontSize = 18,
                    Margin = new Thickness(2),
                    FontWeight = nextMonth ? FontWeights.Bold : FontWeights.Normal
                };

                dayGrid.Children.Add(date);

                ScrollViewer scrollViewer = new ScrollViewer()
                {
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                };
                scrollViewer.Content = dayGrid;

                Grid.SetRow(scrollViewer, row);
                Grid.SetColumn(scrollViewer, col++);
                MonthGrid.Children.Add(scrollViewer);

                if (!nextMonth && current.Day == DateTime.DaysInMonth(current.Year, current.Month))
                {
                    nextMonth = true;
                }
                current = current.AddDays(1);
                if(col % 7 == 0)
                {
                    row++;
                    col = 0;
                    if (row-1 >= 6)
                        break;
                }
            }
            PopulateGrid();
        }
        /// <summary>
        /// Fills the Grid with Events
        /// </summary>
        private void PopulateGrid()
        {
            string[] days = new string[7] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            for(int i = 0; i < 7; i++)
            {
                TextBlock day = new TextBlock()
                {
                    Text = days[i],
                    FontSize = 24,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                Grid.SetColumn(day, i);
                MonthGrid.Children.Add(day);
            }
            foreach (Task t in Tasks)
            {
                DateTime date = t.Date.ToDate();
                if (date < EndDate.ToDate() && date >= StartDate.ToDate())
                {
                    int column = date.GetDayOfWeek(); // the day of the week or column + 1
                    DateTime tempDay = StartDate.ToDate();
                    int row = 0;
                    while (tempDay != date)
                    {
                        tempDay = tempDay.AddDays(1);
                        if(tempDay.GetDayOfWeek() == 1)
                        {
                            row++;
                        }
                    }
                    if (row >= 6)
                        continue;
                    // the controles asociated with the event
                    StackPanel stack = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, -11, 0, 10)
                    };

                    Ellipse ellipse = new Ellipse()
                    {
                        Fill = new SolidColorBrush(Colors.Red),
                        Width = 20,
                        Height = 20,
                        Margin = new Thickness(0, 4, 5, 0),
                    };
                    ellipse.MouseUp += (o, e_) =>
                    {
                        UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(t, false);
                        viewer.ShowDialog();

                        StartDate = StartDate;
                    };

                    TextBlock text = new TextBlock()
                    {
                        Text = t.Name,
                    };
                    text.MouseUp += (o, e_) =>
                    {
                        UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(t, false);
                        viewer.ShowDialog();

                        StartDate = StartDate;
                    };
                    if(t is StaticTask)
                    {
                        if ((t as StaticTask).Comment != "")
                        {
                            text.ToolTip = new TextBlock()
                            {
                                Text = (t as StaticTask).Comment,
                                FontSize = 18,
                                Background = Brushes.AliceBlue
                            };
                        }
                    }
                    

                    stack.Children.Add(ellipse);
                    stack.Children.Add(text);
                    ((MonthGrid.GetAtGridPos(row + 1, column - 1) as ScrollViewer).Content as StackPanel).Children.Add(stack);
                }
            }
        } 

        private void PrevMonth_Click(object sender, RoutedEventArgs e)
        {
            sDay = sDay.ToDate().AddMonths(-1).GetString();
            eDay = eDay.ToDate().AddMonths(-1).GetString();
            CreateGrid();
            StartDay.Text = sDay;
            EndDay.Text = eDay;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            StartDate = StartDay.Text;
            EndDate = EndDay.Text;
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            dialog.PrintVisual(MonthGrid, "The Month Grid Printing");
        }

        /// <summary>
        /// Adds new event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {

            UtilityWindows.EventViewer viewer = new UtilityWindows.EventViewer(null, true);
            viewer.ShowDialog();


            CreateGrid();
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            sDay = sDay.ToDate().AddMonths(1).GetString();
            eDay = eDay.ToDate().AddMonths(1).GetString();
            CreateGrid();
            StartDay.Text = sDay;
            EndDay.Text = eDay;
        }
    }
}
