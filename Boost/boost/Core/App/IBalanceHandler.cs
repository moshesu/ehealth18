using System;

namespace boost.Core.App
{
	public interface IBalanceHandler
	{
		int GetBalance(string userId = null);
		void SendTransaction(int amount, string details = null , string userId = null);
	}
}
