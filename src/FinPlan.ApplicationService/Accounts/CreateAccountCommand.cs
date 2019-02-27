using System;
using System.Threading;
using System.Threading.Tasks;
using FinPlan.Domain.Accounts;
using MediatR;

namespace FinPlan.ApplicationService.Accounts
{
    public class CreateAccountCommand : IRequest
    {
        public AccountDto Account { get; set; }
    }

    public class CreateAccountCommandHandler: IRequestHandler<CreateAccountCommand>
    {
        private readonly IAccountRepository _accountRepository;

        public CreateAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = new Account();
            account.Name = request.Account.Name;
            account.Category = Enum.Parse<Domain.Accounts.AccountCategory>(request.Account.Category);
            account.Type = Enum.Parse<Domain.Accounts.AccountType>(request.Account.Type);

            await _accountRepository.CreateAccountAsync(account);

            return Unit.Value;
        }
    }
}