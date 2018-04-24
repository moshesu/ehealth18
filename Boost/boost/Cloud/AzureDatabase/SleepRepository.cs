using System;
using boost.Core.Entities;
using boost.Repositories;
using Newtonsoft.Json;

namespace boost.Cloud.AzureDatabase
{
	public class SleepRepository : AbstractAzureRepository, ISleepRepository
	{
		public Sleep[] GetSleepRecords(string userId, DateTime startTime, DateTime endTime)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);
			var startTimeParameter = new Parameter("startTime", startTime.Ticks.ToString());
			var endTimeParameter = new Parameter("endTime", endTime.Ticks.ToString());

			var result = CallAzureDatabase("GetSleepRecords", userIdParameter, startTimeParameter, endTimeParameter);
			if (result == null)
				return new Sleep[0];

			return JsonConvert.DeserializeObject<Sleep[]>(result);
		}

		public void SaveSleepRecords(string userId, Sleep[] activities)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);

			var json = JsonConvert.SerializeObject(activities, DateTimeConverter);
			var dataParameter = new Parameter("data", json);

			CallAzureDatabase("SaveSleepRecords", userIdParameter, dataParameter);
		}
	}
}
