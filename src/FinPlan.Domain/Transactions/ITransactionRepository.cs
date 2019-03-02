using System.Threading.Tasks;

namespace FinPlan.Domain.Transactions
{
	public interface ITransactionRepository
	{
		Task<Transaction> GetTransactionByIdAsync(int id);
		Task<bool> UpdateTransactionAsync(Transaction transaction);
	}
}
