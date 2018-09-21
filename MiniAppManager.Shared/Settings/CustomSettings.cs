using System;
using System.ComponentModel;
using System.Reflection;
using System.Configuration;

namespace Bluegrams.Application
{
    /// <summary>
    /// The custom settings handled by the manager.
    /// </summary>
    public sealed class CustomSettings : SettingsBase
    {
        // Single instance:

        private static CustomSettings defaultInstance = ((CustomSettings)(Synchronized(new CustomSettings())));
        /// <summary>
        /// The settings instance.
        /// </summary>
        public static CustomSettings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        // Class members:

        // all properties in this settings class use the same provider.
        private SettingsProvider provider { get { return Providers["VariableSettingsProvider"]; } }

        private CustomSettings() : base()
        {
            var context = new SettingsContext();
            Type type = this.GetType();
            context["GroupName"] = type.FullName;
            context["SettingsClassType"] = type;
            var providers = new SettingsProviderCollection();
            providers.Add(new VariableSettingsProvider());
            base.Initialize(context, new SettingsPropertyCollection(), providers);
        }

        /// <summary>
        /// Adds a settings property.
        /// </summary>
        /// <param name="propInfo">The property info of the property to add.</param>
        internal void AddSetting(PropertyInfo propInfo) => AddSetting(propInfo, getSerializeAs(propInfo.PropertyType));

        /// <summary>
        /// Adds a settings property.
        /// </summary>
        /// <param name="propInfo">The property info of the property to add.</param>
        /// <param name="serializeAs">The serialization mode of the property.</param>
        /// <param name="defaultValue">The default value of the property.</param>
        /// <param name="roamed">Specifies if this setting should be roamed.</param>
        internal void AddSetting(PropertyInfo propInfo, SettingsSerializeAs serializeAs, object defaultValue = null, bool roamed = false)
        {
            SettingsProperty settingsProp = new SettingsProperty(propInfo.Name);
            settingsProp.PropertyType = propInfo.PropertyType;
            settingsProp.Provider = provider;
            settingsProp.Attributes.Add(typeof(UserScopedSettingAttribute), new UserScopedSettingAttribute());
            if (roamed)
            {
                settingsProp.Attributes.Add(typeof(SettingsManageabilityAttribute),
                    new SettingsManageabilityAttribute(SettingsManageability.Roaming));
            }
            settingsProp.SerializeAs = serializeAs;
            settingsProp.DefaultValue = defaultValue;
            SettingsPropertyValue val = new SettingsPropertyValue(settingsProp);
            Properties.Add(settingsProp);
        }

        /// <devdoc>
        ///     Causes a reload to happen on next setting access, by clearing the cached values.
        /// </devdoc>
        public void Reload()
        {
            if (PropertyValues != null)
            {
                PropertyValues.Clear();
            }
        }

        /// <summary>
        /// Calls Reset on the provider.
        /// </summary>
        public void Reset()
        {
            if (Properties != null)
            {
                IApplicationSettingsProvider clientProv = provider as IApplicationSettingsProvider;
                if (clientProv != null)
                {
                    clientProv.Reset(Context);
                }
            }
            Reload();
        }

        /// <summary>
        /// Calls Upgrade on the provider.
        /// </summary>
        public void Upgrade()
        {
            if (Properties != null)
            {
                IApplicationSettingsProvider clientProv = provider as IApplicationSettingsProvider;
                if (clientProv != null)
                {
                    clientProv.Upgrade(Context, Properties);
                }
            }
            Reload();
        }

        private SettingsSerializeAs getSerializeAs(Type type)
        {
            // Check whether the type has a TypeConverter that can convert to/from string
            TypeConverter tc = TypeDescriptor.GetConverter(type);
            bool toString = tc.CanConvertTo(typeof(string));
            bool fromString = tc.CanConvertFrom(typeof(string));
            if (toString && fromString)
            {
                return SettingsSerializeAs.String;
            }
            //Else use Xml Serialization 
            return SettingsSerializeAs.Xml;
        }
    }
}
