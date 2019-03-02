using System.Threading;
using System.Threading.Tasks;
using FinPlan.Domain.Accounts;
using MediatR;

namespace FinPlan.ApplicationService.Accounts
{
    public class GetAccountByIdQuery : IRequest<AccountDto>
    {
        public int Id { get; set; }
    }

    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
    {
        private readonly IAccountRepository _accountRepository;

        public GetAccountByIdHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccountByIdAsync(request.Id);

            return new AccountDto
            {
                Id = account.Id,
                Name = account.Name,
                Category = account.Category.ToString(),
                Type = account.Type.ToString()
            };
        }
    }
}