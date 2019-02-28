using Microsoft.AspNetCore.Identity;

namespace FinPlan.Domain.Accounts
{
	public class Account
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Currency { get; set; } = "AUD";
		public AccountCategory Category { get; set; } = AccountCategory.SpendingAndSaving;
		public AccountType Type { get; set; } = AccountType.Checking;
		public IdentityUser Owner { get; set; }
	}
}