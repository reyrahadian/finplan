using FinPlan.ApplicationService;
using FinPlan.ApplicationService.Accounts;
using FinPlan.ApplicationService.Transactions;
using System.Collections.Generic;

namespace FinPlan.Web.Models.Account
{
	public class AccountViewModel
	{
		public int Id { get; set; }
		public AccountDto Account { get; set; }
		public string SearchKeyWord { get; set; }
		public PaginatedResult<List<TransactionDto>> PaginatedTransactions { get; set; }
		public decimal TotalDebit { get; set; }
		public decimal TotalCredit { get; set; }
	}
}
