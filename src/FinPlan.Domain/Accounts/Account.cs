using FinPlan.Domain.Transactions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FinPlan.Domain.Accounts
{
	public class Account
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Currency { get; set; } = "AUD";
		public AccountCategory Category { get; set; } = AccountCategory.SpendingAndSaving;
		public AccountType Type { get; set; } = AccountType.Checking;
		public IdentityUser Owner { get; set; }
		public virtual List<Transaction> Transactions { get; set; } = new List<Transaction>();

		public Response AddTransactionsByUserId(IEnumerable<Transaction> transactions, string userId)
		{
			if (Owner.Id != userId)
			{
				return new Response(new List<string>
						{$"The user with id:{userId} does not have access to add transaction to this account"});
			}

			Transactions.AddRange(transactions);
			return new Response(true);
		}
	}
}