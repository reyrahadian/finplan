using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinPlan.Domain.Accounts
{
	public interface IAccountRepository
	{
		Task<List<Account>> GetAccountsAsync();
	    Task<Account> GetAccountByIdAsync(int id);
	    Task<bool> DeleteByIdAsync(int requestId);
	    Task<bool> CreateAccountAsync(Account account);
		Task<bool> UpdateAccountAsync(Account account);
	}
}