using System.Threading.Tasks;
using FinPlan.ApplicationService.Accounts;
using FinPlan.Web.Areas.Account.Models;
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
            if (!ModelState.IsValid) return View(model);

            TempData["message"] = "New account has been successfully created";
            return View("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var account = await _service.Send(new GetAccountByIdRequest {Id = id});
            if (account == null) return NotFound();
            var model = new AccountFormViewModel();
            model.MapFrom(account);

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(AccountFormViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            TempData["message"] = "account has been successfully updated";
            return View("Index");
        }

        public IActionResult Delete(int id)
        {
            _service.Send(new DeleteAccountByIdCommand {Id = id});

            return View("Index");
        }
    }
}