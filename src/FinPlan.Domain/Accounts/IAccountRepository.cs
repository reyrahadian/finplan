using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinPlan.Domain.Accounts
{
	public interface IAccountRepository
	{
		Task<List<Account>> GetAccounts();
	}
}