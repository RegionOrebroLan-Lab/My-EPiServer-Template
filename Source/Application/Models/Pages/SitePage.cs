using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;

namespace MyCompany.MyWebApplication.Models.Pages
{
	public abstract class SitePage : PageData
	{
		#region Properties

		[Display(GroupName = SystemTabNames.Content, Order = 100)]
		[CultureSpecific]
		[UIHint(UIHint.Textarea)]
		public virtual string Description { get; set; }

		[Display(GroupName = SystemTabNames.Content, Order = 200)]
		[CultureSpecific]
		public virtual IEnumerable<string> Keywords { get; set; }

		#endregion
	}
}