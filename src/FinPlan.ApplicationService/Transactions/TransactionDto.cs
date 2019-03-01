using System;
using FinPlan.ApplicationService.Accounts;

namespace FinPlan.ApplicationService.Transactions
{
	public class TransactionDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public decimal Amount { get; set; }
		public string Note { get; set; }
		public DateTime? Date { get; set; }
		public string Category { get; set; }
		public TransactionType Type { get; set; }
		public AccountDto Account { get; set; }
	}
}
