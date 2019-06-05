using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using EPiServer.Core;
using MyCompany.MyWebApplication.Models.ViewModels.Blocks.Internal;

namespace MyCompany.MyWebApplication.Models.ViewModels.Blocks
{
	public class PreviewViewModel : IPreviewViewModel
	{
		#region Fields

		private bool? _includeRightArea;
		private bool? _includeSubNavigation;
		private PreviewMode? _mode;
		private const string _modeParameterName = "PreviewMode";
		private IDictionary<PreviewMode, Uri> _modes;

		#endregion

		#region Constructors

		public PreviewViewModel(HttpContextBase httpContext)
		{
			this.HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
		}

		#endregion

		#region Properties

		public virtual CultureInfo Culture => CultureInfo.CurrentUICulture;
		protected internal virtual HttpContextBase HttpContext { get; }

		public virtual bool IncludeRightArea
		{
			get
			{
				if(this._includeRightArea == null)
					this._includeRightArea = this.Mode == PreviewMode.MainAreaWithRightArea || this.Mode == PreviewMode.MainAreaWithSubNavigationAndRightArea || this.Mode == PreviewMode.RightArea || this.Mode == PreviewMode.RightAreaWithSubNavigation;

				return this._includeRightArea.Value;
			}
		}

		public virtual bool IncludeSubNavigation
		{
			get
			{
				if(this._includeSubNavigation == null)
					this._includeSubNavigation = this.Mode == PreviewMode.MainAreaWithSubNavigation || this.Mode == PreviewMode.MainAreaWithSubNavigationAndRightArea || this.Mode == PreviewMode.RightAreaWithSubNavigation;

				return this._includeSubNavigation.Value;
			}
		}

		public virtual ContentArea MainArea { get; set; }

		public virtual PreviewMode Mode
		{
			get
			{
				if(this._mode == null)
					this._mode = Enum.TryParse(this.HttpContext.Request.QueryString[this.ModeParameterName], true, out PreviewMode mode) ? mode : PreviewMode.MainArea;

				return this._mode.Value;
			}
		}

		protected internal virtual string ModeParameterName => _modeParameterName;

		public virtual IDictionary<PreviewMode, Uri> Modes
		{
			get
			{
				// ReSharper disable All
				if(this._modes == null)
				{
					var modes = new Dictionary<PreviewMode, Uri>();
					var uriBuilder = new UriBuilder(this.HttpContext.Request.Url);
					var query = HttpUtility.ParseQueryString(uriBuilder.Query);

					foreach(var mode in Enum.GetValues(typeof(PreviewMode)).Cast<PreviewMode>())
					{
						query.Set(this.ModeParameterName, mode.ToString());

						uriBuilder.Query = query.ToString();

						var url = new Uri(uriBuilder.Uri.PathAndQuery + uriBuilder.Uri.Fragment, UriKind.Relative);

						modes.Add(mode, url);
					}

					this._modes = modes;
				}
				// ReSharper restore All

				return this._modes;
			}
		}

		public virtual ContentArea RightArea { get; set; }

		#endregion
	}
}