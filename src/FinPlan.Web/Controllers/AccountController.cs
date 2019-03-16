using FinPlan.ApplicationService.Accounts;
using FinPlan.ApplicationService.Currencies;
using FinPlan.ApplicationService.Transactions;
using FinPlan.Domain.Users;
using FinPlan.Web.Models.Account;
using FinPlan.Web.Utils;
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
		private readonly UserManager<User> _userManager;

		public AccountController(IMediator service, IHostingEnvironment hostingEnvironment,
			UserManager<User> userManager)
		{
			_service = service;
			_hostingEnvironment = hostingEnvironment;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var userId = (await _userManager.GetUserAsync(User)).Id;
			var accounts = await _service.Send(new GetAccountsRequestQuery(userId));

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
				},
				(await _userManager.GetUserAsync(User)).Id
			));
			if (result.IsSuccessful)
			{
				FlashMessage.SetMessage(TempData, "New account has been successfully created");
				return RedirectToAction("Index");
			}

			ModelState.AddModelError("account", "Failed to create an account");

			var currencies = await _service.Send(new GetCurrenciesQuery());
			model.Currencies = currencies.Select(x => new SelectListItem(x.EnglishName, x.ISOCurrencySymbol));

			return View(model);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var userId = (await _userManager.GetUserAsync(User)).Id;
			var account = await _service.Send(new GetAccountByIdQuery(id, userId));
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
				},
				(await _userManager.GetUserAsync(User)).Id
			));

			if (result.IsSuccessful)
			{
				FlashMessage.SetMessage(TempData, "account has been successfully updated");
				return RedirectToAction("AccountView", new { id = model.Id });
			}

			ModelState.AddModelError("account", "Failed to update the account");

			var currencies = await _service.Send(new GetCurrenciesQuery());
			model.Currencies = currencies.Select(x => new SelectListItem(x.EnglishName, x.ISOCurrencySymbol));
			return View(model);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var userId = (await _userManager.GetUserAsync(User)).Id;
			await _service.Send(new DeleteAccountByIdCommand(id, userId));

			return RedirectToAction("Index");
		}

		private const int TransactionsPerPage = 50;
		public async Task<IActionResult> AccountView(int id, AccountViewModel model, int page = 0)
		{
			var userId = (await _userManager.GetUserAsync(User)).Id;
			var account = await _service.Send(new GetAccountByIdQuery(id, userId));
			if (account == null)
			{
				return NotFound();
			}

			var accountBalanceInfo = await _service.Send(new GetAccountBalanceInfoQuery(id, userId));
			model.TotalDebit = accountBalanceInfo.TotalDebit;
			model.TotalCredit = accountBalanceInfo.TotalCredit;
			model.Id = id;
			model.Account = account;
			var result = await _service.Send(new SearchTransactionsQuery(model.Account.Id, model.SearchKeyWord, page, TransactionsPerPage));
			model.PaginatedTransactions = result;

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

				FlashMessage.SetMessage(TempData, "Failed to import bank statement");
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
			var userId = await GetUserIdAsync();
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

			var userId = await GetUserIdAsync();
			var result = await _service.Send(new UpdateTransactionCommand(model.MapToDto(), userId));
			if (result.IsSuccessful)
			{
				FlashMessage.SetMessage(TempData, "Transaction has been successfully updated");
				return RedirectToAction("AccountView", new { id = model.AccountId });
			}

			result.ErrorMessages.ForEach(x => ModelState.AddModelError("", x));
			return View(model);
		}

		private async Task<string> GetUserIdAsync()
		{
			var userId = (await _userManager.GetUserAsync(User)).Id;
			return userId;
		}

		public async Task<IActionResult> DeleteTransaction(int accountId, int transactionId)
		{
			var userId = await GetUserIdAsync();
			var result = await _service.Send(new DeleteTransactionCommand(transactionId, userId));
			if (result.IsSuccessful)
			{
				FlashMessage.SetMessage(TempData, "Transaction has been successfully deleted");
			}
			else
			{
				FlashMessage.SetMessage(TempData, string.Join("<br/>", result.ErrorMessages), true);
			}

			return RedirectToAction("AccountView", new { id = accountId });
		}

		public IActionResult CreateTransaction(int accountId)
		{
			var model = new TransactionFormViewModel();
			model.AccountId = accountId;

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateTransaction(TransactionFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var userId = await GetUserIdAsync();
			var result = await _service.Send(new CreateTransactionCommand(model.AccountId, model.MapToDto(), userId));
			if (result.IsSuccessful)
			{
				FlashMessage.SetMessage(TempData, "Transaction has been successfully created");
				return RedirectToAction("AccountView", new { id = model.AccountId });
			}

			result.ErrorMessages.ForEach(x => ModelState.AddModelError("", x));
			return View(model);
		}
	}
}