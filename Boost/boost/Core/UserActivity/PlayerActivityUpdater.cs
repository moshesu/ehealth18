using System;
using System.Collections.Generic;
using System.Linq;
using boost.Cloud.HealthCloud.DataFetcher;
using boost.Cloud.HealthCloud.HealthTypes;
using boost.Core.Entities;
using boost.Repositories;
using boost.Util;

namespace boost.Core.UserActivity
{
	public class PlayerActivityUpdater
	{
		private readonly IActivityRepository _activityRepository;
		private readonly IActivityBuilder _activityBuilder;
		private readonly ILocalStorage _localStorage;
		
		public PlayerActivityUpdater(IActivityRepository activityRepository, IActivityBuilder activityBuilder, ILocalStorage localStorage)
		{
			_activityRepository = activityRepository;
			_activityBuilder = activityBuilder;
			_localStorage = localStorage;
		}

		public void SyncPlayerActivitiesToDB()
		{
			var userId = _localStorage.GetCurrentUserId();

			var start = GetLastSyncDate(userId).Date;
			var end = DateTime.Now; //changed from Today to Now

			var activitiesToSync = GetActivitiesForSpan(start, end).ToArray();

			_activityRepository.SaveActivities(userId, activitiesToSync);
		}

		private List<Activity> GetActivitiesForSpan(DateTime start, DateTime end)
		{
			var healthActivities = GetAllActivities(start, end);
			var activitiesToSync = _activityBuilder.BuildActivities(healthActivities);
				
			return activitiesToSync;
		}

		private List<HealthActivity> GetAllActivities(DateTime start, DateTime end)
		{
			var healthActivities = new List<HealthActivity>();
			foreach (ActivityType type in Enum.GetValues(typeof(ActivityType)))
			{
				healthActivities.AddRange(HealthDataFetcher.Instance.GetActivities(type, start, end));
			}

			return healthActivities.OrderBy(activity => activity.DayId).ToList();
		}
		public DateTime GetLastSyncDate(string userId)
		{
			var end = DateTime.Today;
			var start = end - TimeSpan.FromDays(7);
			var activities = _activityRepository.GetActivities(userId, start, end);

			if (activities.IsNullOrEmpty())
				return start;

			return activities.Last().DayId;
		}
	}
}
