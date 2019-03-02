using System.Collections.Generic;

namespace FinPlan.ApplicationService
{
	public class CommandResponse<T> : CommandResponse
	{
		public CommandResponse(bool isSuccessful, T result) : base(isSuccessful)
		{
			Result = result;
		}

		public CommandResponse(List<string> errorMessages, T result) : base(errorMessages)
		{
			Result = result;
		}

		public T Result { get; }
	}

	public class CommandResponse
	{
		public CommandResponse(bool isSuccessful)
		{
			IsSuccessful = isSuccessful;
		}

		public CommandResponse(List<string> errorMessages)
		{
			IsSuccessful = false;
			ErrorMessages = errorMessages;
		}

		public bool IsSuccessful { get; protected set; }
		public List<string> ErrorMessages { get; protected set; }
	}
}