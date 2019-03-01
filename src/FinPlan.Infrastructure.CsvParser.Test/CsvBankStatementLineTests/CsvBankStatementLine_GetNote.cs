using FluentAssertions;
using Xunit;

namespace FinPlan.Infrastructure.CsvParser.Test.CsvBankStatementLineTests
{
	public class CsvBankStatementLine_GetNote
	{
		[Fact]
		public void GetNote_ShouldReturn_CorrectNote()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "Card xx2311"
			};

			var note = csvLine.GetNote();
			note.Should().Be("Card xx2311");
		}

		[Fact]
		public void GetNote_ShouldReturn_EmptyNote()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "CardRAY WHITE 79731165 1,739.65 $ $3,364.40 CR"
			};

			var note = csvLine.GetNote();
			note.Should().Be("CardRAY WHITE 79731165");
		}

		[Fact]
		public void GetNote_ShouldReturn_EmptyNote2()
		{
			var csvLine = new CsvBankStatementLine
			{
				Text = "10 Jul UBER *MWAHU SYDNEY NS AUS"
			};

			var note = csvLine.GetNote();
			note.Should().BeNullOrWhiteSpace();
		}		
	}
}