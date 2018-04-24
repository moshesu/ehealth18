using System;
using System.Linq;
using boostFunctions.Database;
using boostFunctions.Mocking;


namespace boostFunctions.Util
{
    class ComparisonUserCreator
    {
	    private static BoostRepositoryMock _mockRepo = new BoostRepositoryMock();
		private static Player _player = new Player();
		private static DailyProgress _progress = new DailyProgress();

		public static void CreateUsers()
		{
			var players = _mockRepo.GetPlayers();
			foreach (var p in players)
			{
				_player.Save(p);
			}

		    var progresses = _mockRepo.GetDailyProgresses(DateTime.Today.AddDays(-5), DateTime.Now);

			progresses.GetBulk(50).Apply(prog => _progress.Save(prog.ToArray()));
		}
    }
}
