using FinPlan.Domain;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using FinPlan.Domain.Users;

namespace FinPlan.Infrastructure.EntityFramework
{
	public class UserRepository : IUserRepository
	{
		private readonly UserManager<User> _userManager;

		public UserRepository(UserManager<User> _userManager)
		{
			this._userManager = _userManager;
		}

		public async Task<User> GetUserByIdAsync(string userId)
		{
			return (User)await _userManager.FindByIdAsync(userId);
		}

		public async Task<bool> UpdateUserAsync(User user)
		{
			var result = await _userManager.UpdateAsync(user);

			return result.Succeeded;
		}
	}
}
