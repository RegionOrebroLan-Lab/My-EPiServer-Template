using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace MyCompany.MyWebApplication.Models.Pages
{
	[ContentType(GUID = "1bdbd340-8635-4c38-9fba-6a8f93717578")]
	public class StartPage : SitePage, IEditorialContent, IMainAreaContent, IRightAreaContent
	{
		#region Properties

		[Display(GroupName = SystemTabNames.Content, Order = 400)]
		public virtual ContentArea MainArea { get; set; }

		[CultureSpecific]
		[Display(GroupName = SystemTabNames.Content, Order = 300)]
		public virtual XhtmlString MainBody { get; set; }

		[Display(GroupName = SystemTabNames.Content, Order = 500)]
		public virtual ContentArea RightArea { get; set; }

		#endregion
	}
}