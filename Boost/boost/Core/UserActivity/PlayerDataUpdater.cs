using System;
using System.Collections.Generic;
using System.Linq;
using boost.Core.Entities;
using boost.Core.Entities.Progress;
using boost.Repositories;
using boost.Util;

namespace boost.Core.UserActivity
{
	public class PlayerDataUpdater
	{
		private readonly IProgressRepository _progressRepository;
		private readonly IProgressBuilder _progressBuilder;
		private readonly IBalanceUpdater _balanceUpdater;
		private readonly ILocalStorage _localStorage;

		public PlayerDataUpdater(IProgressRepository progressRepository, IProgressBuilder progressBuilder, ILocalStorage localStorage, IBalanceUpdater balanceUpdater)
		{
			_progressRepository = progressRepository;
			_progressBuilder = progressBuilder;
			_localStorage = localStorage;
			_balanceUpdater = balanceUpdater;
		}

		public void SyncPlayerData()
		{
			var userId = _localStorage.GetCurrentUserId();

			var start = GetLastSyncDate(userId).Date;
			var end = DateTime.Today;

			var progressToSync = GetDailyProgress(start, end);

			_balanceUpdater.UpdateBalance(progressToSync);
			_progressRepository.SaveProgress(userId, progressToSync);
		}

		private DailyProgressSummary[] GetDailyProgress(DateTime start, DateTime end)
		{
			var timeSpan = end.Subtract(start);
			var progressToSync = new List<DailyProgressSummary>();

			for (var date = start; date.Date <= end.Date; date = date.AddDays(1))
			{
				var progress = GetDailyProgress(date);
				if (progress != null)
					progressToSync.Add(progress);
			}

			return progressToSync.ToArray();
		}

		private DailyProgressSummary GetDailyProgress(DateTime date)
		{
			//Notice that this also saves current daily to the database
			if (date.Equals(DateTime.Today))
				return _progressBuilder.BuildCurrentDayProgress();

			return _progressBuilder.BuildDailyProgress(date);
		}

		public DateTime GetLastSyncDate(string userId)
		{
			var end = DateTime.Today;
			var start = end - TimeSpan.FromDays(7);
			var progress = _progressRepository.GetProgress(userId, start, end);

			if (progress.IsNullOrEmpty())
				return start;

			return progress.Last().DayId;
		}
	}
}
