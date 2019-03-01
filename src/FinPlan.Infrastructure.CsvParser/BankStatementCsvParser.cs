using CsvHelper;
using FinPlan.ApplicationService;
using FinPlan.ApplicationService.Transactions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FinPlan.Infrastructure.CsvParser
{
	public class BankStatementCsvParser : IBankStatementCsvParser
	{
		public IEnumerable<TransactionDto> Parse(string filePath)
		{
			using (var reader = new StreamReader(filePath))
			{
				using (var csv = new CsvReader(reader))
				{
					csv.Configuration.HasHeaderRecord = false;
					var records = csv.GetRecords<CsvBankStatementLine>().ToList();
					var totalRecords = records.Count();
					var transactions = new List<FinPlan.ApplicationService.Transactions.TransactionDto>();
					for (var i = 0; i <= totalRecords - 1; i++)
					{
						var record = records[i];
						if(!record.IsFirstStatementLine())
							continue;

						if (record.HasMoreThanOneStatementLine() && record.IsFirstStatementLine())
						{
							// a transaction info can be scattered over multiple lines, but not over 3 lines
							var transaction = new TransactionDto
							{
								Date = record.GetDate(),
								Title = record.GetTitle(),
								Type = record.GetTransactionType().GetValueOrDefault()
							};


							var secondRecord = records.ElementAt(i + 1);
							transaction.Amount = secondRecord.GetAmount().GetValueOrDefault();							
							transaction.Note = secondRecord.GetNote();

							var thirdRecord = records.ElementAtOrDefault(i + 2);
							if (thirdRecord != null)
							{
								if (!thirdRecord.IsFirstStatementLine())
								{
									transaction.Note += "<br/>" + thirdRecord.GetNote();
									if (transaction.Amount == 0)
									{
										transaction.Amount = thirdRecord.GetAmount().GetValueOrDefault();
									}
								}
							}

							transactions.Add(transaction);
						}
						else if (!record.HasMoreThanOneStatementLine() && record.IsFirstStatementLine())
						{
							transactions.Add(new TransactionDto()
							{
								Date = record.GetDate(),
								Title = record.GetTitle(),
								Amount = record.GetAmount().GetValueOrDefault(),
								Type = record.GetTransactionType().GetValueOrDefault()
							});
						}
						else
						{
							throw new Exception("Unrecognized csv bank statement line");
						}
					}

					return transactions;
				}
			}
		}
	}
}
