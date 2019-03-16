using FinPlan.Domain.Users;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class GetAccountsRequestQuery : IRequest<List<AccountDto>>
	{
		public string UserId { get; }

		public GetAccountsRequestQuery(string userId)
		{
			UserId = userId;
		}
	}

	public class GetAccountsHandler : IRequestHandler<GetAccountsRequestQuery, List<AccountDto>>
	{
		private readonly IUserRepository _userRepository;

		public GetAccountsHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<List<AccountDto>> Handle(GetAccountsRequestQuery request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetUserByIdAsync(request.UserId);
			if (user == null)
			{
				return new List<AccountDto>();
			}

			return user.GetAccounts()?.Select(x => new AccountDto
			{
				Id = x.Id,
				Category = x.Category.ToString(),
				Name = x.Name,
				Type = x.Type.ToString(),
				UserId = x.Owner?.ToString()
			}).ToList();
		}
	}
}