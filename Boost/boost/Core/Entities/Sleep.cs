using System;

namespace boost.Core.Entities
{
	public class Sleep
	{
		public string Id { get; set; }
		public string UserId { get; set; }
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
		public int TotalCalories { get; set; }
		public int AverageHeartRate { get; set; }
		public int LowestHeartRate { get; set; }
		public int PeakHeartRate { get; set; }

		public Sleep()
		{
			DayId = new DateTime(1970, 1, 1);
		}

		public Sleep(string userId) : this()
		{
			UserId = userId;
		}
	}
}
