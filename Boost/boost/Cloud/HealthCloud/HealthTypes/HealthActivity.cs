using System;
using boost.Core.Entities;
using boost.Util;

namespace boost.Cloud.HealthCloud.HealthTypes
{
	public class HealthActivity
	{
		public ActivityType Type { get; set; }
		public string Id { get; set; }

		public DateTime DayId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string Duration { get; set; }

		public DistanceSummary DistanceSummary { get; set; }
		public CaloriesBurnedSummary CaloriesBurnedSummary { get; set; }
		public HeartRateSummary HeartRateSummary { get; set; }

		public Activity Map()
		{
			return new Activity
			{
				ActivityType = Type,
				Id = Id,
				DayId = DayId,
				StartTime = StartTime,
				EndTime = EndTime,
				Duration = Duration,
				TotalDistance = DistanceSummary?.TotalDistance ?? 0,
				TotalDistanceOnFoot = DistanceSummary?.TotalDistanceOnFoot ?? 0,
				TotalCalories = CaloriesBurnedSummary?.TotalCalories??0,
				AverageHeartRate = HeartRateSummary?.AverageHeartRate ??0,
				LowestHeartRate = HeartRateSummary?.LowestHeartRate ??0,
				PeakHeartRate= HeartRateSummary?.PeakHeartRate ??0,
			};
		}
	}
}
