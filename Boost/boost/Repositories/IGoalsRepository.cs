using boost.Core.Entities;
using System;

namespace boost.Repositories
{
	public interface IGoalsRepository
	{
		Goals GetGoals(string userId);
		void SaveGoals(string userId, Goals goals);
		void RemoveGoals(string initialId);
	}
}
