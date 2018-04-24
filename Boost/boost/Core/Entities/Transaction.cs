using System;

namespace boost.Core.Entities
{
	public class Transaction
	{
		public string UserId { get; set; }
		public DateTime TransactionTime { get; set; }

		public int BalanceBefore { get; set; }
		public int TransactionAmount { get; set; }
		public string Details { get; set; }
	}
}
