using FinPlan.Domain.Transactions;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Transactions
{
	public class DeleteTransactionCommand : IRequest<CommandResponse>
	{
		public int TransactionId { get; }

		public DeleteTransactionCommand(int transactionId, string userId)
		{
			TransactionId = transactionId;
			UserId = userId;
		}

		public string UserId { get; }
	}

	public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, CommandResponse>
	{
		private readonly ITransactionRepository _transactionRepository;

		public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		public async Task<CommandResponse> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
		{
			var transaction = await _transactionRepository.GetTransactionByIdAsync(request.TransactionId);
			if (!transaction.HasAccess(request.UserId))
			{
				return new CommandResponse(new List<string> { "The user does not have access to delete this transaction" });
			}

			var isSuccessful = await _transactionRepository.DeleteByIdAsync(request.TransactionId);
			if (isSuccessful)
			{
				return new CommandResponse(true);
			}

			return new CommandResponse(new List<string> { "Failed to delete transaction" });
		}
	}
}
