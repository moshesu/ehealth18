using System;

namespace boost.Util
{
	public static class DateTimeExtensions
	{
		public static string ToApiTime(this DateTime time) => time.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");

		public static DateTime StartOfWeek(this DateTime time)
		{
			//Currently set for Israeli users
			int diff = (7 + (time.DayOfWeek - DayOfWeek.Sunday)) % 7;

			return time.AddDays(-1 * diff).Date;
		}

		public static DateTime FirstDayOfMonth(this DateTime day)
		{
			return new DateTime(day.Year, day.Month, 1);
		}

		public static int DaysInMonth(this DateTime day)
		{
			return DateTime.DaysInMonth(day.Year, day.Month);
		}

		public static DateTime LastDayOfMonth(this DateTime day)
		{
			return new DateTime(day.Year, day.Month, day.DaysInMonth());
		}
	}
}
