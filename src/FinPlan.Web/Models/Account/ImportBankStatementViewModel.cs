using FinPlan.ApplicationService.Transactions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinPlan.Web.Models.Account
{
	public class ImportBankStatementViewModel
	{
		[Required] public int AccountId { get; set; }

		public IFormFile File { get; set; }

		public bool HasConfirmedToImport { get; set; }
		public IEnumerable<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
		public string UploadedFilePath { get; set; }

		[Required]
		public int Year { get; set; }
	}
}