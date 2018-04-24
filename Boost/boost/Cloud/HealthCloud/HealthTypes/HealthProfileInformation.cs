using System;

namespace boost.Cloud.HealthCloud.HealthTypes
{
	public class HealthProfileInformation
	{
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
	}
}