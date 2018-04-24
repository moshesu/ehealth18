using boost.Core.App;
using System;
using System.Linq;
using boost.Core.Entities.Progress;
using boost.Core.Entities.Users;
using boost.Repositories;
using boost.Util;

namespace boost.Core.UserActivity
{
	class ProgressFetcher : IProgressFetcher
	{
		private readonly ILocalStorage _localStorage;
		private readonly IProgressRepository _progressRepository;
		private readonly IProgressBuilder _progressBuilder;
		

		public ProgressFetcher(ILocalStorage local, 
			IProgressRepository progressRepository, 
			IProgressBuilder progressBuilder)
		{
			_localStorage = local;
			_progressRepository = progressRepository;
			_progressBuilder = progressBuilder;
		}

		public DailyProgressSummary GetDaily(DateTime day, string userId = null)
		{

			if (day.Date.Equals(DateTime.Today) && _localStorage.GetUserCurrentType()==UserType.Player)
			{
				return _progressBuilder.BuildCurrentDayProgress();
			}

			if (userId == null)
				userId = _localStorage.GetCurrentUserId();

			var progressSummary = GetDailyProgress(userId, day.Date);

			return progressSummary;
		}

		public WeeklyProgressSummary GetWeekly(DateTime day, string userId = null)
		{
			if (userId == null)
				userId = _localStorage.GetCurrentUserId();

			var sunday = day.StartOfWeek();

			if (sunday.Equals(DateTime.Today.StartOfWeek()))
			{
				return _progressBuilder.BuildCurrentWeekProgress(userId);
			}

			var weeklyProgress =  _progressBuilder.BuildWeeklyProgress(userId, sunday, sunday.AddDays(7));

			return weeklyProgress;
		}

		private DailyProgressSummary GetDailyProgress(string userId, DateTime dayDate)
		{
			var progress = _progressRepository.GetProgress(userId, dayDate, dayDate.AddDays(1));

			if (progress.IsNullOrEmpty())
				return new DailyProgressSummary();

			return progress.First();
		}
	}
}
