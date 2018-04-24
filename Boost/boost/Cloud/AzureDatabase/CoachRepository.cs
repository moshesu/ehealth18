using boost.Core.Entities.Users;
using boost.Repositories;
using Newtonsoft.Json;

namespace boost.Cloud.AzureDatabase
{
	public class CoachRepository : AbstractAzureRepository, ICoachRepository
	{
		public Coach GetCoach(string coachId)
		{
			var userIdParameter = new Parameter(UserIdKey, coachId);

			var result = CallAzureDatabase("GetCoach", userIdParameter);
			if (result == null)
				return null;

			return JsonConvert.DeserializeObject<Coach>(result);
		}

		public void SaveCoach(Coach coach)
		{
			var userIdParameter = new Parameter(UserIdKey, coach.UserId);

			var json = JsonConvert.SerializeObject(coach, DateTimeConverter);
			var dataParameter = new Parameter("data", json);

			CallAzureDatabase("SaveCoach", userIdParameter, dataParameter);
		}

		public void RemoveCoach(string coachId)
		{
			var userIdParameter = new Parameter(UserIdKey, coachId);

			CallAzureDatabase("RemoveCoach", userIdParameter);
		}
	}
}
