using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinPlan.Domain.Transactions
{
	public interface ITransactionRepository
	{
		Task<Transaction> GetTransactionByIdAsync(int id);
		Task<bool> UpdateTransactionAsync(Transaction transaction);
		Task<bool> DeleteByIdAsync(int id);
		Task<PaginatedResult<List<Transaction>>> SearchTransactionsAsync(int accountId, string searchKeyword, int pageIndex, int recordsPerPage);
	}
}
