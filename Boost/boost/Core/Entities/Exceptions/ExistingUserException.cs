using System;

namespace boost.Core.Entities.Exceptions
{
	public class ExistingUserException : Exception
	{
		public ExistingUserException(string message) : base(message) { }
		public ExistingUserException(string message, Exception e) : base(message, e) { }
	}
}
