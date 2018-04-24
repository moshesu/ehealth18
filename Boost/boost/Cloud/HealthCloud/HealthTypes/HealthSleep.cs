using System;
using boost.Core.Entities;

namespace boost.Cloud.HealthCloud.HealthTypes
{
	public class HealthSleep
	{
		public string Id { get; set; }
		public DateTime DayId { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public DateTime FallAsleepTime { get; set; }
		public DateTime WakeupTime { get; set; }

		public string Duration { get; set; }
		public string SleepDuration { get; set; }
		public string FallAsleepDuration { get; set; }
		public string AwakeDuration { get; set; }
		public string TotalRestfulSleepDuration { get; set; }
		public string TotalRestlessSleepDuration { get; set; }

		public int NumberOfWakeups { get; set; }

		public int RestingHeartRate { get; set; }
		public CaloriesBurnedSummary CaloriesBurnedSummary { get; set; }
		public HeartRateSummary HeartRateSummary { get; set; }

		public Sleep Map()
		{
			return new Sleep()
			{
				Id = Id,
				DayId = DayId,
				StartTime = StartTime,
				EndTime = EndTime,
				FallAsleepTime = FallAsleepTime,
				WakeupTime = WakeupTime,

				Duration = Duration,
				SleepDuration = SleepDuration,
				FallAsleepDuration = FallAsleepDuration,
				AwakeDuration = AwakeDuration,
				TotalRestfulSleepDuration = TotalRestfulSleepDuration,
				TotalRestlessSleepDuration = TotalRestlessSleepDuration,

				NumberOfWakeups = NumberOfWakeups,
				RestingHeartRate = RestingHeartRate,
				TotalCalories = CaloriesBurnedSummary?.TotalCalories ?? 0,
				AverageHeartRate = HeartRateSummary?.AverageHeartRate ?? 0,
				LowestHeartRate = HeartRateSummary?.LowestHeartRate ?? 0,
				PeakHeartRate = HeartRateSummary?.PeakHeartRate ?? 0,
			};
		}
	}
}
