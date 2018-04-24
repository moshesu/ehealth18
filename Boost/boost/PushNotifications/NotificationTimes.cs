using System;

namespace boost.PushNotifications
{
	public class NotificationTimes
	{
		public static int CrystalsHour = 16;
		public static int NextMorning = 32;
		public static int ComparisonMinutes = 1000;

		public static DateTime GetTimeToNotify(int hours)
		{
			var now = DateTime.Now;

			if(now.Hour >= hours) 
				return DateTime.Today.AddDays(1).AddHours(hours);

			return DateTime.Today.AddHours(hours);
		}
	}
}