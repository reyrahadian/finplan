using FinPlan.Domain.Accounts;
using FinPlan.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FinPlan.Infrastructure.EntityFramework.Accounts
{
	public class AccountBalanceInfoCalculator : IAccountBalanceInfoCalculator
	{
		private readonly ApplicationDbContext _dbContext;

		public AccountBalanceInfoCalculator(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<decimal> GetTotalCreditAsync(int accountId)
		{
			return await _dbContext.Transactions.Where(x => x.Account.Id == accountId && x.Type == TransactionType.Income).SumAsync(x => x.Amount);
		}

		public async Task<decimal> GetTotalDebitAsync(int accountId)
		{
			return await _dbContext.Transactions.Where(x => x.Account.Id == accountId && x.Type == TransactionType.Expense).SumAsync(x => x.Amount);
		}
	}
}
