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

        public async Task<List<Account>> GetAccountsAsync()
        {
            return await _dbContext.Accounts.ToListAsync();
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _dbContext.Accounts.FindAsync(id);
        }

        public async Task DeleteByIdAsync(int requestId)
        {
            _dbContext.Accounts.Remove(await _dbContext.Accounts.FindAsync(requestId));
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAccountAsync(Account account)
        {
            await _dbContext.Accounts.AddAsync(account);
            await _dbContext.SaveChangesAsync();
        }
    }
}