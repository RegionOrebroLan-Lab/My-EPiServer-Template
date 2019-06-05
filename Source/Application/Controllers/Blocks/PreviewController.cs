using System;
using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using MyCompany.MyWebApplication.Models.ViewModels.Blocks;
using MyCompany.MyWebApplication.Models.ViewModels.Blocks.Internal;

namespace MyCompany.MyWebApplication.Controllers.Blocks
{
	[CLSCompliant(false)]
	[TemplateDescriptor(AvailableWithoutTag = false, Inherited = true, Tags = new[] {RenderingTags.Preview}, TemplateTypeCategory = TemplateTypeCategories.MvcController)]
	public class PreviewController : ActionControllerBase, IRenderTemplate<BlockData>
	{
		#region Constructors

		public PreviewController(IContentRouteHelper contentRouteHelper)
		{
			this.ContentRouteHelper = contentRouteHelper ?? throw new ArgumentNullException(nameof(contentRouteHelper));
		}

		#endregion

		#region Properties

		protected internal virtual IContentRouteHelper ContentRouteHelper { get; }

		#endregion

		#region Methods

		public virtual ActionResult Index()
		{
			var model = new PreviewViewModel(this.HttpContext);

			// ReSharper disable InvertIf
			if(!ContentReference.IsNullOrEmpty(this.ContentRouteHelper.ContentLink))
			{
				var contentArea = new ContentArea();
				contentArea.Items.Add(new ContentAreaItem {ContentLink = this.ContentRouteHelper.ContentLink});

				if(model.Mode == PreviewMode.RightArea || model.Mode == PreviewMode.RightAreaWithSubNavigation)
					model.RightArea = contentArea;
				else
					model.MainArea = contentArea;
			}
			// ReSharper restore InvertIf

			return this.View("~/Views/Shared/Blocks/Preview/Index.cshtml", model);
		}

		#endregion
	}
}