using boost.Core.Entities.Users;
using boost.Repositories;
using Newtonsoft.Json;

namespace boost.Cloud.AzureDatabase
{
	public class PlayerRepository : AbstractAzureRepository, IPlayerRepository
	{
		public Player GetPlayer(string userId)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);

			var result = CallAzureDatabase("GetPlayer", userIdParameter);
			if (result == null)
				return null;

			return JsonConvert.DeserializeObject<Player>(result);
		}

		public Player[] GetPlayersByCoach(string coachId)
		{
			var coachIdParameter = new Parameter("coachId", coachId);

			var result = CallAzureDatabase("GetPlayersByCoach", coachIdParameter);
			if (result == null)
				return new Player[0];

			return JsonConvert.DeserializeObject<Player[]>(result);
		}

		public void SavePlayer(Player player)
		{
			var userIdParameter = new Parameter(UserIdKey, player.UserId);

			var json = JsonConvert.SerializeObject(player, DateTimeConverter);
			var dataParameter = new Parameter("data", json);

			CallAzureDatabase("SavePlayer", userIdParameter, dataParameter);
		}

		public void RemovePlayer(string playerId)
		{
			var userIdParameter = new Parameter(UserIdKey, playerId);

			CallAzureDatabase("RemovePlayer", userIdParameter);
		}
	}
}
