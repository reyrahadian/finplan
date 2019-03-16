using FinPlan.Domain.Users;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class DeleteAccountByIdCommand : IRequest<CommandResponse>
	{
		public DeleteAccountByIdCommand(int accountId, string userId)
		{
			AccountId = accountId;
			UserId = userId;
		}

		public int AccountId { get; }
		public string UserId { get; }
	}

	public class DeleteAccountByIdCommandHandler : IRequestHandler<DeleteAccountByIdCommand, CommandResponse>
	{
		private readonly IUserRepository _userRepository;

		public DeleteAccountByIdCommandHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		async Task<CommandResponse> IRequestHandler<DeleteAccountByIdCommand, CommandResponse>.Handle(DeleteAccountByIdCommand request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetUserByIdAsync(request.UserId);
			if (user == null)
			{
				return new CommandResponse($"User with id {request.UserId} does not exist");
			}

			if (user.RemoveAccount(request.AccountId))
			{
				var result = await _userRepository.UpdateUserAsync(user);
				if (!result)
				{
					return new CommandResponse($"Failed to delete account:{request.AccountId}");
				}

				return new CommandResponse(result);
			}
			else
			{
				return new CommandResponse($"Cannot remove account");
			}
		}
	}
}
