using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace LmsBackend.Localization
{
    public static class LmsBackendLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(LmsBackendConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(LmsBackendLocalizationConfigurer).GetAssembly(),
                        "LmsBackend.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
