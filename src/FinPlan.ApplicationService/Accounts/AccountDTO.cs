using FinPlan.ApplicationService.Transactions;
using System.Collections.Generic;

namespace FinPlan.ApplicationService.Accounts
{
	public class AccountDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Currency { get; set; }
		public string Category { get; set; }
		public string Type { get; set; }
		public string UserId { get; set; }
		public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
	}
}