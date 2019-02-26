using System;
using System.Collections.Generic;
using System.Linq;
using FinPlan.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinPlan.Infrastructure.EntityFramework
{
	public static class DbInitializer
	{
		public static void Initialize(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
		{
			dbContext.Database.Migrate();

			var defaultUser = SeedDefaultUser(userManager);
			if (defaultUser == null) throw new Exception("Cannot create a default user");

			SeedCategories(dbContext);
		}


		private static IdentityUser SeedDefaultUser(UserManager<IdentityUser> userManager)
		{
			const string userName = "rahadian.rey@gmail.com";
			var defaultUser = userManager.FindByNameAsync(userName).Result;
			if (defaultUser != null) return defaultUser;

			defaultUser = new IdentityUser(userName);
			defaultUser.Email = defaultUser.UserName;
			var result = userManager.CreateAsync(defaultUser, "Pass@word1").Result;
			if (result.Succeeded) return defaultUser;

			return null;
		}

		private static void SeedCategories(ApplicationDbContext dbContext)
		{
			if (dbContext.Categories.Any()) return;

			var categories = new List<Category>();
			categories.Add(new Category("Home"));
			categories.Add(new Category("Household Utilities"));
			categories.Add(new Category("Health & Beauty"));
			categories.Add(new Category("Banking & Finance"));
			categories.Add(new Category("Holiday & Travel"));
			categories.Add(new Category("Groceries"));
			categories.Add(new Category("Shopping"));
			categories.Add(new Category("Insurance"));
			categories.Add(new Category("Food & Drinks"));
			categories.Add(new Category("Entertainment"));
			dbContext.Categories.AddRange(categories);
			dbContext.SaveChanges();
		}
	}
}