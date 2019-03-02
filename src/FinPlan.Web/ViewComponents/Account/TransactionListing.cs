using FinPlan.ApplicationService.Transactions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinPlan.Web.ViewComponents.Account
{
	public class TransactionListing : ViewComponent
	{
		public Task<IViewComponentResult> InvokeAsync(List<TransactionDto> transactions, bool showActionButtons)
		{
			var model = new TransactionListingViewModel
			{
				Transactions = transactions,
				ShowActionButtons = showActionButtons
			};
			return Task.FromResult<IViewComponentResult>(View(model));
		}
	}
}
