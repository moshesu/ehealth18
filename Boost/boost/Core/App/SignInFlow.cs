using System;
using boost.Core.Entities.Users;
using boost.Cloud.HealthCloud.DataFetcher;
using boost.Repositories;
using boost.Core.UserActivity;
using boost.PushNotifications;

namespace boost.Core.App
{
	public interface ISignInFlow
	{
		UserType SignIn();
		void SignOut();
		Player NewPlayerFlow(string initialId);
		Coach NewCoachFlow();
		Player GetPlayer();
		Coach GetCoach();
	}

	public class SignInFlow : ISignInFlow
	{
		private readonly ILocalStorage _localStorage;
		private readonly IUserTypeRepository _usertypeRepository;
		private readonly ICoachRepository _coachRepository;
		private readonly IPlayerRepository _playerRepository;
		private readonly PlayerDataUpdater _dataSync;
		private readonly PlayerActivityUpdater _activitiesSync;
		private readonly UserCreator _userCreator;
		private readonly INotificationsCenter _notificationCenter;

		public SignInFlow(ILocalStorage localStorage,
			IUserTypeRepository usertypeRepository,
			ICoachRepository coachRepository,
			IPlayerRepository playerRepository,
			PlayerDataUpdater playerDataUpdater,
			PlayerActivityUpdater activitiesSync,
			UserCreator userCreator,
			INotificationsCenter notificationCenter)
		{
			_localStorage = localStorage;
			_usertypeRepository = usertypeRepository;
			_coachRepository = coachRepository;
			_playerRepository = playerRepository;
			_dataSync = playerDataUpdater;
			_activitiesSync = activitiesSync;
			_userCreator = userCreator;
			_notificationCenter = notificationCenter;
		}

		public UserType SignIn()
		{
			var type = UserType.None;
			var profile = HealthDataFetcher.Instance.GetProfileInformation();
			var userId = User.GenerateUserId(profile);

			_localStorage.SetCurrentUserId(userId);
			_localStorage.SetProfileInfo(profile);

			try
			{
				var userTypeRecord = _usertypeRepository.GetUserType(userId);
				if (userTypeRecord == null)
					return type;

				type = userTypeRecord.UserType;
			}
			catch (Exception e)
			{
				return type;
			}
			

			if (type == UserType.CoachAndPlayer)
				return type;

			SaveUserToLocalStorage(type);
			return type;
		}

		public void SaveUserToLocalStorage(UserType type)
		{ 
			var userId = _localStorage.GetCurrentUserId();
			_localStorage.SetUserCurrentType(type);

			if (type == UserType.Player)
			{
				_dataSync.SyncPlayerData();
				_activitiesSync.SyncPlayerActivitiesToDB();
				_notificationCenter.Initialize();

				_localStorage.SetCurrentUser(_playerRepository.GetPlayer(userId));
			}

			else
				_localStorage.SetCurrentUser(_coachRepository.GetCoach(userId));
		}

		public Coach GetCoach()
        {
            return _coachRepository.GetCoach(GetUserId());
        }

		public Player GetPlayer()
		{
			return _playerRepository.GetPlayer(GetUserId());
		}

		public Coach NewCoachFlow()
		{
			var profileInfo = _localStorage.GetProfileInfo();

			var coach = _userCreator.CreateCoach(profileInfo);
			_localStorage.SetCurrentUser(coach);

			return coach;
		}

		public Player NewPlayerFlow(string initialUserId)
		{
			var profileInfo = _localStorage.GetProfileInfo();
			var initialUser = _usertypeRepository.GetUserType(initialUserId);
			if (initialUser == null)
				return null;
			var coachId = initialUser.CoachId;

			var player = _userCreator.CreatePlayer(initialUserId, profileInfo, coachId);
			_localStorage.SetCurrentUser(player);

			return player;
		}

		public void SignOut()
		{
			_localStorage.RemoveCurrentUser();
			HealthDataFetcher.Instance.ClearVault();
		}

		private string GetUserId()
		{
			return _localStorage.GetCurrentUserId();
		}
	}
}
