
using boost.Core.Entities.Users;

namespace boost.Repositories
{
	public interface IPlayerRepository
	{
		Player GetPlayer(string userId);
		Player[] GetPlayersByCoach(string coachId);
		void SavePlayer(Player player);
		void RemovePlayer(string initialId);
	}
}
