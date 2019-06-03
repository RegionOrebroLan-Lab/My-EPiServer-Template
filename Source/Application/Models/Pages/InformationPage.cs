using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;

namespace MyCompany.MyWebApplication.Models.Pages
{
	[ContentType(GUID = "3275f438-9336-4a97-aa71-cb88baf4d41c")]
	public class InformationPage : SitePage, IEditorialContent, IHeadlineContent, IMainAreaContent, IIntroductionContent, IRightAreaContent
	{
		#region Properties

		[CultureSpecific]
		[Display(GroupName = SystemTabNames.Content, Order = 300)]
		public virtual string Heading { get; set; }

		[CultureSpecific]
		[Display(GroupName = SystemTabNames.Content, Order = 400)]
		[UIHint(UIHint.Textarea)]
		public virtual string Introduction { get; set; }

		[Display(GroupName = SystemTabNames.Content, Order = 600)]
		public virtual ContentArea MainArea { get; set; }

		[CultureSpecific]
		[Display(GroupName = SystemTabNames.Content, Order = 500)]
		public virtual XhtmlString MainBody { get; set; }

		[Display(GroupName = SystemTabNames.Content, Order = 700)]
		public virtual ContentArea RightArea { get; set; }

		#endregion
	}
}