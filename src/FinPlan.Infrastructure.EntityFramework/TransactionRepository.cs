using FinPlan.Domain;
using FinPlan.Domain.Transactions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

		public async Task<PaginatedResult<List<Transaction>>> SearchTransactionsAsync(int accountId, string searchKeyword, int pageIndex, int recordsPerPage)
		{
			pageIndex = pageIndex < 0 ? 0 : pageIndex;
			var skipRecords = pageIndex * recordsPerPage;
			var transactions = new List<Transaction>();
			var totalPages = 0;
			int totalOfAllTransactions;
			Expression<Func<Transaction, bool>> whereExpresssion;
			if (string.IsNullOrWhiteSpace(searchKeyword))
			{
				whereExpresssion = transaction => transaction.Account.Id == accountId;
				totalOfAllTransactions = _dbContext.Transactions.Count(whereExpresssion);
				totalPages = (int)Math.Ceiling((decimal)totalOfAllTransactions / recordsPerPage);
				transactions = await _dbContext.Transactions.Where(whereExpresssion).Skip(skipRecords).Take(recordsPerPage).ToListAsync();
			}
			else
			{
				whereExpresssion = transaction =>
					transaction.Account.Id == accountId &&
					(
						(
							transaction.Title != null && transaction.Title.ToLowerInvariant().Contains(searchKeyword.ToLowerInvariant())
						)
						||
						(
							transaction.Note != null && transaction.Note.ToLowerInvariant().Contains(searchKeyword.ToLowerInvariant())
						)
					);
				totalOfAllTransactions = _dbContext.Transactions.Count(whereExpresssion);
				totalPages = (int)Math.Ceiling((decimal)totalOfAllTransactions / recordsPerPage);
				transactions = await _dbContext.Transactions.Where(whereExpresssion).Skip(skipRecords).Take(recordsPerPage)
					.ToListAsync();
			}

			return new PaginatedResult<List<Transaction>>(transactions, pageIndex, totalPages, totalOfAllTransactions);
		}
	}
}
