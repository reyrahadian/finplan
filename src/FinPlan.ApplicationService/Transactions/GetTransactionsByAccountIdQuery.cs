using FinPlan.ApplicationService.Transactions.Mappers;
using FinPlan.Domain.Users;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Transactions
{
	public class GetTransactionsByAccountIdQuery : IRequest<List<TransactionDto>>
	{
		public string UserId { get; }
		public int AccountId { get; }

		public GetTransactionsByAccountIdQuery(string userId, int accountId)
		{
			UserId = userId;
			AccountId = accountId;
		}
	}

	public class GetTransactionsByAccountIdHandler : IRequestHandler<GetTransactionsByAccountIdQuery, List<TransactionDto>>
	{
		private readonly IUserRepository _userRepository;

		public GetTransactionsByAccountIdHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<List<TransactionDto>> Handle(GetTransactionsByAccountIdQuery request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetUserByIdAsync(request.UserId);
			if (user == null)
			{
				return new List<TransactionDto>();
			}

			var transactions = user.GetAccountTransactions(request.AccountId);

			return transactions.OrderByDescending(x => x.Date).Select(x => TransactionDtoMapper.Map(x)).ToList();
		}
	}
}
