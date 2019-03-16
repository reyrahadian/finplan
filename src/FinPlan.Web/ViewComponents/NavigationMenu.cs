using FinPlan.ApplicationService.Accounts;
using FinPlan.Domain.Users;
using FinPlan.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinPlan.Web.ViewComponents
{
	public class NavigationMenu : ViewComponent
	{
		private readonly IMediator _service;
		private readonly UserManager<User> _userManager;

		public NavigationMenu(IMediator service, UserManager<User> userManager)
		{
			_service = service;
			_userManager = userManager;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var items = new List<NavigationMenuViewModel>();
			items.Add(new NavigationMenuViewModel
			{
				Title = "Home",
				Uri = Url.Action("Index", "Dashboard")
			});

			var userId = (await _userManager.GetUserAsync(UserClaimsPrincipal)).Id;
			var accounts = await _service.Send(new GetAccountsRequestQuery(userId));
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
