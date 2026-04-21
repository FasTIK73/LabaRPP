using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPP.Extensions;

public static class DateTimeExtensions
{
    public static bool IsDateNotOlder(this DateTime date, DateTime olderDate)
    {
        return date >= olderDate;
    }

    public static bool IsDateInRange(this DateTime date, DateTime fromDate, DateTime toDate)
    {
        return date >= fromDate && date <= toDate;
    }
}
