using System;
using boost.Core.App;
using boost.Core.Entities;
using boost.PushNotifications;
using boost.Repositories;

namespace boost.Core.UserActivity
{
	public class BalanceHandler : IBalanceHandler
	{
		private readonly ITransactionRepository _transactionRepository;
		private readonly ILocalStorage _localStorage;
		private readonly INotificationsCenter _notificationCenter;
		private int _balance;

		public BalanceHandler(
			ITransactionRepository transactionRepository, 
			ILocalStorage localStorage, 
			INotificationsCenter notificationCenter)
		{
			_transactionRepository = transactionRepository;
			_localStorage = localStorage;
			_notificationCenter = notificationCenter;
		}

		public int GetBalance(string userId = null)
		{
			if (userId == null)
				userId = _localStorage.GetCurrentUserId();
				
			var balance = CalculateCurrentBalance(userId);
			return balance;
		}

		private int CalculateCurrentBalance(string userId)
		{
			var lastTransaction = _transactionRepository.GetLastTransaction(userId);
			if (lastTransaction == null)
				return 0;

			return lastTransaction.BalanceBefore + lastTransaction.TransactionAmount;
		}

		public void SendTransaction(int amount, string details = null, string userId = null)
		{
			if (userId == null)
				userId = _localStorage.GetCurrentUserId();
			var transaction = new Transaction
			{
				UserId = userId,
				BalanceBefore = GetBalance(userId),
				TransactionAmount = amount,
				TransactionTime = DateTime.Now,
				Details = details
			};

			_transactionRepository.SaveTransaction(userId, transaction);
			_balance = GetBalance(userId) + amount;
		}
	}
}
