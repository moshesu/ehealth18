using System;
using boost.Core.Entities;

namespace boost.Repositories
{
	public interface ITransactionRepository
	{
		Transaction GetLastTransaction(string userId);
		void SaveTransaction(string userId, Transaction transaction);
		void SaveTransaction(string userId, Transaction[] transactions);
		Transaction[] GetTransactionHistory(string userId, DateTime start, DateTime end);
	}
}
