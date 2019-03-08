using FinPlan.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinPlan.Domain.Users;

namespace FinPlan.ApplicationService.Accounts
{
	public class UpdateAccountCommand : IRequest<CommandResponse>
	{
		public UpdateAccountCommand(AccountDto account, string userId)
		{
			Account = account;
			UserId = userId;
		}

		public AccountDto Account { get; }
		public string UserId { get; }
	}

	public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, CommandResponse>
	{
		private readonly IUserRepository _userRepository;

		public UpdateAccountCommandHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		async Task<CommandResponse> IRequestHandler<UpdateAccountCommand, CommandResponse>.Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetUserByIdAsync(request.UserId);
			var account = user?.GetAccount(request.Account.Id);
			if (account == null)
			{
				return null;
			}
			account.Name = request.Account.Name;
			account.Category = Enum.Parse<Domain.Accounts.AccountCategory>(request.Account.Category);
			account.Type = Enum.Parse<Domain.Accounts.AccountType>(request.Account.Type);

			var isSuccessful = await _userRepository.UpdateUserAsync(user);
			return new CommandResponse(isSuccessful);
		}
	}
}