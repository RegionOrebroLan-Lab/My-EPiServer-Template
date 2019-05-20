using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using EPiServer.Construction;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Web;
using MyCompany.MyWebApplication.Models.Pages;
using RegionOrebroLan;

namespace MyCompany.MyWebApplication.Business.ContentProviders
{
	public class SampleContentProvider : ContentProvider
	{
		#region Fields

		private Dictionary<CultureInfo, IContent> _contentDictionary;
		private static readonly Guid _onlyContentGuid = new Guid("d7168715-ae4e-4856-9325-127b98f8c9fd");
		private static ContentReference _onlyContentReference;

		#endregion

		#region Constructors

		public SampleContentProvider(IContentFactory contentFactory, IContentTypeRepository contentTypeRepository, IDateTimeContext dateTimeContext, ILanguageBranchRepository languageBranchRepository)
		{
			this.ContentFactory = contentFactory ?? throw new ArgumentNullException(nameof(contentFactory));
			this.ContentTypeRepository = contentTypeRepository ?? throw new ArgumentNullException(nameof(contentTypeRepository));
			this.DateTimeContext = dateTimeContext ?? throw new ArgumentNullException(nameof(dateTimeContext));
			this.LanguageBranchRepository = languageBranchRepository ?? throw new ArgumentNullException(nameof(languageBranchRepository));
		}

		#endregion

		#region Properties

		protected internal virtual IDictionary<CultureInfo, IContent> ContentDictionary
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._contentDictionary == null)
				{
					var contentDictionary = new Dictionary<CultureInfo, IContent>();
					var cultures = this.LanguageBranchRepository.ListEnabled().Select(languageBranch => languageBranch.Culture).ToArray();
					var now = this.DateTimeContext.Now;

					var defaultCulture = this.LanguageBranchRepository.LoadFirstEnabledBranch().Culture;
					var defaultContent = this.CreateContent(defaultCulture, cultures, defaultCulture, now);

					contentDictionary.Add(CultureInfo.InvariantCulture, defaultContent);
					contentDictionary.Add(defaultCulture, defaultContent);

					foreach(var culture in cultures.Where(culture => !culture.Equals(defaultCulture)))
					{
						contentDictionary.Add(culture, this.CreateContent(culture, cultures, defaultCulture, now));
					}

					this._contentDictionary = contentDictionary;
				}
				// ReSharper restore InvertIf

				return this._contentDictionary;
			}
		}

		protected internal new virtual IContentFactory ContentFactory { get; }
		protected internal new virtual IContentTypeRepository ContentTypeRepository { get; }
		protected internal virtual IDateTimeContext DateTimeContext { get; }
		protected internal new virtual ILanguageBranchRepository LanguageBranchRepository { get; }
		protected internal virtual Guid OnlyContentGuid => _onlyContentGuid;

		protected internal virtual ContentReference OnlyContentReference
		{
			get
			{
				if(_onlyContentReference == null)
				{
					_onlyContentReference = new ContentReference
					{
						ID = 1,
						ProviderName = this.Name
					};
				}

				return _onlyContentReference;
			}
		}

		#endregion

		#region Methods

		[SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
		protected internal virtual IContent CreateContent(CultureInfo culture, IEnumerable<CultureInfo> existingCultures, CultureInfo masterCulture, DateTime startPublish)
		{
			if(culture == null)
				throw new ArgumentNullException(nameof(culture));

			const string namePrefix = "Provider-content";
			var informationSuffix = " in " + culture.NativeName;
			const string html = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse lobortis semper sapien at semper. Pellentesque maximus mollis ante vitae imperdiet. Duis molestie urna justo. Sed vel feugiat magna. Nullam blandit rhoncus nisl, faucibus lacinia libero mattis ut. In hac habitasse platea dictumst. Etiam tempus velit quis mi elementum, in sollicitudin sapien porttitor. Vestibulum quis lobortis nibh. Nullam sit amet augue a felis dignissim mollis. In hac habitasse platea dictumst. Fusce ornare pharetra enim a blandit. Aenean dignissim dolor nec nisi commodo, at posuere lectus consectetur. Aliquam faucibus rutrum justo. Cras consectetur mi sed arcu vulputate scelerisque eu ut ante. Sed et purus sed tellus fermentum condimentum eu eu odio.</p><p>Mauris aliquam eget justo sit amet rhoncus. Ut eget facilisis lorem. Phasellus vel turpis consequat, vestibulum nisl non, iaculis enim. Pellentesque a nisl id nibh faucibus euismod. Praesent varius ullamcorper ligula nec iaculis. Nunc hendrerit neque consequat dapibus consequat. Vivamus imperdiet, sapien et gravida tristique, mauris turpis porta arcu, eu placerat felis enim sed tortor.</p><p>Integer semper placerat eros at elementum. In hac habitasse platea dictumst. Sed tellus eros, ullamcorper sollicitudin imperdiet in, efficitur et massa. Morbi tempor tempus diam, quis imperdiet nisi euismod vel. Curabitur in pulvinar augue. Maecenas commodo aliquet orci sit amet tempus. Aenean velit tortor, consequat nec libero non, consequat pellentesque lacus. Vivamus tellus lorem, finibus ac nisi et, condimentum congue justo. Vestibulum semper et lacus ac tincidunt. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc consectetur at augue et pellentesque. Vestibulum consequat ornare rhoncus. Curabitur metus diam, dignissim et dapibus ut, mollis eget quam</p>";

			var contentType = this.ContentTypeRepository.Load<InformationPage>();
			var content = (InformationPage) this.ContentFactory.CreateContent(contentType);

			content.SetDefaultValues(contentType);

			content.ContentGuid = this.OnlyContentGuid;
			content.ContentLink = this.OnlyContentReference;
			content.ExistingLanguages = existingCultures;
			content.Heading = "Heading" + informationSuffix;
			content.Language = culture;
			content.LinkType = PageShortcutType.Normal;
			content.LinkURL = this.ConstructContentUri(contentType.ID, this.OnlyContentReference, this.OnlyContentGuid).ToString();
			content.MainBody = new XhtmlString("<h2>Main-body" + informationSuffix + "</h2>" + html);
			content.MasterLanguage = masterCulture;
			content.Name = namePrefix + informationSuffix;
			content.ParentLink = this.EntryPoint.ToPageReference();
			content.StartPublish = startPublish;
			content.Status = VersionStatus.Published;
			content.URLSegment = content.Name.ToLowerInvariant();
			content.VisibleInMenu = true;

			var contentSecurityDescriptor = content.GetContentSecurityDescriptor();
			contentSecurityDescriptor.IsInherited = true;
			contentSecurityDescriptor.ContentLink = content.ContentLink;

			content.MakeReadOnly();

			return content;
		}

		protected override IList<GetChildrenReferenceResult> LoadChildrenReferencesAndTypes(ContentReference contentLink, string languageID, out bool languageSpecific)
		{
			var loadChildrenReferencesAndTypes = base.LoadChildrenReferencesAndTypes(contentLink, languageID, out languageSpecific);

			if(this.EntryPoint.CompareToIgnoreWorkID(contentLink))
				loadChildrenReferencesAndTypes.Add(new GetChildrenReferenceResult {ContentLink = this.OnlyContentReference, IsLeafNode = true, ModelType = typeof(InformationPage)});

			return loadChildrenReferencesAndTypes;
		}

		protected override IContent LoadContent(ContentReference contentLink, ILanguageSelector languageSelector)
		{
			if(languageSelector == null)
				throw new ArgumentNullException(nameof(languageSelector));

			if(!this.OnlyContentReference.CompareToIgnoreWorkID(contentLink))
				throw new ContentNotFoundException(contentLink);

			if(!this.ContentDictionary.TryGetValue(languageSelector.Language, out var content))
				throw new ContentNotFoundException(contentLink);

			return content;
		}

		protected override ContentResolveResult ResolveContent(ContentReference contentLink)
		{
			// ReSharper disable InvertIf
			if(this.OnlyContentReference.CompareToIgnoreWorkID(contentLink))
			{
				var url = this.ConstructContentUri(this.ContentTypeRepository.Load<InformationPage>().ID, contentLink, this.OnlyContentGuid);

				return new ContentResolveResult {ContentLink = contentLink, ContentUri = url, UniqueID = this.OnlyContentGuid};
			}
			// ReSharper restore InvertIf

			return base.ResolveContent(contentLink);
		}

		protected override ContentResolveResult ResolveContent(Guid contentGuid)
		{
			// ReSharper disable InvertIf
			if(this.OnlyContentGuid.Equals(contentGuid))
			{
				var url = this.ConstructContentUri(this.ContentTypeRepository.Load<InformationPage>().ID, this.OnlyContentReference, contentGuid);

				return new ContentResolveResult {ContentLink = this.OnlyContentReference, ContentUri = url, UniqueID = contentGuid};
			}
			// ReSharper restore InvertIf

			return base.ResolveContent(contentGuid);
		}

		#endregion
	}
}