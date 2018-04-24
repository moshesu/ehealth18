using boost.Core.Entities;

namespace boost.Core.App
{
	public interface IGoalsHandler
	{
		Goals GetGoals(string userId = null);
		void SetDailyGoals(
			string userId,
			int stepsAmount, int stepsRewards,
			int sleepAmount = 0, int sleepReward = 0,
			int caloriesAmount = 0, int caloriesReward = 0);

		void SetWeeklyGoals(
			string userId,
			int activeMinutes, int activeMinutesRewards,
			int caloriesAmount = 0, int caloriesReward = 0);
	}
}
