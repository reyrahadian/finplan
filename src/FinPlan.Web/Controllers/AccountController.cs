using FinPlan.ApplicationService.Accounts;
using FinPlan.ApplicationService.Currencies;
using FinPlan.ApplicationService.Transactions;
using FinPlan.Web.Models.Account;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinPlan.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly IMediator _service;
		private readonly UserManager<IdentityUser> _userManager;

		public AccountController(IMediator service, IHostingEnvironment hostingEnvironment,
			UserManager<IdentityUser> userManager)
		{
			_service = service;
			_hostingEnvironment = hostingEnvironment;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var accounts = await _service.Send(new GetAccountsRequestQuery());

			return View(accounts);
		}

		public async Task<IActionResult> Create()
		{
			var currencies = await _service.Send(new GetCurrenciesQuery());
			var model = new AccountFormViewModel();
			model.Currencies = currencies.Select(x => new SelectListItem(x.EnglishName, x.ISOCurrencySymbol));

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(AccountFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _service.Send(new CreateAccountCommand(
				new AccountDto
				{
					Name = model.Name,
					Category = model.Category.ToString(),
					Type = model.Type.ToString(),
					Owner = User.Identity.Name
				})
			);
			if (result.IsSuccessful)
			{
				TempData["message"] = "New account has been successfully created";
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("account", "Failed to create an account");

			var currencies = await _service.Send(new GetCurrenciesQuery());
			model.Currencies = currencies.Select(x => new SelectListItem(x.EnglishName, x.ISOCurrencySymbol));

			return View(model);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var account = await _service.Send(new GetAccountByIdQuery { Id = id });
			if (account == null)
			{
				return NotFound();
			}

			var model = new AccountFormViewModel();
			var currencies = await _service.Send(new GetCurrenciesQuery());
			model.Currencies = currencies.Select(x => new SelectListItem(x.EnglishName, x.ISOCurrencySymbol));
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
			(
				new AccountDto
				{
					Id = model.Id.Value,
					Name = model.Name,
					Category = model.Category.ToString(),
					Type = model.Type.ToString()
				}
			));

			if (result.IsSuccessful)
			{
				TempData["message"] = "account has been successfully updated";
				return RedirectToAction("AccountView", new { id = model.Id });
			}

			ModelState.AddModelError("account", "Failed to update the account");

			var currencies = await _service.Send(new GetCurrenciesQuery());
			model.Currencies = currencies.Select(x => new SelectListItem(x.EnglishName, x.ISOCurrencySymbol));
			return View(model);
		}

		public IActionResult Delete(int id)
		{
			_service.Send(new DeleteAccountByIdCommand(id));

			return View("Index");
		}

		public async Task<IActionResult> AccountView(int id)
		{
			var account = await _service.Send(new GetAccountByIdQuery { Id = id });
			if (account == null)
			{
				return NotFound();
			}

			var model = new AccountViewModel();
			model.Account = account;
			var transactions = await _service.Send(new GetTransactionsByAccountIdQuery(id));
			model.Account.Transactions = transactions;

			return View(model);
		}

		public IActionResult ImportBankStatement(int id)
		{
			return View(new ImportBankStatementViewModel
			{
				AccountId = id
			});
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ImportBankStatement(ImportBankStatementViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			if (model.File?.Length > 0)
			{
				var fileUploadsFolderPath = _hostingEnvironment.ContentRootPath + "\\fileUploads";
				if (!Directory.Exists(fileUploadsFolderPath))
				{
					Directory.CreateDirectory(fileUploadsFolderPath);
				}

				var filePath = fileUploadsFolderPath + "\\" + model.File.FileName;
				if (!System.IO.File.Exists(filePath))
				{
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await model.File.CopyToAsync(stream);
					}
				}

				model.UploadedFilePath = filePath;
			}

			if (model.HasConfirmedToImport)
			{
				var result = await _service.Send(new ImportBankStatementCommand
				{
					FilePath = model.UploadedFilePath,
					Year = model.Year,
					AccountId = model.AccountId,
					UserId = (await _userManager.GetUserAsync(User)).Id
				});

				if (result.IsSuccessful)
				{
					return RedirectToAction("AccountView", new { id = model.AccountId });
				}

				TempData["message"] = "Failed to import bank statement";
				return View(model);
			}
			else
			{
				var result = await _service.Send(new ImportBankStatementPreviewQuery
				{
					FilePath = model.UploadedFilePath,
					Year = model.Year
				});

				model.Transactions = result;
				return View(model);
			}
		}

		public async Task<IActionResult> EditTransaction(int accountId, int transactionId)
		{
			var userId = (await _userManager.GetUserAsync(User)).Id;
			var transaction = await _service.Send(new GetTransactionByIdQuery(transactionId, userId));

			var model = new TransactionFormViewModel();
			model.AccountId = accountId;
			model.MapFrom(transaction);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditTransaction(TransactionFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var userId = (await _userManager.GetUserAsync(User)).Id;
			var result = await _service.Send(new UpdateTransactionCommand(model.MapToDto(), userId));
			if (result.IsSuccessful)
			{
				TempData["message"] = "Transaction has been successfully updated";
				return RedirectToAction("AccountView", new { id = model.AccountId });
			}

			result.ErrorMessages.ForEach(x => ModelState.AddModelError("", x));
			return View(model);
		}

		public IActionResult DeleteTransaction()
		{
			throw new System.NotImplementedException();
		}
	}
}