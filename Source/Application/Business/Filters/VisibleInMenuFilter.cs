using EPiServer.Core;

namespace MyCompany.MyWebApplication.Business.Filters
{
	public class VisibleInMenuFilter : ContentFilter
	{
		#region Methods

		public override bool ShouldFilter(IContent content)
		{
			return content is PageData pageData && !pageData.VisibleInMenu;
		}

		#endregion
	}
}