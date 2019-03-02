using FinPlan.ApplicationService.Transactions;
using System.Collections.Generic;

namespace FinPlan.Web.ViewComponents.Account
{
	public class TransactionListingViewModel
	{
		public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
		public bool ShowActionButtons { get; set; }
	}
}