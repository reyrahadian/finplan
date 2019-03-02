using FinPlan.Domain.Transactions;
using System;

namespace FinPlan.ApplicationService.Transactions.Mappers
{
	public class TransactionMapper
	{
		public static Transaction Map(TransactionDto transaction)
		{
			return new Transaction
			{
				Id = transaction.Id,
				Title = transaction.Title,
				Note = transaction.Note,
				Date = transaction.Date,
				Amount = transaction.Amount,
				//TransactionCategory = Enum.Parse<Transacti>()
				Type = Enum.Parse<Domain.Transactions.TransactionType>(transaction.Type.ToString())
			};
		}
	}
}
