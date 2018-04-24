using System;

namespace boost.Core.Entities.Progress
{
	public class DailyProgressSummary
	{
		public string UserId { get; set; }
		public DateTime DayId { get; set; }

		public int StepsTaken { get; set; }
		public int StepsTakenGoal { get; set; }

		public int CaloriesBurned { get; set; }
		public int CaloriesBurnedGoal { get; set; }

		public int SleepMinutes { get; set; }
		public int SleepMinutesGoal { get; set; }

		public int FloorsClimbed { get; set; }
		public int FloorsClimbedGoal { get; set; }

		public int ActiveMinutes { get; set; }
		public int ActiveMinutesGoal { get; set; }

		public int TotalDistance { get; set; }
		public int TotalDistanceGoal { get; set; }

		public int TotalDistanceOnFoot { get; set; }
		public int TotalDistanceOnFootGoal { get; set; }

		public DailyProgressSummary()
		{
			DayId = new DateTime(1970,1,1);
		}

		public DailyProgressSummary(string userId) : this()
		{
			UserId = userId;
		}
	}
}
