using System;

namespace boost.Cloud.HealthCloud.Exceptions
{
	public class HealthRequestException : Exception
	{
		public HealthRequestException(string message) : base(message) { }
		public HealthRequestException(string message, Exception e) : base(message, e) { }
	}
}
