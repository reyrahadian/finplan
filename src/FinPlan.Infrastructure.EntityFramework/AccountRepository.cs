using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinPlan.Domain.Accounts;
using Microsoft.EntityFrameworkCore;

namespace FinPlan.Infrastructure.EntityFramework
{
	public class AccountRepository : IAccountRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public AccountRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<Account>> GetAccounts()
		{
			return await _dbContext.Accounts.ToListAsync();
		}
	}
}
