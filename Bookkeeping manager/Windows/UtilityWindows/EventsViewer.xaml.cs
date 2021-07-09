using Bookkeeping_manager.Scripts;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Bookkeeping_manager.Windows.UtilityWindows
{
    /// <summary>
    /// Interaction logic for EventsViewer.xaml
    /// </summary>
    public partial class EventViewer : Window
    {
        private bool delete = true;
        public Event Event { get; set; }
        public Tasks.Task Task
        {
            get => TaskGroup[TaskIndex];
            set => TaskGroup[TaskIndex] = value;
        }
        public Tasks.TaskGroup TaskGroup { get; private set; }
        public int TaskIndex { get; private set; }
        private bool Creating { get; set; }
        public bool ShouldCreate { get; private set; }
        public EventViewer(Event @event, bool creating = false)
        {
            Event = @event;
            Color c = Event.Colour.ToColour(true);
            DataContext = this;
            InitializeComponent();
            EventColour.SelectedColor = c;
            if (!@event.CanBeEdited)
            {
                AdvanceButton.Content = "Advance";

                NameTB.Style = FindResource("ReadOnlyTB") as Style;
                EventDate.Style = FindResource("ReadOnlyTB") as Style;
                CommentBox.Style = FindResource("ReadOnlyLargeTB") as Style;
                EventColour.Focusable = false;
                EventColour.InputBindings.Clear();
            }
            delete = creating;
            if (!creating)
                EventDate.Text = Event.Date.GetString();
        }
        public EventViewer(Tasks.TaskGroup taskGroup, Tasks.Task task, bool creating = false)
        {
            TaskGroup = taskGroup;
            TaskIndex = task.Index;
            DataContext = this;
            InitializeComponent();
            EventColour.SelectedColor = Task.Colour.Color;
            EventColour.SelectedColorChanged += EventColour_SelectedColorChanged;
            if (Task.CanAdvance)
            {
                AdvanceButton.Content = "Advance";

                NameTB.Style = FindResource("ReadOnlyTB") as Style;
                EventDate.Style = FindResource("ReadOnlyTB") as Style;
                CommentBox.Style = FindResource("ReadOnlyLargeTB") as Style;
                EventColour.Focusable = false;
                EventColour.InputBindings.Clear();
            }
            Creating = creating;
            delete = creating;

            EventDate.Text = Task.Date.GetString();
        }
        /// <summary>
        /// checks date feild and asignes to task when clicking off the date box never occours when in readonly mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventDate_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime t = DataEnforce.Date(Task.Date.GetString(), EventDate.Text.Trim(), false).ToDate();
            if (t < DateTime.Today)
            {
                MessageBox.Show("Event cannot occur in the past");
                EventDate.Text = Task.Date.GetString();
                return;
            }
            Task.Date = t;
            TaskGroup.BaseDate = t.GetString();
            EventDate.Text = t.GetString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            delete = false;
            Close();
        }

        /// <summary>
        /// both the advance and or delte button will delete it if it can then closes
        /// </summary>
        private void AdvanceButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to {(Task.CanBeEdited ? "remove" : "advance")} the Task", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                if (Task.CanAdvance)
                {
                    TaskGroup.Advance(TaskIndex);
                }
                else if (Creating)
                {
                    // called delte on a new task
                    ShouldCreate = false;
                }
                else
                {
                    // delte a pre existing custom task
                    DataHandler.RemoveTask(TaskGroup);
                }
                Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (TaskGroup.Type == "Custom")
            {
                TaskGroup.ClientName = Task.Name;
            }
            TaskGroup.HasChanged = true;
        }

        /// <summary>
        /// if the task can be edited will set its colour to the new one otherwise it doesnt change the colour
        /// </summary>
        private void EventColour_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (Task.CanAdvance)
            {
                EventColour.SelectedColor = Task.Colour.Color;
            }
            else
            {
                Task.HexColour = EventColour.SelectedColor.GetValueOrDefault().ToString();
            }
        }
    }
}
