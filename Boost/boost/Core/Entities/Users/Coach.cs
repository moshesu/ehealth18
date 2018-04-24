using boost.Cloud.HealthCloud.HealthTypes;

namespace boost.Core.Entities.Users
{
	public class Coach : User
	{
		public string PaymentLastDigits { get; set; }

		public Coach() { }
		public Coach(HealthProfileInformation healthProfileInfo) : base(healthProfileInfo)
		{
		}
	}
}