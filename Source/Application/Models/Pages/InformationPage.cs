using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace MyCompany.MyWebApplication.Models.Pages
{
	[ContentType(GUID = "3275f438-9336-4a97-aa71-cb88baf4d41c")]
	public class InformationPage : SitePage
	{
		#region Properties

		[Display(GroupName = SystemTabNames.Content, Order = 300)]
		[CultureSpecific]
		public virtual string Heading { get; set; }

		[Display(GroupName = SystemTabNames.Content, Order = 400)]
		[CultureSpecific]
		public virtual XhtmlString MainBody { get; set; }

		#endregion
	}
}