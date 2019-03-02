using FinPlan.Domain.Accounts;
using FinPlan.Domain.Transactions;
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
		private readonly IAccountRepository _accountRepository;

		public ImportBankStatementCommandHandler(IBankStatementCsvParser bankStatementCsvParser, IAccountRepository accountRepository)
		{
			_bankStatementCsvParser = bankStatementCsvParser;
			_accountRepository = accountRepository;
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
			var account = await _accountRepository.GetAccountByIdAsync(request.AccountId);

			var result = account.AddTransactionsByUserId(transactions, request.UserId);
			if (result.IsSuccessful)
			{
				var dbUpdateResult = await _accountRepository.UpdateAccountAsync(account);
				return new CommandResponse(dbUpdateResult);
			}

			return new CommandResponse(result.ErrorMessages);
		}
	}
}