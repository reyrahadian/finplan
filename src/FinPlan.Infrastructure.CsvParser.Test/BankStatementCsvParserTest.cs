using FinPlan.ApplicationService.Transactions;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FinPlan.Infrastructure.CsvParser.Test
{
	public class BankStatementCsvParserTest
	{
		[Fact]
		public void Parse_Should_Return_Correct_Result()
		{
			var csvParser = new BankStatementCsvParser();
			var transactions = csvParser.Parse("statement-commbank-1.csv", 2018);

			transactions.ToList().SequenceEqual(GetCSvParserExpectedResult(), new TransactionDtoComparer()).Should().BeTrue();
		}

		public class TransactionDtoComparer : IEqualityComparer<TransactionDto>
		{
			public bool Equals(TransactionDto x, TransactionDto y)
			{
				return x.Date == y.Date && x.Title == y.Title && x.Note == y.Note && x.Amount == y.Amount && x.Type
					   == y.Type;
			}

			public int GetHashCode(TransactionDto obj)
			{
				throw new NotImplementedException();
			}
		}

		private static List<TransactionDto> GetCSvParserExpectedResult()
		{
			var expectedResult = new List<TransactionDto>();
			expectedResult.Add(new TransactionDto
			{
				Date = new DateTime(2018, 7, 2),
				Title = "Direct Debit 323133 IPAYRENT",
				Note = "RAY WHITE 79731165",
				Amount = 1739.65m,
				Type = TransactionType.Expense
			});
			expectedResult.Add(new TransactionDto
			{
				Date = new DateTime(2018, 7, 3),
				Title = "A & R BOULOS PTY LTQPS MOUNT WAVERLE AU",
				Note = "Card xx2311<br/>Value Date: 02/07/2018",
				Amount = 3.70m,
				Type = TransactionType.Expense
			});
			expectedResult.Add(new TransactionDto
			{
				Date = new DateTime(2018, 7, 3),
				Title = "WOOLWORTHS 3138 ST KIL ST KILDA AUS",
				Note = "Card xx2311<br/>Value Date: 30/06/2018",
				Amount = 60.31m,
				Type = TransactionType.Expense
			});
			expectedResult.Add(new TransactionDto
			{
				Date = new DateTime(2018, 7, 3),
				Title = "PUBLIC TRANSPORT VICT DOCKLANDS AUS",
				Note = "Card xx2311<br/>Value Date: 28/06/2018",
				Amount = 50.00m,
				Type = TransactionType.Expense
			});
			expectedResult.Add(new TransactionDto
			{
				Date = new DateTime(2018, 10, 9),
				Title = "DR TURY NION PARKVILLE VICAU",
				Amount = 200m,
				Type = TransactionType.Expense
			});
			expectedResult.Add(new TransactionDto
			{
				Date = new DateTime(2018, 9, 4),
				Title = "Transfer from MEDINA LEA CommBank app",
				Note = "mam",
				Amount = 15m,
				Type = TransactionType.Income
			});
			return expectedResult;
		}
	}
}