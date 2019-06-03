using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace MyCompany.MyWebApplication.Models.Blocks
{
	[ContentType(GUID = "37d746a7-83ca-4c74-8e21-1c9ac19395cc")]
	public class EditorialBlock : SiteBlock, IEditorialContent
	{
		#region Properties

		[CultureSpecific]
		[Display(GroupName = SystemTabNames.Content, Order = 100)]
		public virtual XhtmlString MainBody { get; set; }

		#endregion
	}
}