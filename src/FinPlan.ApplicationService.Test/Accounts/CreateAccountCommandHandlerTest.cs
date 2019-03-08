using FinPlan.ApplicationService.Accounts;
using FinPlan.Domain;
using FluentAssertions;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using FinPlan.Domain.Users;
using Xunit;
using User = FinPlan.Domain.Users.User;

namespace FinPlan.ApplicationService.Test.Accounts
{
	public class CreateAccountCommandHandlerTest
	{
		[Fact]
		public async Task Fail_IfUserDoesNotExist()
		{
			var userRepository = new Mock<IUserRepository>();
			userRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).Returns(Task.FromResult<User>(null));
			var commandHandler = new CreateAccountCommandHandler(userRepository.Object);

			var result = await commandHandler.Handle(new CreateAccountCommand(new AccountDto(), "userid"), CancellationToken.None);
			result.IsSuccessful.Should().BeFalse();
		}

		[Fact]
		public async Task Succeed_IfUserExist()
		{
			var userRepository = new Mock<IUserRepository>();
			var user = new Mock<User>();
			userRepository.Setup(x => x.GetUserByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user.Object));
			var commandHandler = new CreateAccountCommandHandler(userRepository.Object);

			var result = await commandHandler.Handle(new CreateAccountCommand(new AccountDto(), "userid"), CancellationToken.None);
			result.IsSuccessful.Should().BeTrue();
		}
	}
}
