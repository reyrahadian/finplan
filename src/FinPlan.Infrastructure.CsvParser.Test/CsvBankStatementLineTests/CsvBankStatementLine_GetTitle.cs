using FluentAssertions;
using Xunit;

namespace FinPlan.Infrastructure.CsvParser.Test.CsvBankStatementLineTests
{
	public class CsvBankStatementLine_GetTitle
	{
		[Fact]
		public void GetTitle_ShouldReturn_CorrectTitle()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "10 Jul UBER *MWAHU SYDNEY NS AUS"
			};

			var title = csvLine.GetTitle();
			title.Should().Be("UBER *MWAHU SYDNEY NS AUS");
		}

		[Fact]
		public void GetTitle_ShouldReturn_EmptyTitle()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "RAY WHITE 79731165 1,739.65 $ $3,364.40 CR"
			};

			var title = csvLine.GetTitle();
			title.Should().BeNullOrEmpty();
		}		
	}
}