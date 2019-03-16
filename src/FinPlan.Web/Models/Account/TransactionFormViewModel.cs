using FinPlan.ApplicationService.Transactions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FinPlan.ApplicationService.Accounts;

namespace FinPlan.Web.Models.Account
{
	public class TransactionFormViewModel
	{
		[Required]
		public int AccountId { get; set; }

		public int Id { get; set; }

		[Required] public string Title { get; set; }

		[Required] public decimal Amount { get; set; }

		public string Note { get; set; }

		[Required]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime Date { get; set; }

		public string Category { get; set; }

		[Required] public TransactionType Type { get; set; }

		public List<SelectListItem> Types { get; set; } = new List<SelectListItem>
		{
			new SelectListItem(TransactionType.Expense.ToString(), TransactionType.Expense.ToString()),
			new SelectListItem(TransactionType.Income.ToString(), TransactionType.Income.ToString())
		};

		public void MapFrom(TransactionDto transaction)
		{
			Id = transaction.Id;
			Date = transaction.Date;
			Title = transaction.Title;
			Amount = transaction.Amount;
			Note = transaction.Note;
			Category = transaction.Category;
			Type = transaction.Type;
		}

		public TransactionDto MapToDto()
		{
			return new TransactionDto
			{
				Id = Id,
				Date = Date,
				Title = Title,
				Amount = Amount,
				Note = Note,
				Category = Category,
				Type = Type,
				Account = new AccountDto
				{
					Id = AccountId
				}
			};
		}
	}
}