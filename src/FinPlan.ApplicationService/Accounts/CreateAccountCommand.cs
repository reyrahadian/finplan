using FinPlan.Domain;
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
		private readonly IUserRepository _userRepository;

		public CreateAccountCommandHandler(IAccountRepository accountRepository, IUserRepository userRepository)
		{
			_accountRepository = accountRepository;
			_userRepository = userRepository;
		}

		async Task<CommandResponse> IRequestHandler<CreateAccountCommand, CommandResponse>.Handle(CreateAccountCommand request, CancellationToken cancellationToken)
		{
			var account = new Account();
			account.Name = request.Account.Name;
			account.Currency = request.Account.Currency;
			account.Category = Enum.Parse<Domain.Accounts.AccountCategory>(request.Account.Category);
			account.Type = Enum.Parse<Domain.Accounts.AccountType>(request.Account.Type);
			account.Owner = await _userRepository.GetUserByIdAsync(request.Account.UserId);

			var isSuccessful = await _accountRepository.CreateAccountAsync(account);

			return new CommandResponse(isSuccessful);
		}
	}
}