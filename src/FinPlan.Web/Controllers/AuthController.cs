using FinPlan.Domain.Users;
using FinPlan.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FinPlan.Web.Controllers
{
	public class AuthController : Controller
	{
		private readonly SignInManager<User> _signInManager;

		public AuthController(SignInManager<User> signInManager)
		{
			_signInManager = signInManager;
		}

		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]
		public async Task<IActionResult> Login(AuthenticationFormViewModel model, string returnUrl = null)
		{
			returnUrl = returnUrl ?? Url.Action("Index", "Dashboard");
			if (ModelState.IsValid)
			{
				// This doesn't count login failures towards account lockout
				// To enable password failures to trigger account lockout, 
				// set lockoutOnFailure: true
				var result = await _signInManager.PasswordSignInAsync(model.Email,
					model.Password, model.RememberMe, false);
				if (result.Succeeded)
				{
					return LocalRedirect(returnUrl);
				}

				if (result.RequiresTwoFactor)
				{
					return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, model.RememberMe });
				}

				if (result.IsLockedOut)
				{
					return RedirectToPage("./Lockout");
				}

				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
				return View(model);
			}

			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();

			return View("Login");
		}

		public IActionResult Denied()
		{
			return View();
		}
	}
}