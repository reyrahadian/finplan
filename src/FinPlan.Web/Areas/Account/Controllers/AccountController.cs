using System.Threading.Tasks;
using FinPlan.ApplicationService.Accounts;
using FinPlan.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinPlan.Web.Areas.Account.Controllers
{
    [Area("Account")]
	public class AccountController : Controller
	{
		private readonly IMediator _service;

		public AccountController(IMediator service)
		{
			_service = service;
		}

		public async Task<IActionResult> Index()
		{
			var accounts = await _service.Send(new GetAccountsRequest());

			return View(accounts);
		}

		public IActionResult Create()
		{
			return View(new AccountFormViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(AccountFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			TempData["message"] = "New account has been successfully created";
			return View("Index");
		}
	}
}