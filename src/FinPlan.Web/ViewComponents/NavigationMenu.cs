using FinPlan.ApplicationService.Accounts;
using FinPlan.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinPlan.Web.ViewComponents
{
	public class NavigationMenu : ViewComponent
	{
		private readonly IMediator _service;

		public NavigationMenu(IMediator service)
		{
			_service = service;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var items = new List<NavigationMenuViewModel>();
			items.Add(new NavigationMenuViewModel
			{
				Title = "Home",
				Uri = Url.Action("Index", "Dashboard")
			});

			var accounts = await _service.Send(new GetAccountsRequestQuery());
			if (accounts.Any())
			{
				items.AddRange(accounts.Select(x => new NavigationMenuViewModel
				{
					Title = $"{x.Name} ({x.Type.ToString()})",
					Uri = Url.Action("AccountView", "Account", new { id = x.Id })
				}));
			}


			return View(items);
		}
	}
}
