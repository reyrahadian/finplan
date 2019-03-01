using FinPlan.ApplicationService.Transactions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace FinPlan.Infrastructure.CsvParser
{
	internal class CsvBankStatementLine
	{
		public string Text { get; set; }

		public DateTime? GetDate()
		{
			if (string.IsNullOrWhiteSpace(Text))
			{
				return null;
			}

			var words = string.Join(" ", Text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2));
			if (DateTime.TryParseExact(words, "dd MMM", CultureInfo.InvariantCulture, DateTimeStyles.None,
				out var output))
			{
				return output;
			}

			return null;
		}

		public decimal? GetAmount()
		{
			if (string.IsNullOrWhiteSpace(Text))
			{
				return null;
			}

			decimal? amount = Text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Where(
				IsTransactionAmountText).Select(x =>
			{
				decimal? result = null;
				if (decimal.TryParse(x, out var output))
				{
					result = output;
				}

				return result;
			}).FirstOrDefault();

			return amount;
		}

		private static bool IsTransactionAmountText(string x)
		{
			var centPosition = x.IndexOf('.');
			var isCorrectCentPosition = centPosition == x.Length - 3 && centPosition > 0;
			if (isCorrectCentPosition && decimal.TryParse(x, out var output))
			{
				return true;
			}

			return false;
		}

		public string GetTitle()
		{
			if (string.IsNullOrWhiteSpace(Text))
			{
				return null;
			}

			if (GetDate().HasValue)
			{
				//we're on the correct statement line
				var words = Text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(2);
				if (GetAmount().HasValue)
				{
					//transaction amount is on the same line
					var result = new StringBuilder();
					foreach (var word in words)
					{
						if (IsTransactionAmountText(word))
						{
							break;
						}

						result.Append(word + " ");
					}

					return result.ToString().TrimEnd(' ');
				}

				return string.Join(" ", words);
			}

			return null;
		}

		public bool IsFirstStatementLine()
		{
			return GetDate().HasValue;
		}

		public bool HasMoreThanOneStatementLine()
		{
			return GetDate().HasValue && !string.IsNullOrWhiteSpace(GetTitle()) && !GetAmount().HasValue;
		}

		public string GetNote()
		{
			if (string.IsNullOrWhiteSpace(Text))
			{
				return null;
			}

			if (string.IsNullOrWhiteSpace(GetTitle()))
			{
				if (GetAmount().HasValue)
				{
					var words = Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					var result = new StringBuilder();
					foreach (var word in words)
					{
						if (IsTransactionAmountText(word))
						{
							break;							
						}

						result.Append(word + " ");
					}

					return result.ToString().TrimEnd(' ');
				}

				return Text;
			}

			return null;
		}

		public TransactionType? GetTransactionType()
		{
			if (!GetAmount().HasValue)
			{
				return null;
			}

			if (IsCreditTransaction())
			{
				return TransactionType.Income;
			}

			return TransactionType.Expense;
		}

		private bool IsCreditTransaction()
		{
			var creditRelatedKeywords = new List<string>
			{
				"transfer from",
				"salary",
				"hif claim",
				"direct credit"
			};

			return creditRelatedKeywords.Any(x => Text.Contains(x, StringComparison.OrdinalIgnoreCase));
		}
	}
}