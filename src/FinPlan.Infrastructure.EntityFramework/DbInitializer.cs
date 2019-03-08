using FinPlan.Domain;
using FinPlan.Domain.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using FinPlan.Domain.Users;

namespace FinPlan.Infrastructure.EntityFramework
{
	public static class DbInitializer
	{
		public static void Initialize(ApplicationDbContext dbContext, UserManager<User> userManager)
		{
			dbContext.Database.Migrate();

			var defaultUser = SeedDefaultUser(userManager);
			if (defaultUser == null)
			{
				throw new Exception("Cannot create a default user");
			}

			SeedCategories(dbContext);
		}


		private static User SeedDefaultUser(UserManager<User> userManager)
		{
			const string userName = "rahadian.rey@gmail.com";
			var defaultUser = userManager.FindByNameAsync(userName).Result;
			if (defaultUser != null)
			{
				return defaultUser;
			}

			defaultUser = new User();
			defaultUser.UserName = userName;
			defaultUser.Email = defaultUser.UserName;
			var result = userManager.CreateAsync(defaultUser, "Pass@word1").Result;
			if (result.Succeeded)
			{
				return defaultUser;
			}

			return null;
		}

		private static void SeedCategories(ApplicationDbContext dbContext)
		{
			if (dbContext.Categories.Any())
			{
				return;
			}

			var categories = new List<TransactionCategory>();
			categories.Add(new TransactionCategory("Home"));
			categories.Add(new TransactionCategory("Household Utilities"));
			categories.Add(new TransactionCategory("Health & Beauty"));
			categories.Add(new TransactionCategory("Banking & Finance"));
			categories.Add(new TransactionCategory("Holiday & Travel"));
			categories.Add(new TransactionCategory("Groceries"));
			categories.Add(new TransactionCategory("Shopping"));
			categories.Add(new TransactionCategory("Insurance"));
			categories.Add(new TransactionCategory("Food & Drinks"));
			categories.Add(new TransactionCategory("Entertainment"));
			dbContext.Categories.AddRange(categories);
			dbContext.SaveChanges();
		}
	}
}