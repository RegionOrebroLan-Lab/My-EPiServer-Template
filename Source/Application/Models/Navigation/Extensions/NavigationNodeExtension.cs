using System;
using System.Linq;

namespace MyCompany.MyWebApplication.Models.Navigation.Extensions
{
	public static class NavigationNodeExtension
	{
		#region Methods

		public static bool IsEmpty(this INavigationNode navigationNode)
		{
			if(navigationNode == null)
				throw new ArgumentNullException(nameof(navigationNode));

			return !navigationNode.Include && !navigationNode.Children.Any();
		}

		public static bool IsNullOrEmpty(INavigationNode navigationNode)
		{
			return navigationNode == null || navigationNode.IsEmpty();
		}

		#endregion
	}
}