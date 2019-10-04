using System.Linq;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCompany.MyWebApplication.Models.Pages;
using RegionOrebroLan.EPiServer;

namespace MyCompany.MyWebApplication.IntegrationTests
{
	[TestClass]
	public class SampleTest
	{
		#region Methods

		[TestMethod]
		public void Test()
		{
			var contentFacade = ServiceLocator.Current.GetInstance<IContentFacade>();

			var children = contentFacade.Loader.GetChildren<SitePage>(ContentReference.StartPage);

			Assert.AreEqual(1, children.Count());
		}

		#endregion
	}
}