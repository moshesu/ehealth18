using boost.Core.App;
using boost.Core.Entities.Progress;
using boost.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using boost.Util;

namespace boost.Core.Entities
{
	public interface IBalanceUpdater
	{
		void UpdateBalance(DailyProgressSummary[] progress);
	}


	public class BalanceUpdater : IBalanceUpdater
	{
		private readonly IBalanceHandler _balanceHandler;
		private readonly IGoalsRepository _goalsRepository;
		private readonly IProgressFetcher _progressFetcher;

		public BalanceUpdater(IBalanceHandler balanceHandler, IProgressFetcher progressFetcher, IGoalsRepository goalsRepository)
		{
			_balanceHandler = balanceHandler;
			_progressFetcher = progressFetcher;
			_goalsRepository = goalsRepository;
		}

		public void UpdateBalance(DailyProgressSummary[] progress)
		{
			if (progress.IsNullOrEmpty())
				return;

			var goals = _goalsRepository.GetGoals(progress.First().UserId);

			foreach (var summary in progress.Where(summary => summary.DayId != DateTime.Today))
			{
				var crystals = CalculateByDay(summary, goals);
				if (crystals>0)
					_balanceHandler.SendTransaction(crystals, summary.DayId.ToString());
			}

			UpadteLastWeek(progress);
		}

		private void UpadteLastWeek(DailyProgressSummary[] progress)
		{
			var crystals = 0;
			var LastSync = progress.OrderBy(summary => summary.DayId).FirstOrDefault();
			if (LastSync == null)
				return;
			var goals = _goalsRepository.GetGoals(LastSync.UserId);
			if (goals == null)
				return;

			if (IsThisWeek(LastSync.DayId))
				return;

			var lastWeek = DateTime.Today.AddDays(-7);
			var weeklyProgress = _progressFetcher.GetWeekly(lastWeek);
			if (weeklyProgress == null)
				return;


			if (weeklyProgress.ActiveMinutes >= goals.WeeklyActiveMinutes)
				crystals += goals.WeeklyActiveMinutesReward;

			if (weeklyProgress.CaloriesBurned >= goals.WeeklyCaloriesBurned)
				crystals += goals.WeeklyCaloriesBurnedReward;

			_balanceHandler.SendTransaction(crystals, String.Concat("weekly progress of week starting on: ", lastWeek.Date.StartOfWeek().ToString()));
		}

		private bool IsThisWeek(DateTime lastSyncDay)
		{
			var today = DateTime.Today;
			if (lastSyncDay.DayOfWeek > today.DayOfWeek)
				return false;

			if (today.Subtract(lastSyncDay).Days > 7)
				return false;

			return true;
		}

		private int CalculateByDay(DailyProgressSummary summary, Goals goals)
		{
			if (summary == null)
				return 0;

			var crystals = 0;

			var fields = summary.GetType()
				.GetProperties()
				.Where(field => !field.Name.Contains("Id"))
				.Select(field => new KeyValuePair<string,int>(field.Name, (int)field.GetValue(summary)))
				.ToList();

			fields.AddRange(
				goals.GetType().GetProperties()
				.Where(field => field.Name.Contains("Reward") && !field.Name.Contains("Weekly"))
				.Select(field => new KeyValuePair<string, int>(field.Name, (int)field.GetValue(goals))));

			var dict = InitEmptyDict(fields);

			MapToDictionary(fields, dict);

			foreach (var key in dict)
			{
				var act = key.Value.Item1;
				var goal = key.Value.Item2;
				var reward = key.Value.Item3;
				
				crystals += CalculateCrystals(act, goal, reward);
			}

			return crystals;
		}

		private static void MapToDictionary(List<KeyValuePair<string,int>> fields, Dictionary<string, Tuple<int, int, int>> dict)
		{
			foreach (var f in fields)
			{
				var name = f.Key;
				var value = f.Value;

				if (name.Contains("Goal"))
				{
					name = name.Substring(0, name.Length - "Goal".Length);
					dict[name] = new Tuple<int, int, int>(dict[name].Item1, value, dict[name].Item3);
				}

				else if (name.Contains("Reward"))
				{
					name = name.Substring(0, name.Length - "Reward".Length);
					dict[name] = new Tuple<int, int, int>(dict[name].Item1, dict[name].Item2, value);
				}

				else
				{
					dict[name] = new Tuple<int, int, int>(value, dict[name].Item2, dict[name].Item3);
				}
			}
		}

		private static Dictionary<string, Tuple<int, int, int>> InitEmptyDict(List<KeyValuePair<string, int>> fields)
		{
			var dict = new Dictionary<string, Tuple<int, int, int>>();
			foreach (var f in fields)
			{
				if (!(f.Key.Contains("Goal") || f.Key.Contains("Reward")))
					dict.Add(f.Key, new Tuple<int, int, int>(0, 0, 0));
			}

			return dict;
		}

		private int CalculateCrystals(int act, int goal, int reward)
		{
			if (act <= 0 || goal <= 0 || reward <= 0)
				return 0;

			if (act > goal)
				return reward;

			return 0;
		}
	}
}
