using FinPlan.ApplicationService.Transactions;
using FluentAssertions;
using System;
using Xunit;

namespace FinPlan.Infrastructure.CsvParser.Test.CsvBankStatementLineTests
{
	public class CsvBankStatementLine_GetAmount
	{
		[Fact]
		public void GetAmount_ShouldReturn_CorrectAmount()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "RAY WHITE 79731165 1,739.65 $ $3,364.40 CR"
			};

			var amount = csvLine.GetAmount();
			amount.Should().Be(1739.65m);
			var transactionType = csvLine.GetTransactionType();
			transactionType.Should().Be(TransactionType.Expense);
		}


		[Fact]
		public void GetAmount_ShouldReturn_CorrectCreditAmount()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "Direct Credit RAY WHITE 79731165 1,739.65 $ $3,364.40 CR"
			};

			var amount = csvLine.GetAmount();
			amount.Should().Be(1739.65m);
			var transactionType = csvLine.GetTransactionType();
			transactionType.Should().Be(TransactionType.Income);
		}

		[Fact]
		public void GetAmount_ShouldReturn_EmptyAmount()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "10 Jul UBER *MWAHU SYDNEY NS AUS"
			};

			var amount = csvLine.GetAmount();
			amount.Should().BeNull();
		}

		[Fact]
		public void GetTransactionType_ShouldReturn_CorrectTransactionType()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "02 Jul Direct Debit 323133 IPAYRENT"
			};

			csvLine.GetTransactionType().Should().BeNull();
		}

		[Fact]
		public void Parse_ShouldReturn_CorrectResult()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "09 Oct DR TURY NION PARKVILLE VICAU 200.00 ( $2,694.53 CR"
			};

			csvLine.HasMoreThanOneStatementLine().Should().BeFalse();
			csvLine.GetDate().Should().Be(new DateTime(DateTime.Now.Year, 10, 9));
			csvLine.GetTitle().Should().Be("DR TURY NION PARKVILLE VICAU");
			csvLine.GetAmount().Should().Be(200m);
			csvLine.GetTransactionType().Should().Be(TransactionType.Expense);
		}
	}
}