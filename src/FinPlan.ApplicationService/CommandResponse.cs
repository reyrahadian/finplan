using System.Collections.Generic;

namespace FinPlan.ApplicationService
{
	public class CommandResponse<T> : CommandResponse
	{
		public CommandResponse(bool isSuccessful, T result) : base(isSuccessful)
		{
			Result = result;
		}

		public CommandResponse(string errorMessage, T result) : this(new List<string> { errorMessage }, result)
		{
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

		public CommandResponse(string errorMessage) : this(new List<string> { errorMessage })
		{
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