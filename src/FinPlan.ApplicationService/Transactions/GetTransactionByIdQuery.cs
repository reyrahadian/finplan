using FinPlan.ApplicationService.Transactions.Mappers;
using FinPlan.Domain.Transactions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Transactions
{
	public class GetTransactionByIdQuery : IRequest<TransactionDto>
	{
		public int Id { get; }
		public string UserId { get; }

		public GetTransactionByIdQuery(int id, string userId)
		{
			Id = id;
			UserId = userId;
		}
	}

	public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
	{
		private readonly ITransactionRepository _transactionRepository;

		public GetTransactionByIdHandler(ITransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
		{
			var transaction = await _transactionRepository.GetTransactionByIdAsync(request.Id);
			if (transaction == null)
			{
				return null;
			}

			if (transaction.HasAccess(request.UserId))
			{
				return TransactionDtoMapper.Map(transaction);
			}

			return null;
		}
	}
}
