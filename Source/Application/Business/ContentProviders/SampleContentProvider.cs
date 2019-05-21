using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using EPiServer.Construction;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Localization;
using EPiServer.Web;
using MyCompany.MyWebApplication.Models.Pages;

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

		public SampleContentProvider(IContentFactory contentFactory, IContentTypeRepository contentTypeRepository, ILanguageBranchRepository cultureRepository, LocalizationService localization)
		{
			this.ContentFactory = contentFactory ?? throw new ArgumentNullException(nameof(contentFactory));
			this.ContentTypeRepository = contentTypeRepository ?? throw new ArgumentNullException(nameof(contentTypeRepository));
			this.CultureRepository = cultureRepository ?? throw new ArgumentNullException(nameof(cultureRepository));
			this.Localization = localization ?? throw new ArgumentNullException(nameof(localization));
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
					var cultures = this.CultureRepository.ListEnabled().Select(languageBranch => languageBranch.Culture).ToArray();

					var defaultCulture = this.CultureRepository.LoadFirstEnabledBranch().Culture;
					var defaultContent = this.CreateContent(defaultCulture, cultures, defaultCulture);

					contentDictionary.Add(CultureInfo.InvariantCulture, defaultContent);
					contentDictionary.Add(defaultCulture, defaultContent);

					foreach(var culture in cultures.Where(culture => !culture.Equals(defaultCulture)))
					{
						contentDictionary.Add(culture, this.CreateContent(culture, cultures, defaultCulture));
					}

					this._contentDictionary = contentDictionary;

					Thread.Sleep(500);
				}
				// ReSharper restore InvertIf

				return this._contentDictionary;
			}
		}

		protected internal new virtual IContentFactory ContentFactory { get; }
		protected internal new virtual IContentTypeRepository ContentTypeRepository { get; }
		protected internal virtual ILanguageBranchRepository CultureRepository { get; }
		protected internal virtual LocalizationService Localization { get; }
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
		protected internal virtual IContent CreateContent(CultureInfo culture, IEnumerable<CultureInfo> existingCultures, CultureInfo masterCulture)
		{
			if(culture == null)
				throw new ArgumentNullException(nameof(culture));

			const string localizationKeyPrefix = "/samplecontentprovider/";

			var contentType = this.ContentTypeRepository.Load<InformationPage>();
			var content = (InformationPage) this.ContentFactory.CreateContent(contentType);

			content.SetDefaultValues(contentType);

			content.ContentGuid = this.OnlyContentGuid;
			content.ContentLink = this.OnlyContentReference;
			content.ExistingLanguages = existingCultures;
			content.Heading = this.Localization.GetStringByCulture(localizationKeyPrefix + "heading", culture);
			content.Language = culture;
			content.LinkType = PageShortcutType.Normal;
			content.LinkURL = this.ConstructContentUri(contentType.ID, this.OnlyContentReference, this.OnlyContentGuid).ToString();
			content.MainBody = new XhtmlString(this.Localization.GetStringByCulture(localizationKeyPrefix + "mainbody", culture));
			content.MasterLanguage = masterCulture;
			content.Name = this.Localization.GetStringByCulture(localizationKeyPrefix + "name", culture);
			content.ParentLink = this.EntryPoint.ToPageReference();
			content.Status = VersionStatus.Published;
			content.URLSegment = this.Localization.GetStringByCulture(localizationKeyPrefix + "urlsegment", culture);
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