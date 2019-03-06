using System.Collections.Generic;
using System.Threading.Tasks;
using FinPlan.Domain.Transactions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;

namespace FinPlan.Domain.Accounts
{
	public class Account
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Currency { get; set; } = "AUD";
		public AccountCategory Category { get; set; } = AccountCategory.SpendingAndSaving;
		public AccountType Type { get; set; } = AccountType.Checking;
		public virtual IdentityUser Owner { get; set; }
		public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();

		public Response AddTransactionsByUserId(IEnumerable<Transaction> transactions, string userId)
		{
			if (!HasAccess(userId))
			{
				return new Response(new List<string>
					{$"The user with id:{userId} does not have access to add transaction to this account"});
			}

			Transactions.AddRange(transactions);
			return new Response(true);
		}

		public Response AddTransactionByUserId(Transaction transaction, string userId)
		{
			return AddTransactionsByUserId(new List<Transaction> {transaction}, userId);
		}

		public bool HasAccess(string userId)
		{
			return Owner.Id == userId;
		}

		public async Task<decimal> GetTotalDebit(IAccountBalanceInfoCalculator accountBalanceInfoCalculator)
		{
			return await accountBalanceInfoCalculator.GetTotalDebitAsync(Id);
		}

		public async Task<decimal> GetTotalCredit(IAccountBalanceInfoCalculator accountBalanceInfoCalculator)
		{
			return await accountBalanceInfoCalculator.GetTotalCreditAsync(Id);
		}
	}
}