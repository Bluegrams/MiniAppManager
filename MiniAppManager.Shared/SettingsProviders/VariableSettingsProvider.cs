using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Bluegrams.Application.SettingsProviders
{
    public class VariableSettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        SettingsProvider provider;

        public VariableSettingsProvider()
        {
            if (MiniAppManagerBase.PortableMode) provider = new PortableSettingsProvider();
            else provider = new LocalFileSettingsProvider();
        }

        public override string ApplicationName { get => provider.ApplicationName; set { } }

        public override string Name => provider.Name;

        public override void Initialize(string name, NameValueCollection config)
        {
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
    }
}
