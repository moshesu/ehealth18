using System;
using System.Collections.Generic;
using System.Linq;
using boostFunctions.Comparison;
using boostFunctions.Database;
using boostFunctions.Util;

namespace boostFunctions.Mocking
{
	public class BoostRepositoryMock : IBoostRepository
	{
		private readonly string[] _genders = new[] {"male", "female"};

		private readonly string[] _firstNames = new[]
			{"Maul", "Nihilus", "Luke", "Rey", "Traya", "Ashoka", "Han", "Zeb", "Sabine"};

		private readonly string[] _lastNames = new[] {"SkyWalker", "Organa", "Fett", "Solo", "Antilles", "Imwe"};

		private readonly DateTime[] _birthdays = new DateTime[]
			{new DateTime(1992, 1, 1)};

		private string _targetUserId = "Leiah";
		private static readonly Random _rand = new Random();

		public IList<DailyProgress.DailyProgressModel> GetDailyProgresses(DateTime from, DateTime to)
		{
			return GetPlayersProgress("Leiah");
		}

		public IList<Player.PlayerModel> GetPlayers()
		{
			return GetPlayers("Leiah");
		}

		public IList<DailyProgress.DailyProgressModel> GetPlayersProgress(string id)
		{
			_targetUserId = id;
			var progress = Enumerable.Range(1, 400).Select(GenerateProgess).ToList();

			progress[0].UserId = _targetUserId;
			progress[10].UserId = _targetUserId;

			return progress;
		}

		private DailyProgress.DailyProgressModel GenerateProgess(int userId)
		{
			var numericalId = userId > 100 ? userId % 100 : userId;
			var day = userId / 100;
			var id = String.Concat("mock", numericalId.ToString());

			return new DailyProgress.DailyProgressModel()
			{
				UserId = id,
				DayId = DateTime.Today.AddDays(-1 - day),
				StepsTaken = _rand.NormalRandomInt(3800, 660, 900, 10000),
				ActiveHours = _rand.NormalRandomInt(150, 25, 0, 400),
				SleepMinutes = _rand.NormalRandomInt(450, 50, 200, 600)
			};
		}

		public IList<Player.PlayerModel> GetPlayers(string id)
		{
			_targetUserId = id;

			var players = Enumerable.Range(1, 100).Select(GeneratePlayer).ToList();

			players[0].UserId = _targetUserId;
			return players;
		}

		private Player.PlayerModel GeneratePlayer(int playerId)
		{
			var id = playerId.ToString();

			return new Player.PlayerModel
			{
				UserId = String.Concat("mock", id),
				FirstName = String.Concat(_firstNames.RandomItem(), id),
				LastName = _lastNames.RandomItem(),
				Gender = _genders.RandomItem(),

				Height = 1300,
				Weight = 50000,

				Birthdate = new DateTime(1992, 8, 13),
				CreatedTime = DateTime.Now,
				LastUpdateTime = DateTime.Now,
				CoachId = "123456789",
				PostalCode = "23123",
				PreferredLocale = "en-US"
			};
		}
	}
}