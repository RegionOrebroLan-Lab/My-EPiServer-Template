using System;
using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace MyCompany.MyWebApplication.Business.Bootstrapping
{
	public class Startup
	{
		#region Constructors

		public Startup() { }

		#endregion

		#region Methods

		public virtual void Configuration(IAppBuilder applicationBuilder)
		{
			applicationBuilder.AddCmsAspNetIdentity<ApplicationUser>();

			applicationBuilder.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Util/Login.aspx"),
				Provider = new CookieAuthenticationProvider
				{
					OnApplyRedirect = cookieApplyRedirectContext => { applicationBuilder.CmsOnCookieApplyRedirect(cookieApplyRedirectContext, cookieApplyRedirectContext.OwinContext.Get<ApplicationSignInManager<ApplicationUser>>()); },

					OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager<ApplicationUser>, ApplicationUser>(
						TimeSpan.FromMinutes(30),
						(manager, user) => manager.GenerateUserIdentityAsync(user))
				}
			});
		}

		#endregion
	}
}