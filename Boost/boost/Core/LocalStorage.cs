using boost.Core.Entities.Users;
using boost.Cloud.HealthCloud.HealthTypes;
using Windows.Storage;
using boost.Core.UserComparison;
using Newtonsoft.Json;

namespace boost.Core
{
	public interface ILocalStorage
	{
		string GetCurrentUserId();
		T GetCurrentUser<T>() where T : User;
		HealthProfileInformation GetProfileInfo();
		UserType GetUserCurrentType();
		Comparison GetComparison();
		void SetCurrentUserId(string userId);
		void SetCurrentUser(User user);
		void SetProfileInfo(HealthProfileInformation profile);
		void SetUserCurrentType(UserType type);
		void SetComparison(Comparison comparison);
		void RemoveCurrentUser();
	}

	public class LocalStorage : ILocalStorage
	{
		private const string UserId = "userId";
		private const string CurrentUser = "user";
		private const string ProfileInfo = "profile";
		private const string UserCurrentType = "type";
		private const string Comparison = "comparison";

		ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

		public T GetCurrentUser<T>() where T: User
		{
			return LoadValue<T>(CurrentUser);
		}

		public string GetCurrentUserId()
		{
			return LoadValue<string>(UserId);
		}

		public HealthProfileInformation GetProfileInfo()
		{
			return LoadValue<HealthProfileInformation>(ProfileInfo);
		}

		public UserType GetUserCurrentType()
		{
			return LoadValue<UserType>(UserCurrentType);
		}

		public Comparison GetComparison()
		{
			return LoadValue<Comparison>(Comparison);
		}

		public void RemoveCurrentUser()
		{
			SetCurrentUser(null);
			SetCurrentUserId(null);
			SetProfileInfo(null);
			SetUserCurrentType(UserType.None);
		}

		public void SetCurrentUser(User user)
		{
			SaveValue(CurrentUser, user);
		}

		public void SetCurrentUserId(string userId)
		{
			SaveValue(UserId, userId);
		}

		public void SetProfileInfo(HealthProfileInformation profile)
		{
			SaveValue(ProfileInfo, profile);
		}

		public void SetUserCurrentType(UserType type)
		{
			SaveValue(UserCurrentType, type);
		}

		public void SetComparison(Comparison comparison)
		{
			SaveValue(Comparison, comparison);
		}

		public void SaveValue<T>(string key, T t)
		{
			var convertedString = JsonConvert.SerializeObject(t);
			_localSettings.Values[key] = convertedString;
		}

		public T LoadValue<T>(string key)
		{
			var serializedString = (string) _localSettings.Values[key];

			if (serializedString == null)
				return default(T);

			return JsonConvert.DeserializeObject<T>(serializedString);
		}
	}
}
