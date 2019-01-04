using System;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;
using System.Collections;
using Bluegrams.Application.Update;

namespace Bluegrams.Application
{
    /// <summary>
    /// The interface implemented by every MiniAppManager class.
    /// </summary>
    public abstract class MiniAppManagerBase
    {
        private object parent;
        private List<string> managedSettings;

        /// <summary>
        /// Indicates that a new update is available.
        /// </summary>
        [Obsolete("Please use properties of UpdateCheckEventArgs instead.")]
        public bool UpdateAvailable { get; private set; }
        /// <summary>
        /// Sets how the standard update notification should be shown after checking for updates.
        /// </summary>
        public UpdateNotifyMode UpdateNotifyMode { get; set; }
        /// <summary>
        /// If set to true, the manager checks for '/portable' or '--portable' option on startup to run in portable mode.
        /// </summary>
        [Obsolete]
        public bool PortableModeArgEnabled { get; set; }
        /// <summary>
        /// If set true, also saves/ restores the window's sizes when it is not marked as resizable.
        /// </summary>
        public bool AlwaysTrackResize { get; set; }

        /// <summary>
        /// Static property indicating whether the app manager instance is in portable mode.
        /// </summary>
        public static bool PortableMode { get; private set; }

        /// <summary>
        /// Occurs when a made check for updates is completed.
        /// </summary>
        public event EventHandler<UpdateCheckEventArgs> CheckForUpdatesCompleted;

        /// <summary>
        /// The project's website shown in the 'About' box.
        /// </summary>
        [Obsolete("Please use assembly attributes instead.")]
        public Link ProductWebsite { get; set; }
        /// <summary>
        /// A link to the license, under which the project is published.
        /// </summary>
        [Obsolete("Please use assembly attributes instead.")]
        public Link ProductLicense { get; set; }
        /// <summary>
        /// A list containing cultures supported by the application.
        /// </summary>
        [Obsolete("Please use assembly attributes instead.")]
        public CultureInfo[] SupportedCultures { get; set; }
        /// <summary>
        /// Information about the latest update of the application.
        /// </summary>
        [Obsolete]
        public AppUpdate LatestUpdate { get; private set; }

        /// <summary>
        /// The URL where to look for information about updates.
        /// </summary>
        public string UpdateCheckUrl { get; set; }

        /// <summary>
        /// The settings of the manager.
        /// </summary>
        public abstract ApplicationSettingsBase Settings { get; }

        /// <summary>
        /// Initializes a new instance of MiniAppManagerBase.
        /// </summary>
        public MiniAppManagerBase(object parent) : this(parent, false) { }

        /// <summary>
        /// Initializes a new instance of MiniAppManagerBase.
        /// </summary>
        /// <param name="parent">The parent window.</param>
        /// <param name="portable">true if app manager should run in portable mode; otherwise false.</param>
        public MiniAppManagerBase(object parent, bool portable)
        {
            this.parent = parent;
            PortableMode = portable;
            UpdateNotifyMode = UpdateNotifyMode.IfNewUpdate;
            managedSettings = new List<string>();
        }

        /// <summary>
        /// Sets up the automatic saving of the window state and custom properties.
        /// (This method should be called before the window is initialized.)
        /// </summary>
        public virtual void Initialize()
        {
            // To be removed
            if (PortableModeArgEnabled)
            {
                string[] args = Environment.GetCommandLineArgs();
                if (Array.IndexOf(args, "/portable") >= 0 || Array.IndexOf(args, "--portable") >= 0)
                {
                    PortableMode = true;
                }
            }
        }

        /// <summary>
        /// Shows an 'About' box with application information.
        /// </summary>
        public abstract void ShowAboutBox();
        /// <summary>
        /// Changes the application culture to the given culture.
        /// </summary>
        /// <param name="culture">The new culture to be set.</param>
        public abstract void ChangeCulture(CultureInfo culture);

        /// <summary>
        /// Makes the given array of settings portable together with the manager.
        /// </summary>
        /// <param name="settings">An array of custom settings.</param>
        public void MakePortable(params ApplicationSettingsBase[] settings)
        {
            VariableSettingsProvider.ApplyProvider(settings);
        }

        /// <summary>
        /// Adds a public property of the managed window to the managed properties.
        /// </summary>
        /// <param name="propertyName">The name of the property to add.</param>
        public void AddManagedProperty(string propertyName)
        {
            managedSettings.Add(propertyName);
            CustomSettings.Default.AddSetting(parent.GetType().GetProperty(propertyName));
        }

        /// <summary>
        /// Adds a public property of the managed window to the managed properties.
        /// </summary>
        /// <param name="propertyName">The name of the property to add.</param>
        /// <param name="serializeAs">Specifically sets how the property should be serialized.</param>
        /// <param name="defaultValue">The default value to use if no saved value is found.</param>
        /// <param name="roamed">Specifies if this setting should be roamed.</param>
        public void AddManagedProperty(string propertyName, SettingsSerializeAs serializeAs, object defaultValue = null, bool roamed = false)
        {
            managedSettings.Add(propertyName);
            CustomSettings.Default.AddSetting(parent.GetType().GetProperty(propertyName), serializeAs, defaultValue, roamed);
        }

        /// <summary>
        /// Adds public properties of the managed window to the managed properties.
        /// </summary>
        /// <param name="propertyNames">The names of the properties to add.</param>
        public void AddManagedProperties(params string[] propertyNames)
        {
            foreach (string prop in propertyNames)
                AddManagedProperty(prop);
        }

        /// <summary>
        /// Checks for update information.
        /// </summary>
        public void CheckForUpdates()
        {
            UpdateChecker.CheckForUpdates(UpdateCheckUrl, OnCheckForUpdatesCompleted);
        }

        /// <summary>
        /// Checks for update information at the given URL (which should provide a serialized AppUpdate object).
        /// </summary>
        /// <param name="url">The URL to check.</param>
        [Obsolete("Use UpdateCheckUrl property")]
        public void CheckForUpdates(string url)
        {
            UpdateCheckUrl = url;
            UpdateChecker.CheckForUpdates(url, OnCheckForUpdatesCompleted);
        }

        /// <summary>
        /// This method is invoked after a check for updates is completed.
        /// </summary>
        protected virtual void OnCheckForUpdatesCompleted(UpdateCheckEventArgs e)
        {
            LatestUpdate = e.Update;
            UpdateAvailable = e.NewVersion;
            CheckForUpdatesCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Upgrades all managed settings.
        /// </summary>
        protected void Upgrade()
        {
            Properties.SharedSettings.Default.Upgrade();
            CustomSettings.Default.Upgrade();
        }

        /// <summary>
        /// This method has to be called during the loading event of the parent window.
        /// </summary>
        protected void Parent_Loaded()
        {
            if (legacySettingsUpgrade(parent)) return;
            foreach (string s in managedSettings)
            {
                object setting = CustomSettings.Default[s];
                if (setting == null) continue;
                var prop = parent.GetType().GetProperty(s);
                prop.SetValue(parent, setting);
            }
        }

        // one-way upgrade old settings stored in the format used prior to v.0.4.
        private bool legacySettingsUpgrade(object parent)
        {
            Hashtable customSettings = Properties.SharedSettings.Default.CustomSettings;
            if (customSettings == null || customSettings.Count < 1) return false;
            foreach (string s in managedSettings)
            {
                if (customSettings.ContainsKey(s))
                {
                    parent.GetType().GetProperty(s).SetValue(parent, customSettings[s]);
                }
            }
            Properties.SharedSettings.Default.CustomSettings = null;
            Properties.SharedSettings.Default.Save();
            return true;
        }

        /// <summary>
        /// This method has to be called during the closing event of the parent window.
        /// </summary>
        protected void Parent_Closing()
        {
            if (managedSettings.Count < 1) return;
            foreach (string s in managedSettings)
            {
                CustomSettings.Default[s] = parent.GetType().GetProperty(s).GetValue(parent);
            }
            CustomSettings.Default.Save();
        }
    }
}
