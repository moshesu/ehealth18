
using boost.Core.Entities.Users;

namespace boost.Repositories
{
	public interface ICoachRepository
	{
		Coach GetCoach(string coachId);
		void SaveCoach(Coach coach);
		void RemoveCoach(string coachId);
	}
}
