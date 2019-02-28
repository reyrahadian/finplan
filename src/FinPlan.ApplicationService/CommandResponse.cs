namespace FinPlan.ApplicationService
{
	public class CommandResponse<T> : CommandResponse
	{
		public T Result { get; set; }
	}

	public class CommandResponse
	{
		public CommandResponse(bool isSuccessful = false)
		{
			IsSuccessful = isSuccessful;
		}

		public bool IsSuccessful { get; set; }
	}
}