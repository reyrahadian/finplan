using FinPlan.ApplicationService.Transactions.Mappers;
using FinPlan.Domain.Accounts;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
		private readonly IAccountRepository _accountRepository;

		public CreateTransactionCommandHandler(IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		public async Task<CommandResponse> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
		{
			var account = await _accountRepository.GetAccountByIdAsync(request.AccountId);

			if (!account.HasAccess(request.UserId))
			{
				return new CommandResponse(new List<string> { "The user does not have access to update this account" });
			}

			var transaction = TransactionMapper.Map(request.Transaction);
			var result = account.AddTransactionByUserId(transaction, request.UserId);
			if (result.IsSuccessful)
			{
				await _accountRepository.UpdateAccountAsync(account);
				return new CommandResponse(true);
			}

			return new CommandResponse(new List<string> { "Failed to create new transaction" });
		}
	}
}
