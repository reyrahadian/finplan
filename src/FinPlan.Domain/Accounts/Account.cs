using Microsoft.AspNetCore.Identity;

namespace FinPlan.Domain.Accounts
{
	public class Account
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public AccountCategory Category { get; set; }
		public AccountType Type { get; set; }
		public IdentityUser Owner { get; set; }
	}
}