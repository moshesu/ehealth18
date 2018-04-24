using System;
using boost.Core.Entities;

namespace boost.Repositories
{
	public interface IActivityRepository
	{
		Activity[] GetActivities(string userId, DateTime startTime, DateTime endTime);

		void SaveActivities(string userId, Activity[] activities);
	}
}
