using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Useful_CSharp_Extension_Methods
{
    public static class DatetimeExtensions
    {
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException

            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue)
            {
                return dateTime; // do not modify "guard" values
            }
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }

        public static string ClearSecondsAndMilliseconds(this DateTime? datetime)
        {
            if (datetime.HasValue)
            {
                var dt = datetime.Value;

                var milliseconds = dt.Millisecond;
                var secconds = dt.Second;
                var cdate = dt;
                cdate.AddMilliseconds(-1 * milliseconds).AddSeconds(-1 * secconds);
                return cdate.ToString();
            }
            else
            {
                return "";
            }

        }
    }
}
