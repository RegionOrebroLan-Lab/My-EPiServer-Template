using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Prototype
{
	public class Startup
	{
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
			services.AddMvc().WithRazorPagesRoot("/Views");
		}

		#endregion
	}
}