using Microsoft.AspNetCore.Identity;

namespace FinPlan.Web.Data
{
	public static class DbUserInitializer
	{
		public static void Initialize(UserManager<IdentityUser> userManager)
		{
			const string userName = "rahadian.rey@gmail.com";
			if (userManager.FindByNameAsync(userName).Result != null) return;

			var defaultUser = new IdentityUser(userName);
			defaultUser.Email = defaultUser.UserName;
			var identityResult = userManager.CreateAsync(defaultUser, "Pass@word1").Result;
		}
	}
}