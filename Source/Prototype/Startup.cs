using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Prototype.Models.Content;
using Prototype.Models.Internal;
using Prototype.Models.Navigation;
using Prototype.Models.ViewModels;
using Prototype.Models.ViewModels.Shared;

namespace Prototype
{
	public class Startup
	{
		#region Constructors

		public Startup(IHostingEnvironment hostingEnvironment)
		{
			this.HostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
		}

		#endregion

		#region Properties

		protected internal virtual IHostingEnvironment HostingEnvironment { get; }

		#endregion

		#region Methods

		public virtual void Configure(IApplicationBuilder applicationBuilder)
		{
			applicationBuilder.UseDeveloperExceptionPage();
			applicationBuilder.UseBrowserLink();
			applicationBuilder.UseStaticFiles();
			applicationBuilder.UseMvc();
		}

		public virtual void ConfigureServices(IServiceCollection services)
		{
			var contentRoot = this.CreateContentMap();

			services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddSingleton(contentRoot);
			services.AddScoped<IContentContext, ContentContext>();
			services.AddScoped<ILayoutFactory, LayoutFactory>();
			services.AddScoped<INavigationFactory, NavigationFactory>();
			services.AddScoped<IViewModelFactory, ViewModelFactory>();

			services.AddMvc()
				.WithRazorPagesRoot("/Views")
				.AddRazorPagesOptions(options =>
				{
					options.Conventions.AddPageRoute(contentRoot.Path, "/" + contentRoot.UrlSegment + "/");

					foreach(var contentNode in contentRoot.Descendants)
					{
						options.Conventions.AddPageRoute(contentNode.Path, contentNode.Url.ToString());
					}
				});
		}

		protected internal virtual IContentRoot CreateContentMap()
		{
			return new ContentRoot(this.CreateSiteMap());
		}

		protected internal virtual SiteMapNode CreateSiteMap()
		{
			var siteMapContent = File.ReadAllText(this.HostingEnvironment.ContentRootFileProvider.GetFileInfo("SiteMap.json").PhysicalPath);

			return JsonConvert.DeserializeObject<SiteMapNode>(siteMapContent);
		}

		#endregion
	}
}