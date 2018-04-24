using boost.Core.Entities.Users;
using boost.Repositories;

namespace boost.Core.App
{
	public interface ICoachPlayersPage
	{
		Player[] GetPlayers();
		string AddNewPlayer();
	}
	public class CoachPlayersPage : ICoachPlayersPage
	{
		private readonly ILocalStorage _localStorage;
		private readonly IPlayerRepository _playerRepository;
		private readonly IUserTypeRepository _userTypeRepository;

		public CoachPlayersPage(
			ILocalStorage localStorage, 
			ICoachRepository coachRepository,
			IPlayerRepository playerRepository,
			IUserTypeRepository userTypeRepository)
		{
			_localStorage = localStorage;
			_playerRepository = playerRepository;
			_userTypeRepository = userTypeRepository;
		}

		public string AddNewPlayer()
		{
			var playerInitialId = _userTypeRepository.AddUnassignedPlayer(_localStorage.GetCurrentUserId());
			CreateUnassignedPlayer(playerInitialId);

			return playerInitialId;
		}

		private void CreateUnassignedPlayer(string playerInitialId)
		{
			var unassignedPlayer = CreateNewUnassigned(playerInitialId);
			var playerTypeRecord = new UserTypeRecord(playerInitialId, UserType.UnassignedPlayer, _localStorage.GetCurrentUserId());

			_playerRepository.SavePlayer(unassignedPlayer);
			_userTypeRepository.SaveUserType(playerTypeRecord);
		}

		public Player[] GetPlayers()
		{
			var coachId = _localStorage
				.GetCurrentUser<Coach>()
				.UserId;

			var players = _playerRepository.GetPlayersByCoach(coachId);

			return players;
		}

		private Player CreateNewUnassigned(string playerInitialId)
		{
			return new Player
			{
				UserId = playerInitialId,
				CoachId = _localStorage.GetCurrentUserId(),
				Birthdate = new System.DateTime(1970, 1, 1),
				CreatedTime = new System.DateTime(1970, 1, 1),
				LastUpdateTime = new System.DateTime(1970, 1, 1)
			};
		}
	}
}
