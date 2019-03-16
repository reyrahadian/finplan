using FinPlan.Domain.Accounts;
using FinPlan.Domain.Transactions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace FinPlan.Domain.Users
{
	public class User : IdentityUser
	{
		/// <summary>
		/// This property should only be accessed from the domain layer
		/// </summary>
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

		public List<Account> GetAccounts()
		{
			return Accounts;
		}

		public List<Transaction> GetAccountTransactions(int accountId)
		{
			var account = Accounts.SingleOrDefault(x => x.Id == accountId);
			if (account == null)
			{
				return new List<Transaction>();
			}

			return account.Transactions;
		}

		public Transaction GetAccountTransaction(int accountId, int transactionId)
		{
			var account = Accounts.SingleOrDefault(x => x.Id == accountId);
			if (account == null)
			{
				return null;
			}

			var transaction = account.Transactions.FirstOrDefault(x => x.Id == transactionId);

			return transaction;
		}
	}
}