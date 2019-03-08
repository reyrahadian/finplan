using FinPlan.ApplicationService.Transactions.Mappers;
using FinPlan.Domain;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FinPlan.Domain.Users;

namespace FinPlan.ApplicationService.Transactions
{
	public class CreateTransactionCommand : IRequest<CommandResponse>
	{
		public CreateTransactionCommand(int accountId, TransactionDto transaction, string userId)
		{
			Transaction = transaction;
			UserId = userId;
			AccountId = accountId;
		}

		public TransactionDto Transaction { get; }

		public int AccountId { get; }

		public string UserId { get; }
	}

	public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, CommandResponse>
	{
		private readonly IUserRepository _userRepository;

		public CreateTransactionCommandHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<CommandResponse> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetUserByIdAsync(request.UserId);

			var account = user?.GetAccount(request.AccountId);
			if (account == null)
			{
				return null;
			}

			if (!account.HasAccess(request.UserId))
			{
				return new CommandResponse(new List<string> { "The user does not have access to update this account" });
			}

			var transaction = TransactionMapper.Map(request.Transaction);
			var result = account.AddTransactionByUserId(transaction, request.UserId);
			if (result.IsSuccessful)
			{
				await _userRepository.UpdateUserAsync(user);
				return new CommandResponse(true);
			}

			return new CommandResponse(new List<string> { "Failed to create new transaction" });
		}
	}
}
