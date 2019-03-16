using FinPlan.Domain.Accounts;
using FinPlan.Domain.Users;
using JetBrains.Annotations;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class CreateAccountCommand : IRequest<CommandResponse>
	{
		public CreateAccountCommand([NotNull] AccountDto account, [NotNull] string userId)
		{
			if (account == null)
			{
				throw new ArgumentNullException(nameof(account));
			}

			if (string.IsNullOrWhiteSpace(userId))
			{
				throw new ArgumentException(nameof(userId));
			}

			Account = account;
			UserId = userId;
		}

		public AccountDto Account { get; }

		public string UserId { get; }
	}

	public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CommandResponse>
	{
		private readonly IUserRepository _userRepository;

		public CreateAccountCommandHandler([NotNull]IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<CommandResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
		{
			Enum.TryParse<Domain.Accounts.AccountCategory>(request.Account.Category, out var category);
			Enum.TryParse<Domain.Accounts.AccountType>(request.Account.Type, out var type);
			var account = new Account
			{
				Name = request.Account.Name,
				Currency = request.Account.Currency,
				Category = category,
				Type = type
			};

			var user = await _userRepository.GetUserByIdAsync(request.UserId);
			if (user == null)
			{
				return new CommandResponse("User doesn't exist");
			}

			user.AddAccount(account);
			var isSuccessful = await _userRepository.UpdateUserAsync(user);

			return new CommandResponse(isSuccessful);
		}
	}
}