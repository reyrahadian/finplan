using FinPlan.Domain.Accounts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class CreateAccountCommand : IRequest<CommandResponse>
	{
		public CreateAccountCommand(AccountDto account)
		{
			Account = account;
		}

		public AccountDto Account { get; }
	}

	public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CommandResponse>
	{
		private readonly IAccountRepository _accountRepository;

		public CreateAccountCommandHandler(IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		async Task<CommandResponse> IRequestHandler<CreateAccountCommand, CommandResponse>.Handle(CreateAccountCommand request, CancellationToken cancellationToken)
		{
			var account = new Account();
			account.Name = request.Account.Name;
			account.Currency = request.Account.Currency;
			account.Category = Enum.Parse<Domain.Accounts.AccountCategory>(request.Account.Category);
			account.Type = Enum.Parse<Domain.Accounts.AccountType>(request.Account.Type);

			var isSuccessful = await _accountRepository.CreateAccountAsync(account);

			return new CommandResponse(isSuccessful);
		}
	}
}