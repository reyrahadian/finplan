using FinPlan.Domain.Accounts;
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
		private readonly IAccountRepository _accountRepository;
		private readonly IAccountBalanceInfoCalculator _accountBalanceInfoCalculator;

		public GetAccountBalanceInfoQueryHandler(IAccountRepository accountRepository, IAccountBalanceInfoCalculator accountBalanceInfoCalculator)
		{
			_accountRepository = accountRepository;
			_accountBalanceInfoCalculator = accountBalanceInfoCalculator;
		}

		public async Task<AccountBalanceInfoDto> Handle(GetAccountBalanceInfoQuery request, CancellationToken cancellationToken)
		{
			var account = await _accountRepository.GetAccountByIdAsync(request.AccountId);
			if (account == null)
			{
				return null;
			}

			if (!account.HasAccess(request.UserId))
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
