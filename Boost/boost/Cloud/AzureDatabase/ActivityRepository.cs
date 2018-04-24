using System;
using boost.Core.Entities;
using boost.Repositories;
using Newtonsoft.Json;

namespace boost.Cloud.AzureDatabase
{
	public class ActivityRepository : AbstractAzureRepository, IActivityRepository
	{
		public Activity[] GetActivities(string userId, DateTime startTime, DateTime endTime)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);
			var startTimeParameter = new Parameter("startTime", startTime.Ticks.ToString());
			var endTimeParameter = new Parameter("endTime", endTime.Ticks.ToString());

			var result = CallAzureDatabase("GetActivities", userIdParameter, startTimeParameter, endTimeParameter);
			if(result == null)
				return new Activity[0];

			return JsonConvert.DeserializeObject<Activity[]>(result);
		}

		public void SaveActivities(string userId, Activity[] activities)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);

			var json = JsonConvert.SerializeObject(activities, DateTimeConverter);
			var dataParameter = new Parameter("data", json);

			CallAzureDatabase("SaveActivities", userIdParameter, dataParameter);
		}
	}
}