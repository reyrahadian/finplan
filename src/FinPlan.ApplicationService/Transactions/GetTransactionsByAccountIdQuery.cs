using FinPlan.ApplicationService.Transactions.Mappers;
using FinPlan.Domain.Accounts;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Transactions
{
	public class GetTransactionsByAccountIdQuery : IRequest<List<TransactionDto>>
	{
		public int AccountId { get; }

		public GetTransactionsByAccountIdQuery(int accountId)
		{
			AccountId = accountId;
		}
	}

	public class GetTransactionsByAccountIdHandler : IRequestHandler<GetTransactionsByAccountIdQuery, List<TransactionDto>>
	{
		private readonly IAccountRepository _accountRepository;

		public GetTransactionsByAccountIdHandler(IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		public async Task<List<TransactionDto>> Handle(GetTransactionsByAccountIdQuery request, CancellationToken cancellationToken)
		{
			var transactions = await _accountRepository.GetTransactionsByAccountIdAsync(request.AccountId);

			return transactions.OrderByDescending(x => x.Date).Select(x => TransactionDtoMapper.Map(x)).ToList();
		}
	}
}
