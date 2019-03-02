using FinPlan.ApplicationService.Accounts;
using FinPlan.Domain.Transactions;
using System;

namespace FinPlan.ApplicationService.Transactions.Mappers
{
	public class TransactionDtoMapper
	{
		public static TransactionDto Map(Transaction transaction)
		{
			return new TransactionDto
			{
				Id = transaction.Id,
				Date = transaction.Date,
				Amount = transaction.Amount,
				Note = transaction.Note,
				Title = transaction.Title,
				Category = transaction.TransactionCategory?.Name,
				Type = Enum.Parse<TransactionType>(transaction.Type.ToString()),
				Account = new AccountDto
				{
					Id = transaction.Account.Id
				}
			};
		}
	}
}
