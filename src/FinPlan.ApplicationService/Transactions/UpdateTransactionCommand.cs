using FinPlan.Domain.Transactions;
using FinPlan.Domain.Users;
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
		private readonly IUserRepository _userRepository;
		private readonly ITransactionRepository _transactionRepository;

		public UpdateTransactionCommandHandler(IUserRepository userRepository, ITransactionRepository transactionRepository)
		{
			_userRepository = userRepository;
			_transactionRepository = transactionRepository;
		}

		public async Task<CommandResponse> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetUserByIdAsync(request.UserId);
			if (user == null)
			{
				return new CommandResponse("User doesn't exist");
			}

			var transaction = user.GetAccountTransaction(request.Transaction.Account.Id, request.Transaction.Id);

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
