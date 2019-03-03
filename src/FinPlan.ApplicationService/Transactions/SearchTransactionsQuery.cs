using FinPlan.ApplicationService.Transactions.Mappers;
using FinPlan.Domain.Transactions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinPlan.ApplicationService.Transactions
{
	public class SearchTransactionsQuery : IRequest<PaginatedResult<List<TransactionDto>>>
	{
		public SearchTransactionsQuery(int accountId, string searchKeyword, int pageIndex, int recordsPerPage)
		{
			AccountId = accountId;
			SearchKeyword = searchKeyword;
			PageIndex = pageIndex;
			RecordsPerPage = recordsPerPage;
		}

		public int AccountId { get; }
		public string SearchKeyword { get; }
		public int PageIndex { get; }
		public int RecordsPerPage { get; }
	}

	public class
		SearchTransactionsQueryHandler : IRequestHandler<SearchTransactionsQuery, PaginatedResult<List<TransactionDto>>>
	{
		private readonly ITransactionRepository _transactionRepository;

		public SearchTransactionsQueryHandler(ITransactionRepository transactionRepository)
		{
			_transactionRepository = transactionRepository;
		}

		public async Task<PaginatedResult<List<TransactionDto>>> Handle(SearchTransactionsQuery request,
			CancellationToken cancellationToken)
		{
			var result = await _transactionRepository.SearchTransactionsAsync(request.AccountId, request.SearchKeyword, request.PageIndex, request.RecordsPerPage);

			return new PaginatedResult<List<TransactionDto>>(result.Result.Select(TransactionDtoMapper.Map).ToList(),
				result.PageIndex, result.TotalPages, result.TotalRecords);
		}
	}
}