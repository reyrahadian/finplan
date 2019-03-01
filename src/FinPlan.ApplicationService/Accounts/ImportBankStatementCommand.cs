using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace FinPlan.ApplicationService.Accounts
{
	public class ImportBankStatementCommand : IRequest<CommandResponse>
	{
		public string FilePath { get; set; }
	}

	public class ImportBankStatementCommandHandler : IRequestHandler<ImportBankStatementCommand,CommandResponse>
	{
		public Task<CommandResponse> Handle(ImportBankStatementCommand request, CancellationToken cancellationToken)
		{
			throw new System.NotImplementedException();
		}
	}
}