using Bookkeeping_manager.Scripts;
using System.Windows.Media;

namespace Bookkeeping_manager.src
{
    public static class Constants
    {
        public static DateTimeInterval YEAR = new DateTimeInterval(1, 0, 0);
        public static DateTimeInterval MONTH = new DateTimeInterval(0, 1, 0);
        public static DateTimeInterval WEEK = new DateTimeInterval(0, 0, 7);
        public static DateTimeInterval DAY = new DateTimeInterval(0, 0, 1);
        // colours
        public static SolidColorBrush DEFAULT_COLOUR = new SolidColorBrush(Colors.Red);

        public static SolidColorBrush CONFIRMATION_DUE_COLOUR = new SolidColorBrush(Colors.Red);
        public static SolidColorBrush SUBMIT_CONFIRMATION_COLOUR = new SolidColorBrush(Colors.Red);

        public static SolidColorBrush AML_DUE_COLOUR = new SolidColorBrush(Colors.Red);

    }
}
