using FinPlan.ApplicationService.Transactions.Mappers;
using FinPlan.Domain.Accounts;
using System.Linq;

namespace FinPlan.ApplicationService.Accounts.Mappers
{
	public class AccountDtoMapper
	{
		public static AccountDto Map(Account account)
		{
			return new AccountDto
			{
				Id = account.Id,
				Type = account.Type.ToString(),
				Currency = account.Currency,
				Category = account.Category.ToString(),
				Name = account.Name,
				Transactions = account.Transactions.Select(x => TransactionDtoMapper.Map(x)).ToList(),
				Owner = account.Owner.Id
			};
		}
	}
}
