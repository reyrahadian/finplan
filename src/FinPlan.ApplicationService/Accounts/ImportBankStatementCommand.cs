using FinPlan.Domain.Transactions;
using FinPlan.Domain.Users;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class ImportBankStatementCommand : IRequest<CommandResponse>
	{
		public string FilePath { get; set; }
		public int Year { get; set; }
		public int AccountId { get; set; }
		public string UserId { get; set; }
	}

	public class ImportBankStatementCommandHandler : IRequestHandler<ImportBankStatementCommand, CommandResponse>
	{
		private readonly IBankStatementCsvParser _bankStatementCsvParser;
		private readonly IUserRepository _userRepository;

		public ImportBankStatementCommandHandler(IBankStatementCsvParser bankStatementCsvParser, IUserRepository userRepository)
		{
			_bankStatementCsvParser = bankStatementCsvParser;
			_userRepository = userRepository;
		}

		public async Task<CommandResponse> Handle(ImportBankStatementCommand request, CancellationToken cancellationToken)
		{
			var transactionsFromCsvFile = _bankStatementCsvParser.Parse(request.FilePath, request.Year);
			var transactions = transactionsFromCsvFile.Select(x => new Transaction
			{
				Date = x.Date,
				Title = x.Title,
				Note = x.Note,
				Amount = x.Amount,
				Type = Enum.Parse<TransactionType>(x.Type.ToString())
			});
			var user = await _userRepository.GetUserByIdAsync(request.UserId);

			var account = user?.GetAccount(request.AccountId);
			if (account == null)
			{
				return null;
			}

			var result = account.AddTransactionsByUserId(transactions, request.UserId);
			if (result.IsSuccessful)
			{
				var dbUpdateResult = await _userRepository.UpdateUserAsync(user);
				return new CommandResponse(dbUpdateResult);
			}

			return new CommandResponse(result.ErrorMessages);
		}
	}
}