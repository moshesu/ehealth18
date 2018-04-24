using System;
using boost.Core.Entities.Progress;

namespace boost.Repositories
{
	public interface IProgressRepository
	{
		DailyProgressSummary[] GetProgress(string userId, DateTime startTime, DateTime endTime);

		void SaveProgress(string userId, DailyProgressSummary[] progressRecords);
	}
}
