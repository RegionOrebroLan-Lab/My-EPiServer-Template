using System;
using System.Web.Mvc;
using EPiServer.Forms.Controllers;
using EPiServer.Forms.Implementation.Elements;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;

namespace MyCompany.MyWebApplication.Controllers.Forms
{
	[CLSCompliant(false)]
	[TemplateDescriptor(AvailableWithoutTag = true, Default = true, ModelType = typeof(FormContainerBlock), TemplateTypeCategory = TemplateTypeCategories.MvcPartialController)]
	public class FormController : FormContainerBlockController
	{
		#region Methods

		public override ActionResult Index(FormContainerBlock currentBlock)
		{
			var viewResult = base.Index(currentBlock);

			return viewResult;
		}

		#endregion
	}
}