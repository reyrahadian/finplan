using FinPlan.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FinPlan.Infrastructure.EntityFramework
{
	public class TransactionRepository : ITransactionRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public TransactionRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Transaction> GetTransactionByIdAsync(int id)
		{
			return await _dbContext.Transactions.Include(x => x.Account).FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<bool> UpdateTransactionAsync(Transaction transaction)
		{
			_dbContext.Update(transaction);
			var result = await _dbContext.SaveChangesAsync();

			return result != 0;
		}

		public async Task<bool> DeleteByIdAsync(int id)
		{
			_dbContext.Remove(await _dbContext.Transactions.FindAsync(id));
			var result = await _dbContext.SaveChangesAsync();

			return result != 0;
		}
	}
}
