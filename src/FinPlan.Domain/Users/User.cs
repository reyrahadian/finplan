using System.Collections.Generic;
using System.Linq;
using FinPlan.Domain.Accounts;
using Microsoft.AspNetCore.Identity;

namespace FinPlan.Domain.Users
{
	public class User : IdentityUser
	{
		public virtual List<Account> Accounts { get; set; } = new List<Account>();

		public bool RemoveAccount(int accountId)
		{
			return Accounts.Remove(Accounts.FirstOrDefault(x => x.Id == accountId));
		}

		public Account GetAccount(int accountId)
		{
			return Accounts.SingleOrDefault(x => x.Id == accountId);
		}

		public void AddAccount(Account account)
		{
			account.Owner = this;
			Accounts.Add(account);
		}
	}
}