using System;

namespace boost.Cloud.HealthCloud.HealthTypes
{
	public class HealthDailySummary
	{
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public int StepsTaken { get; set; }
		public int FloorsClimbed { get; set; }
		public int ActiveHours { get; set; }
		public int ActiveSeconds { get; set; }

		public CaloriesBurnedSummary CaloriesBurnedSummary { get; set; }
		public DistanceSummary DistanceSummary { get; set; }
		public HeartRateSummary HeartRateSummary { get; set; }
	}
}
