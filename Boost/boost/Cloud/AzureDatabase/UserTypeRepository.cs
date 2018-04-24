using System;
using boost.Core.Entities.Users;
using boost.Repositories;
using Newtonsoft.Json;

namespace boost.Cloud.AzureDatabase
{
	public class UserTypeRepository : AbstractAzureRepository, IUserTypeRepository
	{
		private const int InitialIdLength = 5;

		public UserTypeRecord GetUserType(string userId)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);

			var result = CallAzureDatabase("GetUserType", userIdParameter);
			if (result == null)
				return null;

			return JsonConvert.DeserializeObject<UserTypeRecord>(result);
		}

		public string AddUnassignedPlayer(string coachId)
		{
			bool generate = true;
			string initialId = null;

			while (generate)
			{
				initialId = GetInitialId(InitialIdLength);
				if (GetUserType(initialId) == null)
					generate = false;
			}

			SaveUserType(initialId, UserType.UnassignedPlayer, coachId);
			return initialId;
		}

		public void RemovePlayerTypeRecord(string userId)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);

			CallAzureDatabase("RemovePlayerType", userIdParameter);
		}

		public void SaveUserType(string userId, UserType type, string coachId = null)
		{
			SaveUserType(new UserTypeRecord
			{
				UserId = userId,
				UserType = type,
				CoachId = coachId
			});
		}

		public void SaveUserType(UserTypeRecord record)
		{
			var userIdParameter = new Parameter(UserIdKey, record.UserId);

			var json = JsonConvert.SerializeObject(record);
			var dataParameter = new Parameter("data", json);

			CallAzureDatabase("SaveUserType", userIdParameter, dataParameter);
		}

		public void UpdateNewPlayerUserType(string initialId, string newId)
		{
			var userTypeRecord = new UserTypeRecord();
			RemovePlayerTypeRecord(initialId);

			userTypeRecord.UserId = newId;
			userTypeRecord.UserType = UserType.Player;

			var currentType = GetUserType(newId);

			if (currentType?.UserType == UserType.Coach)
				userTypeRecord.UserType = UserType.CoachAndPlayer;

			SaveUserType(userTypeRecord);
		}

		private string GetInitialId(int length)
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var stringChars = new char[length];
			var random = new Random();

			for (int i = 0; i < stringChars.Length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}

			return new String(stringChars);
		}
	}
}