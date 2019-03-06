using FinPlan.ApplicationService;
using FinPlan.Domain;
using FinPlan.Domain.Accounts;
using FinPlan.Domain.Transactions;
using FinPlan.Infrastructure.CsvParser;
using FinPlan.Infrastructure.EntityFramework;
using FinPlan.Infrastructure.EntityFramework.Accounts;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FinPlan.Infrastructure.Bootstrapper
{
	public class Bootstrapper
	{
		public static void InitializeServices(IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options
					.UseLazyLoadingProxies()
					.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
			services.AddDefaultIdentity<IdentityUser>()
				//.AddDefaultUI(UIFramework.Bootstrap4)
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddMediatR(typeof(FinPlan.ApplicationService.Accounts.AccountCategory).Assembly);

			services.AddScoped<IAccountRepository, AccountRepository>();
			services.AddScoped<IBankStatementCsvParser, BankStatementCsvParser>();
			services.AddScoped<ITransactionRepository, TransactionRepository>();
			services.AddScoped<IAccountBalanceInfoCalculator, AccountBalanceInfoCalculator>();
			services.AddScoped<IUserRepository, UserRepository>();
		}

		public static void InitializeDb(IServiceProvider serviceProvider)
		{
			var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
			DbInitializer.Initialize(context, serviceProvider.GetRequiredService<UserManager<IdentityUser>>());
		}
	}
}
