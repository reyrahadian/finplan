using FinPlan.Domain.Accounts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class UpdateAccountCommand : IRequest<CommandResponse>
	{
		public UpdateAccountCommand(AccountDto account)
		{
			Account = account;
		}

		public AccountDto Account { get; }
	}

	public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, CommandResponse>
	{
		private readonly IAccountRepository _accountRepository;

		public UpdateAccountCommandHandler(IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		async Task<CommandResponse> IRequestHandler<UpdateAccountCommand, CommandResponse>.Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
		{
			var account = await _accountRepository.GetAccountByIdAsync(request.Account.Id);
			account.Name = request.Account.Name;
			account.Category = Enum.Parse<Domain.Accounts.AccountCategory>(request.Account.Category);
			account.Type = Enum.Parse<Domain.Accounts.AccountType>(request.Account.Type);

			var isSuccessful = await _accountRepository.UpdateAccountAsync(account);
			return new CommandResponse(isSuccessful);
		}
	}
}