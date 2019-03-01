using FinPlan.ApplicationService.Transactions;
using System.Collections.Generic;

namespace FinPlan.ApplicationService
{
	public interface IBankStatementCsvParser
	{
		IEnumerable<TransactionDto> Parse(string filePath);
	}
}
