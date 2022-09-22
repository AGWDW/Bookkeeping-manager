using Bookkeeping_manager.Scripts;
using Bookkeeping_manager.src;
using Bookkeeping_manager.src.Tasks;
using System;
using System.Windows;
using System.Windows.Media;

namespace Bookkeeping_manager.Windows.UtilityWindows
{

    internal enum ViewerState
    {
        Adding, Advancing, Removing
    }
    /// <summary>
    /// Interaction logic for EventsViewer.xaml
    /// </summary>
    public partial class EventViewer : Window
    {
        public Task Task { get; set; }
        private ViewerState State { get; set; }
        public EventViewer(Task task, bool creating)
        {
            if(task is null)
            {
                task = TaskManager.GetOrCreate(0, TaskType.Static, out _);
            }
            InitializeComponent();
            DataContext = task; 
            Task = task;
            if (task is StaticTask)
            {
                if (creating)
                {
                    State = ViewerState.Adding;
                }
                else
                {
                    State = ViewerState.Removing;
                }
            }
            else
            {
                State = ViewerState.Advancing;
            }
            ApplyState();
        }

        public void ApplyState()
        {
            EventDate.Text = Task.Date;
            switch (State)
            {
                case ViewerState.Adding:
                    ActionButton.Content = "Add and Close";
                    SaveButton.Content = "Close";
                    EventDate.Text = "";
                    break;
                case ViewerState.Removing:
                    ActionButton.Content = "Remove and Close";
                    break;
                case ViewerState.Advancing:
                    ActionButton.Content = "Advance and Close";

                    NameTB.Style = FindResource("ReadOnlyTB") as Style;
                    EventDate.Style = FindResource("ReadOnlyTB") as Style;
                    CommentBox.Style = FindResource("ReadOnlyLargeTB") as Style;
                    EventColour.Focusable = false;
                    EventColour.InputBindings.Clear();

                    break;
            }
        }

        /// <summary>
        /// will verify the date and change the format to include /'s
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventDate_LostFocus(object sender, RoutedEventArgs e)
        {
            var t = DataEnforce.Date("", EventDate.Text.Trim(), false).ToDate();
            if (t < DateTime.Now.Date)
            {
                MessageBox.Show("Event cannot occur in the past");
                EventDate.Text = Task.GetDate();
                return;
            }
            Task.SetDate(t);
            EventDate.Text = t.GetString();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if(State == ViewerState.Adding)
            {
                TaskManager.DeleteTask(Task.UID);
            }
            Close();
        }

        private void Advance__Add__ButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res;
            switch (State)
            {
                case ViewerState.Adding:
                    break;
                case ViewerState.Removing:
                    res = MessageBox.Show("Are you sure you want to remove this task", "", MessageBoxButton.YesNo);
                    if(res == MessageBoxResult.Yes)
                    {
                        Task.Advance();
                    }
                    else
                    {
                        return;
                    }
                    break;
                case ViewerState.Advancing:
                    res = MessageBox.Show("Are you sure you want to advance this task", "", MessageBoxButton.YesNo);
                    if(res == MessageBoxResult.Yes)
                    {
                        Task.Advance();
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DatabaseConnection.UpdateTask(Task.UID);
        }

        private void EventColour_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            /*if (!Event.CanBeEdited)
            {
                EventColour.SelectedColor = Event.Colour.ToColour(true);
            }
            else
            {
                Event.ColourType = EventColour.SelectedColor.GetValueOrDefault().ToString("").ToLower();
            }*/
        }
    }
}
