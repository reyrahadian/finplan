namespace FinPlan.Domain
{
	public class PaginatedResult<T>
	{
		public PaginatedResult(T result, int pageIndex, int totalPages, int totalRecords)
		{
			Result = result;
			PageIndex = pageIndex;
			TotalPages = totalPages;
			TotalRecords = totalRecords;
		}

		public T Result { get; }
		public int PageIndex { get; }
		public int TotalPages { get; }
		public int TotalRecords { get; }
	}
}
