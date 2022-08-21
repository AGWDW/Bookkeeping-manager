using Bookkeeping_manager.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping_manager.src
{
    public static class Constants
    {
        public static DateTimeInterval YEAR  = new DateTimeInterval(1, 0, 0);
        public static DateTimeInterval MONTH = new DateTimeInterval(0, 1, 0);
        public static DateTimeInterval DAY   = new DateTimeInterval(0, 0, 1);
    }
}
