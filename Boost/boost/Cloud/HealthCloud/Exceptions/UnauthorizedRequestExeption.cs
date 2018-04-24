namespace boost.Cloud.HealthCloud.Exceptions
{
	public class UnauthorizedRequestExeption : HealthRequestException
	{
		public UnauthorizedRequestExeption(string message) : base(message) { }
	}
}
