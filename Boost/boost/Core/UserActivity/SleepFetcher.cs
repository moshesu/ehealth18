using System;
using System.Collections.Generic;
using System.Linq;
using boost.Cloud.HealthCloud.DataFetcher;
using boost.Cloud.HealthCloud.HealthTypes;
using boost.Core.App;
using boost.Core.Entities;
using boost.Core.Entities.Users;
using boost.Repositories;
using boost.Util;

namespace boost.Core.UserActivity
{
	public class SleepFetcher : ISleepFetcher
	{
		private readonly ILocalStorage _localStorage;
		private readonly ISleepRepository _sleepRepository;

		public SleepFetcher(ILocalStorage localStorage, ISleepRepository sleepRepository)
		{
			_localStorage = localStorage;
			_sleepRepository = sleepRepository;
		}

		public Sleep[] GetSleepDuringSpan(DateTime start, DateTime end, string userId = null)
		{
			if (userId == null)
				userId = _localStorage.GetCurrentUserId();

			var healthSleep = new HealthSleep[0];
			var todaysSleep = new Sleep{DayId = DateTime.Today};

			if (end.Date.Equals(DateTime.Today) && _localStorage.GetUserCurrentType() == UserType.Player)
			{
				end = end.AddDays(-1);
				healthSleep = HealthDataFetcher.Instance.GetSleepRecords(start, end);
			}

			var sleep = GetSleep(start, end, userId);

			if (!healthSleep.IsNullOrEmpty())
			{
				todaysSleep = healthSleep.First().Map();
				sleep.Add(todaysSleep);
			}
			
			return sleep.ToArray();
		}

		public Sleep[] GetSleepForMonth(DateTime dayInMonth, string userId = null)
		{
			var start = dayInMonth.FirstDayOfMonth();
			var end = (dayInMonth.Month.Equals(DateTime.Today.Month)) ? DateTime.Today : dayInMonth.LastDayOfMonth();

			return GetSleepDuringSpan(start, end, userId);
		}

		private List<Sleep> GetSleep(DateTime start, DateTime end, string userId)
		{
			var sleep = _sleepRepository.GetSleepRecords(userId, start, end);

			if (sleep.IsNullOrEmpty())
				return new List<Sleep>();

			return sleep.ToList();
		}
	}
}
