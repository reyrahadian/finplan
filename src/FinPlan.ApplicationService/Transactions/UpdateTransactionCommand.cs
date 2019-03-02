using FinPlan.Domain.Transactions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Transactions
{
	public class UpdateTransactionCommand : IRequest<CommandResponse>
	{
		public UpdateTransactionCommand(TransactionDto transaction, string userId)
		{
			Transaction = transaction;
			UserId = userId;
		}

		public TransactionDto Transaction { get; }
		public string UserId { get; }
	}

	public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, CommandResponse>
	{
		private readonly ITransactionRepository _transactionRepository;

		public UpdateTransactionCommandHandler(ITransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		public async Task<CommandResponse> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
		{
			var transaction = await _transactionRepository.GetTransactionByIdAsync(request.Transaction.Id);

			if (!transaction.HasAccess(request.UserId))
			{
				return new CommandResponse(new List<string> { $"User id:{request.UserId} does not have access to update the transaction" });
			}

			transaction.Date = request.Transaction.Date;
			transaction.Amount = request.Transaction.Amount;
			transaction.Note = request.Transaction.Note;
			transaction.Title = request.Transaction.Title;
			//transaction.TransactionCategory = request.Transaction.Category
			transaction.Type = Enum.Parse<Domain.Transactions.TransactionType>(request.Transaction.Type.ToString());

			var isSuccessful = await _transactionRepository.UpdateTransactionAsync(transaction);
			if (isSuccessful)
			{
				return new CommandResponse(true);
			}

			return new CommandResponse(new List<string> { "Failed to update the transaction" });
		}
	}
}
