using System;
using System.Linq;
using boost.Core.Entities;
using boost.Core.Entities.Progress;
using boost.Repositories;
using boost.Util;
using Newtonsoft.Json;

namespace boost.Cloud.AzureDatabase
{
	public class ProgressRepository : AbstractAzureRepository, IProgressRepository
	{
		public DailyProgressSummary[] GetProgress(string userId, DateTime startTime, DateTime endTime)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);
			var startTimeParameter = new Parameter("startTime", startTime.Ticks.ToString());
			var endTimeParameter = new Parameter("endTime", endTime.Ticks.ToString());

			var result = CallAzureDatabase("GetProgress", userIdParameter, startTimeParameter, endTimeParameter);
			if (result == null)
				return new DailyProgressSummary[0];

			return JsonConvert.DeserializeObject<DailyProgressSummary[]>(result);
		}

		public void SaveProgress(string userId, DailyProgressSummary[] progressRecords)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);

			var json = JsonConvert.SerializeObject(progressRecords, DateTimeConverter);
			var dataParameter = new Parameter("data", json);

			CallAzureDatabase("SaveProgress", userIdParameter, dataParameter);
		}
	}
}
