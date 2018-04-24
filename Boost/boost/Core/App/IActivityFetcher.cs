using System;
using boost.Core.Entities;

namespace boost.Core.App
{
	public interface IActivityFetcher
	{
		Activity GetActivity(ActivityType type, DateTime day, string userId = null);
		Activity[] GetActivities(DateTime day, string userId = null);
		Activity[] GetActivitiesDuringSpan(DateTime start, DateTime end, string userId = null);
		Activity[] GetActivitiesForMonth(DateTime dayInMonth, string userId = null);
	}
}
