using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Bluegrams.Application
{
    /// <summary>
    /// Provides application settings either from a local or a portable source.
    /// </summary>
    public class VariableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        SettingsProvider provider;

        public VariableSettingsProvider()
        {
            if (MiniAppManagerBase.PortableMode) provider = new PortableSettingsProvider();
            else provider = new LocalFileSettingsProvider();
        }

        public override string ApplicationName { get { return provider.ApplicationName; } set { } }

        public override string Name => "VariableSettingsProvider";

        public override void Initialize(string name, NameValueCollection config)
        {
            if (String.IsNullOrEmpty(name)) name = "VariableSettingsProvider";
            base.Initialize(name, config);
            provider.Initialize(name, config);
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            return provider.GetPropertyValues(context, collection);
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            provider.SetPropertyValues(context, collection);
        }

        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
        {
            if (provider is IApplicationSettingsProvider)
                return (provider as IApplicationSettingsProvider).GetPreviousVersion(context, property);
            else return null;
        }

        public void Reset(SettingsContext context)
        {
            if (provider is IApplicationSettingsProvider)
                (provider as IApplicationSettingsProvider).Reset(context);
        }

        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
            if (provider is IApplicationSettingsProvider)
                (provider as IApplicationSettingsProvider).Upgrade(context, properties);
        }

        /// <summary>
        /// Applies this settings provider to each property of the given settings.
        /// </summary>
        /// <param name="settingsList">An array of settings.</param>
        public static void ApplyProvider(params ApplicationSettingsBase[] settingsList)
        {
            foreach (var settings in settingsList) {
                var provider = new VariableSettingsProvider();
                settings.Providers.Add(provider);
                foreach (SettingsProperty prop in settings.Properties)
                    prop.Provider = provider;
                settings.Reload();
            }
        }
    }
}
