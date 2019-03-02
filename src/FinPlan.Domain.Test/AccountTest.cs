using FinPlan.Domain.Accounts;
using FinPlan.Domain.Transactions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Xunit;

namespace FinPlan.Domain.Test
{
	public class AccountTest
	{
		[Fact]
		public void AddTransactionsByUserId_ShouldSucceed_WhenAddedByCorrectUser()
		{
			var account = new Account();
			account.Owner = new IdentityUser();
			account.Owner.Id = "1";
			var response = account.AddTransactionsByUserId(new List<Transaction>(), "1");
			response.IsSuccessful.Should().BeTrue();
		}

		[Fact]
		public void AddTransactionsByUserId_ShouldFail_WhenAddedByIncorrectUser()
		{
			var account = new Account();
			account.Owner = new IdentityUser();
			account.Owner.Id = "2";
			var response = account.AddTransactionsByUserId(new List<Transaction>(), "1");
			response.IsSuccessful.Should().BeFalse();
		}
	}
}
