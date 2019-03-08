using System;
using System.Threading.Tasks;
using FinPlan.ApplicationService.Accounts;
using FluentAssertions;
using Xunit;

namespace FinPlan.ApplicationService.Test.Accounts
{
	public class CreateAccountCommandTest
	{
		[Fact]
		public async Task ThrowException_IfAccountIsNull()
		{

			Action action = () => new CreateAccountCommand(null, "test");
			action.Should().Throw<ArgumentNullException>();
		}

		[Fact]
		public async Task ThrowException_IfUserIdIsNull()
		{
			Action action = () => new CreateAccountCommand(new AccountDto(), string.Empty);
			action.Should().Throw<ArgumentException>();
		}
	}
}