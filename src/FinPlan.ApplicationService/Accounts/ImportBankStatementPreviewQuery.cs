using FinPlan.ApplicationService.Transactions;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class ImportBankStatementPreviewQuery : IRequest<IEnumerable<TransactionDto>>
	{
		public string FilePath { get; set; }
		public int Year { get; set; }
	}

	public class ImportBankStatementPreviewHandler : IRequestHandler<ImportBankStatementPreviewQuery, IEnumerable<TransactionDto>>
	{
		private readonly IBankStatementCsvParser _bankStatementCsvParser;

		public ImportBankStatementPreviewHandler(IBankStatementCsvParser bankStatementCsvParser)
		{
			_bankStatementCsvParser = bankStatementCsvParser;
		}

		public Task<IEnumerable<TransactionDto>> Handle(ImportBankStatementPreviewQuery request, CancellationToken cancellationToken)
		{
			return Task.FromResult(_bankStatementCsvParser.Parse(request.FilePath, request.Year));
		}
	}
}