using FinPlan.Domain.Accounts;
using FinPlan.Domain.Transactions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinPlan.Infrastructure.EntityFramework
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Account> Accounts { get; set; }
		public DbSet<TransactionCategory> Categories { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
	}
}