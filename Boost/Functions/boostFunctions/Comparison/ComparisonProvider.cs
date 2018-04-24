using System;
using System.Collections.Generic;
using System.Linq;
using boostFunctions.Database;
using boostFunctions.Mocking;
using boostFunctions.UserComparison;
using boostFunctions.Util;

namespace boostFunctions.Comparison
{
	public interface IBoostRepository
	{
		IList<Player.PlayerModel> GetPlayers();
		IList<DailyProgress.DailyProgressModel> GetDailyProgresses(DateTime from, DateTime to);
	}

	public class BoostRepository : IBoostRepository
	{
		private readonly bool _mockFlag = false; //TODO turn off mocking @Shanee
		private readonly BoostRepositoryMock _mock = new BoostRepositoryMock();

		public IList<DailyProgress.DailyProgressModel> GetDailyProgresses(DateTime from, DateTime to)
		{
			if (_mockFlag)
				return _mock.GetDailyProgresses(from, to);

			return new DailyProgress().GetAll(from, to);
		}

		public IList<Player.PlayerModel> GetPlayers()
		{
			if (_mockFlag)
				return _mock.GetPlayers();

			return new Player().GetAll();
		}
	}

	public class ComparisonProvider
	{
		private IBoostRepository _playersProvider;
		private int _numOfBuckets = 10;

		public ComparisonProvider(IBoostRepository playersProvider)
		{
			_playersProvider = playersProvider;
		}

		public Comparison GetUserComparison(string userId)
		{
			var allPlayers = _playersProvider.GetPlayers();
			var allDailyProgress = _playersProvider.GetDailyProgresses(DateTime.Today.AddDays(-30), DateTime.Now).ToArray();

			var currentPlayer = allPlayers.FirstOrDefault(p => p.UserId == userId);
			var currentPlayerProgress = allDailyProgress.Where(p => p.UserId == userId).ToList();

			if (currentPlayer == null || !currentPlayerProgress.Any())
			{
				return new Comparison();
			}

			var progressForComparison = GetProgressToCompare(currentPlayer, allPlayers, allDailyProgress);

			return new Comparison
			{
				LastCalculated = DateTime.Now,
				SleepComparison = BuildComparisonData(progressForComparison, currentPlayerProgress, x => x.SleepMinutes),
				StepsComparison = BuildComparisonData(progressForComparison, currentPlayerProgress, x => x.StepsTaken),
				ActiveHoursComparison = BuildComparisonData(progressForComparison, currentPlayerProgress, x => x.ActiveHours)
			};
		}

		private ComparisonData BuildComparisonData(IList<DailyProgress.DailyProgressModel> progress,
			IList<DailyProgress.DailyProgressModel> userProgress,
			Func<DailyProgress.DailyProgressModel, int> selector)
		{
			try
			{
				var userAverage = userProgress.Average(selector);

				var min = progress.Min(selector);
				var max = progress.Max(selector);
				var bucketSize = (max - min) / _numOfBuckets;
				var buckets = new int[_numOfBuckets];

				foreach (var p in progress)
				{
					var value = selector(p);
					int i;

				if (value > min + bucketSize * 9)
					i = _numOfBuckets - 1;
				else
					i = (value - min) / bucketSize;

					buckets[i]++;
				}

				var playerBucket = (int) Math.Floor((userAverage - min) / bucketSize);

				return new ComparisonData
				{
					Min = min,
					Max = max,
					UserPercentile = playerBucket,
					PercentileAmounts = buckets
				};
			}
			catch
			{
				return new ComparisonData
				{
					Min = 0,
					Max = 0,
					UserPercentile = 0,
					PercentileAmounts = new int[0]
				};
			}
		}

		private static List<DailyProgress.DailyProgressModel> GetProgressToCompare(Player.PlayerModel current,
			IList<Player.PlayerModel> allPlayers,
			DailyProgress.DailyProgressModel[] allDailyProgress)
		{
			var inRangeUserIds = GetUserIdsByAge(allPlayers, current);
			var progressbyUsers = allDailyProgress.Where(progress => inRangeUserIds.Contains(progress.UserId));

			return progressbyUsers.ToList();
		}

		private static List<string> GetUserIdsByAge(IList<Player.PlayerModel> allPlayers, Player.PlayerModel currentPlayer)
		{
			var playersInAgeRange = allPlayers
				.Where(InAgeRange(currentPlayer))
				.Select(player => player.UserId);

			return playersInAgeRange.ToList();
		}

		private static Func<Player.PlayerModel, bool> InAgeRange(Player.PlayerModel currentPlayer)
		{
			return player => player.Birthdate.WithinYearRange(currentPlayer.Birthdate);
		}
	}
}