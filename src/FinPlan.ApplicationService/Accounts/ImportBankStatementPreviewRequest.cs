using FinPlan.ApplicationService.Transactions;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Accounts
{
	public class ImportBankStatementPreviewRequest : IRequest<IEnumerable<TransactionDto>>
	{
		public string FilePath { get; set; }
	}

	public class ImportBankStatementPreviewHandler : IRequestHandler<ImportBankStatementPreviewRequest, IEnumerable<TransactionDto>>
	{
		private readonly IBankStatementCsvParser _bankStatementCsvParser;

		public ImportBankStatementPreviewHandler(IBankStatementCsvParser bankStatementCsvParser)
		{
			_bankStatementCsvParser = bankStatementCsvParser;
		}

		public Task<IEnumerable<TransactionDto>> Handle(ImportBankStatementPreviewRequest request, CancellationToken cancellationToken)
		{
			return Task.FromResult(_bankStatementCsvParser.Parse(request.FilePath));
		}
	}
}