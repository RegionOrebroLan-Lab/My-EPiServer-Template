using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace Prototype.Models.Navigation.Extensions
{
	public static class NavigationNodeExtension
	{
		#region Methods

		public static IEnumerable<INavigationNode> MaximumItems(this INavigationNode navigationNode, int maximumNumberOfItems)
		{
			if(navigationNode == null)
				throw new ArgumentNullException(nameof(navigationNode));

			var skip = 0;

			var items = navigationNode.ToArray();

			var activeItem = items.FirstOrDefault(item => item.Active || item.ActiveAncestor);

			// ReSharper disable InvertIf
			if(activeItem != null)
			{
				var indexOfActive = items.IndexOf(activeItem);

				if(indexOfActive >= 0)
				{
					var indexOfLast = indexOfActive + 1;

					while(indexOfLast >= items.Length)
					{
						indexOfLast--;
					}

					if(indexOfLast >= maximumNumberOfItems)
						skip = indexOfLast - maximumNumberOfItems + 1;
				}
			}
			// ReSharper restore InvertIf

			return items.Skip(skip).Take(maximumNumberOfItems).ToArray();
		}

		#endregion
	}
}