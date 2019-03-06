using FinPlan.Domain;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FinPlan.Infrastructure.EntityFramework
{
	public class UserRepository : IUserRepository
	{
		private readonly UserManager<IdentityUser> _userManager;

		public UserRepository(UserManager<IdentityUser> _userManager)
		{
			this._userManager = _userManager;
		}

		public async Task<IdentityUser> GetUserByIdAsync(string userId)
		{
			return await _userManager.FindByIdAsync(userId);
		}
	}
}
