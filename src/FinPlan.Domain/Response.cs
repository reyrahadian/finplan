using System.Collections.Generic;

namespace FinPlan.Domain
{
	public class Response
	{
		public Response(bool isSuccessful)
		{
			IsSuccessful = isSuccessful;
		}

		public Response(List<string> errorMessages)
		{
			IsSuccessful = false;
			ErrorMessages = errorMessages;
		}

		public List<string> ErrorMessages { get; protected set; }

		public bool IsSuccessful { get; protected set; }
	}

	public class Response<T> : Response
	{
		public Response(bool isSuccessful, T result) : base(isSuccessful)
		{
			Result = result;
		}

		public Response(List<string> errorMessages, T result) : base(errorMessages)
		{
			Result = result;
		}

		public T Result { get; }
	}
}