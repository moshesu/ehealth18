using System;
using System.Security.Cryptography;
using System.Text;
using boost.Cloud.HealthCloud.DataFetcher;
using boost.Cloud.HealthCloud.HealthTypes;

namespace boost.Core.Entities.Users
{
	public abstract class User
	{
		public string UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Gender { get; set; }

		public int Height { get; set; }
		public int Weight { get; set; }

		public string PostalCode { get; set; }
		public string PreferredLocale { get; set; }

		public DateTime Birthdate { get; set; }
		public DateTime CreatedTime { get; set; }
		public DateTime LastUpdateTime { get; set; }

		public User(){}

		public User(HealthProfileInformation healthProfileInfo)
		{
			Map(healthProfileInfo);
		}

		public void Map(HealthProfileInformation healthProfileInfo)
		{
			UserId = GenerateUserId(healthProfileInfo);

			FirstName = healthProfileInfo.FirstName;
			LastName = healthProfileInfo.LastName;
			Gender = healthProfileInfo.Gender;
			Height = healthProfileInfo.Height;
			Weight = healthProfileInfo.Weight;
			PostalCode = healthProfileInfo.PostalCode;
			PreferredLocale = healthProfileInfo.PreferredLocale;
			Birthdate = healthProfileInfo.Birthdate;
			CreatedTime = healthProfileInfo.CreatedTime;
			LastUpdateTime = healthProfileInfo.LastUpdateTime;
		}

		public static string GenerateUserId(HealthProfileInformation healthProfileInfo)
		{
			var userInfoString = healthProfileInfo.FirstName +
					healthProfileInfo.Birthdate +
					healthProfileInfo.CreatedTime +
					healthProfileInfo.Gender;

			MD5 md5Hasher = MD5.Create();
			var hashedInfo = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(userInfoString));
			var intValue = BitConverter.ToInt32(hashedInfo, 0);

			return intValue.ToString();
		}
	}
}