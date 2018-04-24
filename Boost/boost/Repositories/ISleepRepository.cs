using System;
using boost.Core.Entities;

namespace boost.Repositories
{
	public interface ISleepRepository
	{
		Sleep[] GetSleepRecords(string userId, DateTime startTime, DateTime endTime);

		void SaveSleepRecords(string userId, Sleep[] activities);
	}
}
