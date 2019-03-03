namespace FinPlan.ApplicationService
{
	public class PaginatedResult<T> where T : new()
	{
		public PaginatedResult(T result, int pageIndex, int totalPages, int totalRecords)
		{
			Result = result;
			PageIndex = pageIndex;
			TotalPages = totalPages;
			TotalRecords = totalRecords;
		}

		public T Result { get; } = new T();
		public int PageIndex { get; }
		public int TotalPages { get; }
		public int TotalRecords { get; }

		public bool HasNextPage()
		{
			return PageIndex < TotalPages - 1;//page 1 starts from index 0
		}

		public bool HasPreviousPage()
		{
			return PageIndex > 0;
		}
	}
}
