using FinPlan.Domain.Accounts;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class GetAccountsRequestQuery : IRequest<List<AccountDto>>
	{
	}

	public class GetAccountsHandler : IRequestHandler<GetAccountsRequestQuery, List<AccountDto>>
	{
		private readonly IAccountRepository _accountRepository;

		public GetAccountsHandler(IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		public async Task<List<AccountDto>> Handle(GetAccountsRequestQuery request, CancellationToken cancellationToken)
		{
			var accounts = await _accountRepository.GetAccountsAsync();
			return accounts.Select(x => new AccountDto
			{
				Id = x.Id,
				Category = x.Category.ToString(),
				Name = x.Name,
				Type = x.Type.ToString(),
				UserId = x.Owner?.ToString()
			}).ToList();
		}
	}
}