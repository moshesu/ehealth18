using System;
using System.Collections.Generic;
using System.Linq;
using boost.Core.App;
using boost.Core.Entities;
using boost.Core.Entities.Users;
using boost.Repositories;
using boost.Util;

namespace boost.Core.UserActivity
{
	public class ActivityFetcher : IActivityFetcher
	{
		private readonly ILocalStorage _localStorage;
		private readonly IActivityRepository _activityRepository;
		private readonly IActivityBuilder _activityBuilder;

		public ActivityFetcher(ILocalStorage localStorage, IActivityRepository activityRepository, IActivityBuilder activityBuilder)
		{
			_localStorage = localStorage; 
			_activityRepository = activityRepository;
			_activityBuilder = activityBuilder;
		}

		public Activity[] GetActivitiesForMonth(DateTime dayInMonth, string userId=null)
		{
			var start = dayInMonth.FirstDayOfMonth();
			var end = (dayInMonth.Month.Equals(DateTime.Today.Month)) ? DateTime.Today : dayInMonth.LastDayOfMonth();

			return GetActivitiesDuringSpan(start, end, userId);
		}

		public Activity[] GetActivitiesDuringSpan(DateTime start, DateTime end, string userId = null)
		{
			if (userId == null)
				userId = _localStorage.GetCurrentUserId();

			var todaysActivities = new List<Activity>[0];
			if (end.Date.Equals(DateTime.Today) && _localStorage.GetUserCurrentType() == UserType.Player)
			{
				todaysActivities = BuildAllDayActivities(end);
				end = end.AddDays(-1);
			}

			var activities = new List<Activity>();
			activities.AddRange(FetchActivities(start, end, userId));

			if (!todaysActivities.IsNullOrEmpty())
			{
				activities.AddRange(todaysActivities.SelectMany(x => x));
			}
			return activities.OrderByDescending(activity => activity.DayId).ToArray();
		}

		public Activity GetActivity(ActivityType type, DateTime day, string userId = null)
		{
			var activities = GetActivities(day, userId);

			if (activities.IsNullOrEmpty())
				return new Activity(userId);

			return activities.FirstOrDefault(x => x.ActivityType.Equals(type));
		}

		public Activity[] GetActivities(DateTime day, string userId = null)
		{
			var start = day.Date;
			var end = day.Date.AddDays(1);

			if (userId == null)
				userId = _localStorage.GetCurrentUserId();

			if (day.Date.Equals(DateTime.Today) && _localStorage.GetUserCurrentType() == UserType.Player)
				return BuildAllDayActivities(day)
					.SelectMany(x => x)
					.ToArray();

			return FetchActivities(start, end, userId);
		}

		private Activity[] FetchActivities(DateTime start, DateTime end, string userId)
		{
			var activities = _activityRepository.GetActivities(userId, start, end);

			if (activities.IsNullOrEmpty())
				return new Activity[0];

			return activities;
		}

		private List<Activity>[] BuildAllDayActivities(DateTime day)
		{
			var currentDayActivities = new List<Activity>[6];
			var index = 0;

			foreach (ActivityType type in Enum.GetValues(typeof(ActivityType)))
			{
				var activity = _activityBuilder.BuildActivity(type, day);
				currentDayActivities[index] = activity;
				index++;
			}

			return currentDayActivities;
		}

		private Activity[] CreateEmptyActivities(DateTime day)
			{
				var activities = new Activity[6];
				var index = 0;

				foreach (ActivityType type in Enum.GetValues(typeof(ActivityType)))
				{
					var activity = new Activity(_localStorage.GetCurrentUserId());
					activity.DayId = day.Date;
					activity.ActivityType = type;

					activities[index] = activity;
					index++;
				}

				return activities;

			}

	}
}
