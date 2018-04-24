using System;
using boost.Core.Entities;
using boost.Repositories;
using Newtonsoft.Json;

namespace boost.Cloud.AzureDatabase
{
	public class GoalsRepository : AbstractAzureRepository, IGoalsRepository
	{
		public Goals GetGoals(string userId)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);

			var result = CallAzureDatabase("GetGoals", userIdParameter);
			if (result == null)
				return null;

			return JsonConvert.DeserializeObject<Goals>(result);
		}

		public void SaveGoals(string userId, Goals goals)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);

			var json = JsonConvert.SerializeObject(goals, DateTimeConverter);
			var dataParameter = new Parameter("data", json);

			CallAzureDatabase("SaveGoals", userIdParameter, dataParameter);
		}

		public void RemoveGoals(string playerId)
		{
			var userIdParameter = new Parameter(UserIdKey, playerId);

			CallAzureDatabase("RemoveGoals", userIdParameter);
		}
	}
}
