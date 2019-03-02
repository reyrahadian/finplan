using FinPlan.Domain.Accounts;
using FinPlan.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinPlan.Infrastructure.EntityFramework
{
	public class AccountRepository : IAccountRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public AccountRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Account>> GetAccountsAsync()
		{
			return await _dbContext.Accounts.ToListAsync();
		}

		public async Task<Account> GetAccountByIdAsync(int id)
		{
			return await _dbContext.Accounts.FindAsync(id);
		}

		public async Task<bool> DeleteByIdAsync(int requestId)
		{
			_dbContext.Accounts.Remove(await _dbContext.Accounts.FindAsync(requestId));
			var result = await _dbContext.SaveChangesAsync();
			return result != 0;
		}

		public async Task<bool> CreateAccountAsync(Account account)
		{
			await _dbContext.Accounts.AddAsync(account);
			var result = await _dbContext.SaveChangesAsync();
			return result != 0;
		}

		public async Task<bool> UpdateAccountAsync(Account account)
		{
			_dbContext.Accounts.Update(account);
			var result = await _dbContext.SaveChangesAsync();

			return result != 0;
		}

		public async Task<List<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
		{
			return await _dbContext.Transactions.Where(x => x.Account.Id == accountId).ToListAsync();
		}
	}
}