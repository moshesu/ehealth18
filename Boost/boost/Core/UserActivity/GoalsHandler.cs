using boost.Core.App;
using boost.Core.Entities;
using boost.Repositories;

namespace boost.Core.UserActivity
{
	public class GoalsHandler : IGoalsHandler
	{
		private readonly ILocalStorage _localStorage;
		private readonly IGoalsRepository _goalsRepository;
		private string _userId;

		public GoalsHandler(ILocalStorage local, IGoalsRepository goalsRepository)
		{
			_localStorage = local;
			_goalsRepository = goalsRepository;
		}

		public Goals GetGoals(string playerId = null)
		{
			_userId = _localStorage.GetCurrentUserId();

			if (playerId != null)
			{
				_userId = playerId;
			}

			var goals = _goalsRepository.GetGoals(_userId);
			return goals;
		}

		//Only Steps amount and reward are mandatory
		public void SetDailyGoals(
			string  playerId,
			int stepsAmount, int stepsRewards, 
			int sleepAmount = 0, int sleepReward = 0,
			int caloriesAmount = 0,  int caloriesReward = 0 )
		{
			var goals = _goalsRepository.GetGoals(playerId);
			if (goals == null)
				goals = new Goals();

			goals.StepsTaken = stepsAmount;
			goals.StepsTakenReward = stepsRewards;
			goals.SleepMinutes = sleepAmount;
			goals.SleepMinutesReward = sleepReward;
			goals.CaloriesBurned = caloriesAmount;
			goals.CaloriesBurnedReward = caloriesReward;

			_goalsRepository.SaveGoals(playerId, goals);
		}

		public void SetWeeklyGoals(
			string playerId,
			int activeMinutes, int activeMinutesRewards,
			int caloriesAmount = 0, int caloriesReward = 0)
		{
			var goals = _goalsRepository.GetGoals(playerId);
			if (goals == null)
				goals = new Goals();

			goals.WeeklyActiveMinutes = activeMinutes;
			goals.WeeklyActiveMinutesReward = activeMinutesRewards;
			goals.WeeklyCaloriesBurned = caloriesAmount;
			goals.WeeklyCaloriesBurnedReward = caloriesReward;

			_goalsRepository.SaveGoals(playerId, goals);
		}
	}
}
