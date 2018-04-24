namespace boost.Cloud.HealthCloud.Exceptions
{
	public class UserDeniedAccessExeption : HealthRequestException
	{
		public UserDeniedAccessExeption(string message) : base(message) { }
	}
}
