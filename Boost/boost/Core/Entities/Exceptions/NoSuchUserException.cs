using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace boost.Core.Entities.Exceptions
{
	public class NoSuchUserException : Exception
	{
		public NoSuchUserException(string message) : base(message) { }
		public NoSuchUserException(string message, Exception e) : base(message, e) { }
	}
}
