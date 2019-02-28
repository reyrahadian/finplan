using FinPlan.ApplicationService.Accounts;
using FinPlan.Web.Models.Account;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinPlan.Web.Controllers
{
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
		public async Task<IActionResult> Create(AccountFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _service.Send(new CreateAccountCommand
			{
				Account = new AccountDto
				{
					Name = model.Name,
					Category = model.Category.ToString(),
					Type = model.Type.ToString(),
					Owner = User.Identity.Name

				}
			});
			if (result.IsSuccessful)
			{
				TempData["message"] = "New account has been successfully created";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("account", "Failed to create an account");
			return View(model);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var account = await _service.Send(new GetAccountByIdRequest { Id = id });
			if (account == null)
			{
				return NotFound();
			}

			var model = new AccountFormViewModel();
			model.MapFrom(account);

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(AccountFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _service.Send(new UpdateAccountCommand
			{
				Account = new AccountDto
				{
					Id = model.Id.Value,
					Name = model.Name,
					Category = model.Category.ToString(),
					Type = model.Type.ToString()
				}
			});

			if (result.IsSuccessful)
			{
				TempData["message"] = "account has been successfully updated";
				return RedirectToAction("AccountView", new { id = @model.Id });
			}

			ModelState.AddModelError("account", "Failed to update the account");
			return View(model);
		}

		public IActionResult Delete(int id)
		{
			_service.Send(new DeleteAccountByIdCommand { Id = id });

			return View("Index");
		}

		public async Task<IActionResult> AccountView(int id)
		{
			var account = await _service.Send(new GetAccountByIdRequest { Id = id });
			if (account == null)
			{
				return NotFound();
			}

			var model = new AccountViewModel();
			model.Account = account;

			return View(model);
		}
	}
}