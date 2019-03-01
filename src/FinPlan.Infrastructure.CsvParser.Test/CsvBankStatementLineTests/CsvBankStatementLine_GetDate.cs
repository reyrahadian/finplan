using System;
using FluentAssertions;
using Xunit;

namespace FinPlan.Infrastructure.CsvParser.Test.CsvBankStatementLineTests
{
	public class CsvBankStatementLine_GetDate
	{
		[Fact]
		public void GetDate_ShouldReturn_CorrectDate()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "10 Jul UBER *MWAHU SYDNEY NS AUS"
			};

			csvLine.HasMoreThanOneStatementLine().Should().BeTrue();
			var date = csvLine.GetDate();
			date.Should().Be(new DateTime(DateTime.Now.Year, 07, 10));
		}

		[Fact]
		public void GetDate_ShouldReturn_EmptyDate()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "RAY WHITE 79731165 1,739.65 $ $3,364.40 CR"
			};

			var date = csvLine.GetDate();
			date.Should().BeNull();
		}		
	}
}
