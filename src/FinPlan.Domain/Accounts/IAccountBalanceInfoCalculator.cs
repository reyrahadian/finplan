using System.Threading.Tasks;

namespace FinPlan.Domain.Accounts
{
	public interface IAccountBalanceInfoCalculator
	{
		Task<decimal> GetTotalCreditAsync(int accountId);
		Task<decimal> GetTotalDebitAsync(int accountId);
	}
}
