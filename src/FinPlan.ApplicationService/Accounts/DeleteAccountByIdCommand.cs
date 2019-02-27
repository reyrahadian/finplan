using FinPlan.Domain.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
    public class DeleteAccountByIdCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteAccountByIdCommandHandler : IRequestHandler<DeleteAccountByIdCommand>
    {
        private readonly IAccountRepository _accountRepository;

        public DeleteAccountByIdCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Unit> Handle(DeleteAccountByIdCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccountByIdAsync(request.Id);
            if (account == null)
            {
                //do something
            }

            await _accountRepository.DeleteByIdAsync(request.Id);

            return Unit.Value;
        }
    }
}
