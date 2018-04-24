using System;
using System.Linq;
using boost.Core.Entities;
using boost.Repositories;
using Newtonsoft.Json;

namespace boost.Cloud.AzureDatabase
{
	public class TransactionRepository : AbstractAzureRepository, ITransactionRepository
	{
		public Transaction GetLastTransaction(string userId)
		{
			var allTransactions = GetTransactionHistory(userId, new DateTime(2000, 1, 1), DateTime.Now.AddDays(1));
			return allTransactions.OrderByDescending(x => x.TransactionTime).FirstOrDefault();
		}

		public Transaction[] GetTransactionHistory(string userId, DateTime startTime, DateTime endTime)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);
			var startTimeParameter = new Parameter("startTime", startTime.Ticks.ToString());
			var endTimeParameter = new Parameter("endTime", endTime.Ticks.ToString());

			var result = CallAzureDatabase("GetTransactions", userIdParameter, startTimeParameter, endTimeParameter);
			if (result == null)
				return new Transaction[0];

			return JsonConvert.DeserializeObject<Transaction[]>(result);
		}

		public void SaveTransaction(string userId, Transaction transaction)
		{
			SaveTransaction(userId, new[] {transaction});
		}

		public void SaveTransaction(string userId, Transaction[] transactions)
		{
			var userIdParameter = new Parameter(UserIdKey, userId);

			var json = JsonConvert.SerializeObject(transactions, DateTimeConverter);
			var dataParameter = new Parameter("data", json);

			CallAzureDatabase("SaveTransactions", userIdParameter, dataParameter);
		}
	}
}