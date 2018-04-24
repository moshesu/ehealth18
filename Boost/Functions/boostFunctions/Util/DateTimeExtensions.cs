using System;

namespace boostFunctions.Util
{
    public static class DateTimeExtensions
    {
	    public static bool WithinYearRange(this DateTime date, DateTime rangeCenter, int yearRange = 5)
	    {
		    var start = rangeCenter.AddYears(-yearRange);
		    var end = rangeCenter.AddYears(yearRange);

		    var inRange = (date >= start && date <= end);
			return inRange;
	    }
	}
}
