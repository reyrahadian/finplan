using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FinPlan.ApplicationService.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinPlan.Web.Models.Account
{
    public class AccountFormViewModel
    {
        [HiddenInput] public int? Id { get; set; }

        [Required] public string Name { get; set; }

        [Required] public AccountCategory Category { get; set; }

        public List<SelectListItem> Categories { get; } = new List<SelectListItem>
        {
            new SelectListItem("Spending & Savings", AccountCategory.SpendingAndSaving.ToString()),
            new SelectListItem("Loan", AccountCategory.Loan.ToString())
        };

        [Required] public AccountType Type { get; set; }

        public List<SelectListItem> Types { get; } = new List<SelectListItem>
        {
            new SelectListItem("Cash", AccountType.Cash.ToString()),
            new SelectListItem("CreditCard", AccountType.CreditCard.ToString()),
            new SelectListItem("Checking", AccountType.Checking.ToString()),
            new SelectListItem("Online & Digital Wallet", AccountType.OnlineAndDigitalWallet.ToString()),
            new SelectListItem("Prepaid", AccountType.Prepaid.ToString()),
            new SelectListItem("Gift Card", AccountType.GiftCard.ToString()),
            new SelectListItem("Personal Loan", AccountType.PersonalLoan.ToString()),
            new SelectListItem("Student Loan", AccountType.StudentLoan.ToString()),
            new SelectListItem("Auto Loan", AccountType.AutoLoan.ToString()),
            new SelectListItem("Mortgage", AccountType.Mortgage.ToString()),
            new SelectListItem("Other Loan", AccountType.OtherLoan.ToString())
        };

		public string Currency { get; set; }
		public IEnumerable<SelectListItem> Currencies { get; set; } = new List<SelectListItem>();

        public void MapFrom(AccountDto account)
        {
            Id = account.Id;
            Name = account.Name;
            Category = Enum.Parse<AccountCategory>(account.Category, true);
            Type = Enum.Parse<AccountType>(account.Type, true);

        }
    }
}