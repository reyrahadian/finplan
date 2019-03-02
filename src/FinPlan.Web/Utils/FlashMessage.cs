using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace FinPlan.Web.Utils
{
	public class FlashMessage
	{
		private const string MessageKey = "message";
		private const string ErrorKey = "error";

		public static void SetMessage(ITempDataDictionary tempData, string message, bool isError = false)
		{
			tempData[MessageKey] = message;
			if (isError)
			{
				tempData[ErrorKey] = true;
			}
		}

		public static string GetMessage(ITempDataDictionary tempData)
		{
			return (string)tempData[MessageKey];
		}

		public static bool IsError(ITempDataDictionary tempData)
		{
			return tempData[ErrorKey] != null;
		}
	}
}
