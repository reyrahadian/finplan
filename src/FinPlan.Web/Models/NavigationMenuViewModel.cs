using System.Collections.Generic;

namespace FinPlan.Web.Models
{
	public class NavigationMenuViewModel
	{
		public string Title { get; set; }
		public string Uri { get; set; }
		public IEnumerable<NavigationMenuViewModel> Children { get; set; } = new List<NavigationMenuViewModel>();
	}
}