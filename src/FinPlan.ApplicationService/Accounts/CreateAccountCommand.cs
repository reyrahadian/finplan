﻿using FinPlan.Domain.Accounts;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class CreateAccountCommand : IRequest<CommandResponse>
	{
		public AccountDto Account { get; set; }
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
			account.Category = Enum.Parse<Domain.Accounts.AccountCategory>(request.Account.Category);
			account.Type = Enum.Parse<Domain.Accounts.AccountType>(request.Account.Type);

			var isSuccessful = await _accountRepository.CreateAccountAsync(account);

			return new CommandResponse(isSuccessful);
		}
	}
}