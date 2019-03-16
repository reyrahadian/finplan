using FinPlan.Domain.Accounts;
using FinPlan.Domain.Users;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class GetAccountBalanceInfoQuery : IRequest<AccountBalanceInfoDto>
	{
		public int AccountId { get; }
		public string UserId { get; }

		public GetAccountBalanceInfoQuery(int accountId, string userId)
		{
			AccountId = accountId;
			UserId = userId;
		}
	}

	public class GetAccountBalanceInfoQueryHandler : IRequestHandler<GetAccountBalanceInfoQuery, AccountBalanceInfoDto>
	{
		private readonly IUserRepository _userRepository;
		private readonly IAccountBalanceInfoCalculator _accountBalanceInfoCalculator;

		public GetAccountBalanceInfoQueryHandler(IUserRepository userRepository,
			IAccountBalanceInfoCalculator accountBalanceInfoCalculator)
		{
			_userRepository = userRepository;
			_accountBalanceInfoCalculator = accountBalanceInfoCalculator;
		}

		public async Task<AccountBalanceInfoDto> Handle(GetAccountBalanceInfoQuery request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetUserByIdAsync(request.UserId);

			var account = user?.GetAccount(request.AccountId);
			if (account == null)
			{
				return null;
			}

			return new AccountBalanceInfoDto
			{
				TotalDebit = await account.GetTotalDebit(_accountBalanceInfoCalculator),
				TotalCredit = await account.GetTotalCredit(_accountBalanceInfoCalculator)
			};
		}
	}

	public class AccountBalanceInfoDto
	{
		public decimal TotalDebit { get; set; }
		public decimal TotalCredit { get; set; }
	}
}
