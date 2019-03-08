using System.Threading.Tasks;
using JetBrains.Annotations;

namespace FinPlan.Domain.Users
{
	public interface IUserRepository
	{
		[CanBeNull]
		Task<User> GetUserByIdAsync(string userId);
		Task<bool> UpdateUserAsync(User user);
	}
}
