using System;
using boost.Core.Entities.Progress;

namespace boost.Core.App
{
	public interface IProgressFetcher
	{
		DailyProgressSummary GetDaily(DateTime day, string userId = null);

		WeeklyProgressSummary GetWeekly(DateTime sunday, string userId = null);

	}
}
