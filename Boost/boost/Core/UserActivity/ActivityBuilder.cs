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
	public interface IActivityBuilder
	{
		List<Activity> BuildActivity(ActivityType type, DateTime day);
		List<Activity> BuildActivities(List<HealthActivity> activities);
	}

	public class ActivityBuilder : IActivityBuilder
	{
		private readonly ILocalStorage _localStorage;

		public ActivityBuilder(ILocalStorage localStorage, IActivityRepository activityRepository)
		{
			_localStorage = localStorage;
		}

		public List<Activity> BuildActivity(ActivityType type, DateTime day)
		{
			var userId = _localStorage.GetCurrentUserId();
			var activities = HealthDataFetcher.Instance.GetActivities(type, day.Date, day.Date.AddDays(1));

			if (activities.IsNullOrEmpty())
				return new List<Activity> { new Activity(userId) };

			return activities.Select(MapActivity).ToList();
		}
		public List<Activity> BuildActivities(List<HealthActivity> healthActivities)
		{
			return healthActivities.Select(MapActivity).ToList();
		}

		private Activity MapActivity(HealthActivity activity)
		{
			return new Activity
			{
				UserId = _localStorage.GetCurrentUserId(),
				Id = activity.Id,
				ActivityType = activity.Type,
				DayId = activity.DayId,
				Duration = activity.Duration,
				TotalDistance =(activity.DistanceSummary != null) ? activity.DistanceSummary.TotalDistance : 0,
				TotalDistanceOnFoot =(activity.DistanceSummary != null) ? activity.DistanceSummary.TotalDistanceOnFoot : 0,
				TotalCalories =(activity.CaloriesBurnedSummary != null) ? activity.CaloriesBurnedSummary.TotalCalories : 0,
				AverageHeartRate =(activity.HeartRateSummary != null) ? activity.HeartRateSummary.AverageHeartRate : 0,
				LowestHeartRate = (activity.HeartRateSummary != null) ? activity.HeartRateSummary.LowestHeartRate : 0,
				PeakHeartRate = (activity.HeartRateSummary != null) ? activity.HeartRateSummary.PeakHeartRate : 0,
				EndTime = activity.EndTime,
				StartTime = activity.StartTime,
			};
		}
	}
}
