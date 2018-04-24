using System;
using boost.Cloud.HealthCloud.HealthTypes;
using boost.Core.Entities.Exceptions;
using boost.Repositories;

namespace boost.Core.Entities.Users
{
	public interface IUserCreator
	{
		Coach CreateCoach(HealthProfileInformation healthProfileInfo);
		Player CreatePlayer(string initialId, HealthProfileInformation healthProfileInfo, string coachId);
	}

	public class UserCreator : IUserCreator
	{
		private readonly IUserTypeRepository _userTypeRepository;
		private readonly ICoachRepository _coachRepository;
		private readonly IPlayerRepository _playerRepository;
		private readonly IGoalsRepository _goalsRepository;

		public UserCreator(
			IUserTypeRepository userTypeRepository,
			ICoachRepository coachRepository,
			IPlayerRepository playerRepository,
			IGoalsRepository goalsRepository)
		{
			_coachRepository = coachRepository;
			_userTypeRepository = userTypeRepository;
			_playerRepository = playerRepository;
			_goalsRepository = goalsRepository;
		}

		public Coach CreateCoach(HealthProfileInformation healthProfileInfo)
		{
			var coach = new Coach(healthProfileInfo);

			_coachRepository.SaveCoach(coach);
			_userTypeRepository.SaveUserType(coach.UserId, UserType.Coach);

			return coach;
		}

		public Player CreatePlayer(string initialId, HealthProfileInformation profile, string coachId)
		{
			// Here we assume user is unassigned user
			var player = _playerRepository.GetPlayer(initialId);

			if (player.CoachId != coachId)
			{
				throw new InvalidOperationException("Player has been assigned to a differnt coach");
			}

			var newId = User.GenerateUserId(profile);
			if (_playerRepository.GetPlayer(newId)!= null)
			{
				throw new ExistingUserException("This player has already been created");
			}

			player.Map(profile);

			UpdateUnassignedToPlayer(initialId, player);

			return player;
		}

		private void UpdateUnassignedToPlayer(string initialId, Player player)
		{
			var userId = player.UserId;

			_playerRepository.RemovePlayer(initialId);
			_playerRepository.SavePlayer(player);

			_userTypeRepository.RemovePlayerTypeRecord(initialId);
			_userTypeRepository.SaveUserType(userId, UserType.Player, player.CoachId);

			var goals = _goalsRepository.GetGoals(initialId);
			_goalsRepository.RemoveGoals(initialId);
			_goalsRepository.SaveGoals(userId, goals);
		}
	}
}