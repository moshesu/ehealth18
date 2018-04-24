using System;

namespace boost.Cloud.HealthCloud.Exceptions
{
	public class TokenAcquiringException : HealthRequestException
	{
		public TokenAcquiringException(string message, Exception e) : base(message, e) { }
	}
}
