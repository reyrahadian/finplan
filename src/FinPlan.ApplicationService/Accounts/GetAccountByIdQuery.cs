using FinPlan.Domain.Users;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class GetAccountByIdQuery : IRequest<AccountDto>
	{
		public GetAccountByIdQuery(int accountId, string userId)
		{
			AccountId = accountId;
			UserId = userId;
		}

		public int AccountId { get; }
		public string UserId { get; }
	}

	public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountDto>
	{
		private readonly IUserRepository _userRepository;

		public GetAccountByIdHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetUserByIdAsync(request.UserId);

			var account = user?.GetAccount(request.AccountId);
			if (account == null)
			{
				return null;
			}

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