using boost.Core.Entities;
using boost.Core.Entities.Users;

namespace boost.Repositories
{
	public interface IUserTypeRepository
	{
		UserTypeRecord GetUserType(string userId);
		void SaveUserType(string userId, UserType type, string coachId = null);
		void SaveUserType(UserTypeRecord record);
		string AddUnassignedPlayer(string coachId);
		void RemovePlayerTypeRecord(string initialId);
	}
}
