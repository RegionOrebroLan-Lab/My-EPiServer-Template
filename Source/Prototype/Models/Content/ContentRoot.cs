using Prototype.Models.Internal;

namespace Prototype.Models.Content
{
	public class ContentRoot : ContentNode, IContentRoot
	{
		#region Constructors

		public ContentRoot(SiteMapNode siteMapNode) : base(null, siteMapNode) { }

		#endregion

		#region Properties

		protected internal override string UrlSegmentInternal => string.Empty;

		#endregion
	}
}