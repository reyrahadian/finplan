using FinPlan.Domain.Accounts;
using System;

namespace FinPlan.Domain.Transactions
{
	public class Transaction
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public decimal Amount { get; set; }
		public string Note { get; set; }
		public DateTime Date { get; set; }
		public TransactionCategory TransactionCategory { get; set; }
		public TransactionType Type { get; set; }
		public Account Account { get; set; }

		public bool HasAccess(string userId)
		{
			return Account.Owner.Id == userId;
		}
	}
}