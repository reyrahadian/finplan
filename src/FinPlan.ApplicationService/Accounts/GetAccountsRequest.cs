using FinPlan.Domain.Accounts;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class GetAccountsRequest : IRequest<List<AccountDto>>
	{
	}

	public class GetAccountsHandler : IRequestHandler<GetAccountsRequest, List<AccountDto>>
	{
		private readonly IAccountRepository _accountRepository;

		public GetAccountsHandler(IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		public async Task<List<AccountDto>> Handle(GetAccountsRequest request, CancellationToken cancellationToken)
		{
			var accounts = await _accountRepository.GetAccountsAsync();
			return accounts.Select(x => new AccountDto
			{
				Id = x.Id,
				Category = x.Category.ToString(),
				Name = x.Name,
				Type = x.Type.ToString(),
				Owner = x.Owner?.ToString()
			}).ToList();
		}
	}
}