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
using System.Windows.Shapes;
using Bookkeeping_manager.Scripts;

namespace Bookkeeping_manager.Windows.UtilityWindows
{
    /// <summary>
    /// Interaction logic for EventsViewer.xaml
    /// </summary>
    public partial class EventViewer : Window
    {
        private bool delete = true;
        public Event Event { get; set; }
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

        private void EventDate_LostFocus(object sender, RoutedEventArgs e)
        {
            var t = DataEnforce.Date(Event.Date.ToString("dd/MM/yyyy"), EventDate.Text.Trim(), false).ToDate();
            if(t < DateTime.Now.Date)
            {
                MessageBox.Show("Event cannot occur in the past");
                EventDate.Text = Event.Date.GetString();
                return;
            }
            Event.Date = t;
            EventDate.Text = t.GetString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            delete = false;
            Close();
        }

        private void AdvanceButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to {(Event.CanBeEdited ? "remove" : "advance")} the Task", "", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                // Yes
                delete = !Event.Advance(1);
                Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Event.Changed = true;
            if(!Event.Delete)
                Event.Delete = delete;
        }

        private void EventColour_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (!Event.CanBeEdited)
            {
                EventColour.SelectedColor = Event.Colour.ToColour(true);
            }
            else
            {
                Event.ColourType = EventColour.SelectedColor.GetValueOrDefault().ToString("").ToLower();
            }
        }
    }
}
