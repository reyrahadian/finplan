using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinPlan.Domain.Accounts
{
	public interface IAccountRepository
	{
		Task<List<Account>> GetAccountsAsync();
	    Task<Account> GetAccountByIdAsync(int id);
	    Task DeleteByIdAsync(int requestId);
	    Task CreateAccountAsync(Account account);
	}
}