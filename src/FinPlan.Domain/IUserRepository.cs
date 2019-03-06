using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FinPlan.Domain
{
	public interface IUserRepository
	{
		Task<IdentityUser> GetUserByIdAsync(string userId);
	}
}
