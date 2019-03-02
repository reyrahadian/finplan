using FinPlan.Domain.Accounts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class DeleteAccountByIdCommand : IRequest<CommandResponse>
	{
		public DeleteAccountByIdCommand(int id)
		{
			Id = id;
		}

		public int Id { get; }
	}

	public class DeleteAccountByIdCommandHandler : IRequestHandler<DeleteAccountByIdCommand, CommandResponse>
	{
		private readonly IAccountRepository _accountRepository;

		public DeleteAccountByIdCommandHandler(IAccountRepository accountRepository)
		{
			_accountRepository = accountRepository;
		}

		async Task<CommandResponse> IRequestHandler<DeleteAccountByIdCommand, CommandResponse>.Handle(DeleteAccountByIdCommand request, CancellationToken cancellationToken)
		{
			var account = await _accountRepository.GetAccountByIdAsync(request.Id);
			if (account == null)
			{
				//do something
			}

			var isSuccessful = await _accountRepository.DeleteByIdAsync(request.Id);

			return new CommandResponse(isSuccessful);
		}
	}
}
