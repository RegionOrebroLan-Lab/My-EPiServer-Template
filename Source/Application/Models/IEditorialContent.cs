using EPiServer.Core;

namespace MyCompany.MyWebApplication.Models
{
	public interface IEditorialContent
	{
		#region Properties

		XhtmlString MainBody { get; set; }

		#endregion
	}
}