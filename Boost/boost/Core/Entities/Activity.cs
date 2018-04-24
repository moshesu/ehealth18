using System;

namespace boost.Core.Entities
{
	public class Activity
	{
		public string UserId { get; set; }

		public ActivityType ActivityType { get; set; }
		public string Id { get; set; }

		public DateTime DayId { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string Duration { get; set; }

		public int TotalDistance { get; set; }
		public int TotalDistanceOnFoot { get; set; }

		public int TotalCalories { get; set; }
		public int AverageHeartRate { get; set; }
		public int LowestHeartRate { get; set; }
		public int PeakHeartRate { get; set; }

		public Activity()
		{
			DayId = new DateTime(1970, 1, 1);
			StartTime = new DateTime(1970, 1, 1);
			EndTime = new DateTime(1970, 1, 1);
		}

		public Activity(string userId) : this()
		{
			UserId = userId;
		}

		public Activity(string userId, ActivityType type) : this(userId)
		{
			ActivityType = type;
		}
	}
}
