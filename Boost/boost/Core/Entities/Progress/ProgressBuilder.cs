using System;
using System.Linq;
using boost.Cloud.HealthCloud.DataFetcher;
using boost.Cloud.HealthCloud.HealthTypes;
using boost.Repositories;
using boost.Util;

namespace boost.Core.Entities.Progress
{
	public interface IProgressBuilder
	{
		DailyProgressSummary BuildCurrentDayProgress();
		DailyProgressSummary BuildDailyProgress(DateTime date);
		WeeklyProgressSummary BuildCurrentWeekProgress(string userId);
		WeeklyProgressSummary BuildWeeklyProgress(string usreId, DateTime start, DateTime end);
	}

	public class ProgressBuilder : IProgressBuilder
	{
		private readonly ILocalStorage _localStorage;
		private readonly IGoalsRepository _goalsRepository;
		private readonly IProgressRepository _progressRepository;
		private readonly ISleepRepository _sleepRepository;

		public ProgressBuilder(ILocalStorage local, 
			IGoalsRepository goalsRepository, 
			IProgressRepository progressRepository, 
			ISleepRepository sleepRepository)
		{
			_localStorage = local;
			_goalsRepository = goalsRepository;
			_progressRepository = progressRepository;
			_sleepRepository = sleepRepository;
		}

		public DailyProgressSummary BuildCurrentDayProgress()
		{
			var progress =  BuildDailyProgress(DateTime.Today);

			_progressRepository.SaveProgress(_localStorage.GetCurrentUserId(), new DailyProgressSummary[]{progress});

			return progress;
		}

		public DailyProgressSummary BuildDailyProgress(DateTime date)
		{
			var start = date.Date;
			var end = (date.Date==DateTime.Today)? DateTime.Now : date.AddDays(1);

			var dailySummary = GetDailySummary(start, end);
			var sleepSummary = GetSleepSummary(start, end);

			var goals = _goalsRepository.GetGoals(_localStorage.GetCurrentUserId());

			if (goals == null)
				goals = new Goals();

			var progressSummary = new DailyProgressSummary
			{
				UserId = _localStorage.GetCurrentUserId(),
				DayId = dailySummary.StartTime.Date,
				StepsTaken = dailySummary.StepsTaken,
				StepsTakenGoal = goals.StepsTaken,
				CaloriesBurned = dailySummary.CaloriesBurnedSummary.TotalCalories,
				CaloriesBurnedGoal = goals.CaloriesBurned,
				SleepMinutes = (int) TimeUtil.GetDuration(sleepSummary.SleepDuration).TotalMinutes,
				SleepMinutesGoal = goals.SleepMinutes,
				ActiveMinutes = dailySummary.ActiveSeconds / 60,
				FloorsClimbed = dailySummary.FloorsClimbed,
				TotalDistance = dailySummary.DistanceSummary.TotalDistance,
				TotalDistanceOnFoot = dailySummary.DistanceSummary.TotalDistanceOnFoot
			};

			return progressSummary;
		}

		public WeeklyProgressSummary BuildCurrentWeekProgress(string userId)
		{
			var sunday = DateTime.Today.StartOfWeek();
			var end = DateTime.Now;

			return BuildWeeklyProgress(userId, sunday, end);
		}

		public WeeklyProgressSummary BuildWeeklyProgress(string userId, DateTime start, DateTime end)
		{
			if (userId.Equals(_localStorage.GetCurrentUserId()))
				return BuildForPlayer(start, end);

			var playersProgress = _progressRepository.GetProgress(userId, start, end);
			var weeklyProgressSummary = CalculateWeeklyByDays(playersProgress);

			return weeklyProgressSummary;
		}

		public WeeklyProgressSummary CalculateWeeklyByDays(DailyProgressSummary[] progress)
		{
			if (progress.IsNullOrEmpty())
				return new WeeklyProgressSummary();

			var weeklyProgressSummary = new WeeklyProgressSummary()
			{
				ActiveMinutes = progress.Sum(p => p.ActiveMinutes),
				ActiveMinutesGoal = progress.Last().ActiveMinutesGoal,
				CaloriesBurned = progress.Sum(p => p.CaloriesBurned),
				CaloriesBurnedGoal = progress.Sum(p => p.CaloriesBurnedGoal)
			};

			return weeklyProgressSummary;
		}

		public WeeklyProgressSummary BuildForPlayer(DateTime start, DateTime end)
		{
			var dailySummaries = GetDailySummaries(start, end);

			if (dailySummaries.IsNullOrEmpty())
				return new WeeklyProgressSummary();

			var activeMinutes = CalculateTotalActiveMinutes(dailySummaries);
			var calories = CalculateTotalCalories(dailySummaries);

			var weeklygoals = _goalsRepository.GetGoals(_localStorage.GetCurrentUserId()); 
			var weeklyProgressSummary = new WeeklyProgressSummary()
			{
				ActiveMinutes = activeMinutes,
				ActiveMinutesGoal = weeklygoals.WeeklyActiveMinutes,
				CaloriesBurned = calories,
				CaloriesBurnedGoal = weeklygoals.WeeklyCaloriesBurned,
			};
			return weeklyProgressSummary;
		}

		private static int CalculateTotalActiveMinutes(HealthDailySummary[] dailySummaries)
		{
			var totalSeconds =  dailySummaries
				.Select(summary => summary.ActiveSeconds)
				.Sum();
			return totalSeconds / 60;
		}

		private static int CalculateTotalCalories(HealthDailySummary[] dailySummaries)
		{
			return dailySummaries
				.Select(summary => summary.CaloriesBurnedSummary)
				.Select(caloriesSummary => caloriesSummary.TotalCalories)
				.Sum();
		}

		private HealthSleep GetSleepSummary(DateTime start, DateTime end)
		{
			var summaries = GetSleepSummaries(start, end);

			if (summaries.IsNullOrEmpty())
				return new HealthSleep
				{
					SleepDuration = "0"
				};
			
			var summary =  summaries.First();
			if (summary == null)
				return new HealthSleep();

			return summary;
		}


		private HealthSleep[] GetSleepSummaries(DateTime start, DateTime end)
		{
			var healthSleep =  HealthDataFetcher.Instance.GetSleepRecords(start, end);
			var sleep = healthSleep.Select(record => record.Map()).ToArray();

			if (!sleep.IsNullOrEmpty())
			{
				var userId = _localStorage.GetCurrentUserId();
				foreach(var sum in sleep)
				{
					sum.UserId = userId;
					if (sum.FallAsleepTime <	new DateTime(1970, 1, 1))
						sum.FallAsleepTime =	new DateTime(1970, 1, 1);
					if (sum.WakeupTime <  new DateTime(1970, 1, 1))
						sum.WakeupTime =  new DateTime(1970, 1, 1);
				}
				_sleepRepository.SaveSleepRecords(userId, sleep);
			}

			return healthSleep;
		}

		private HealthDailySummary GetDailySummary(DateTime start, DateTime end)
		{
			var summaries = HealthDataFetcher.Instance.GetSummaries(start, end);

			if (summaries.IsNullOrEmpty() || summaries.First()==null)
				return new HealthDailySummary
				{
					StartTime = new DateTime(1970,1,1),
					StepsTaken = 0,
					CaloriesBurnedSummary = new CaloriesBurnedSummary { TotalCalories = 0 },
					ActiveSeconds = 0,
					FloorsClimbed = 0,
					DistanceSummary = new DistanceSummary { TotalDistance = 0, TotalDistanceOnFoot = 0 }
				};

			return summaries.First();
		}

		private HealthDailySummary[] GetDailySummaries(DateTime start, DateTime end)
		{
			return HealthDataFetcher.Instance.GetSummaries(start, end);
		}
	}
}
