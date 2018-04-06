using System;
using System.Globalization;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Configuration;

namespace Bluegrams.Application
{
    /// <summary>
    /// The interface implemented by every MiniAppManager class.
    /// </summary>
    public abstract class MiniAppManagerBase
    {
        /// <summary>
        /// Indicates that a new update is available.
        /// </summary>
        public bool UpdateAvailable { get; private set; }
        /// <summary>
        /// Indicates whether a check for new updates took place.
        /// </summary>
        public bool UpdateCheckSuccessful { get; private set; }
        /// <summary>
        /// Indicates whether the user should be notified about a new update on every startup.
        /// </summary>
        public bool UpdateNotifyEveryStartup { get; set; }
        /// <summary>
        /// If set to true, the manager checks for '/portable' or '--portable' option on startup to run in portable mode.
        /// </summary>
        public bool PortableModeArgEnabled { get; set; }

        /// <summary>
        /// Static property indicating whether the app manager instance is in portable mode.
        /// </summary>
        public static bool PortableMode { get; private set; }

        /// <summary>
        /// Occurs when a made check for updates is completed.
        /// </summary>
        public event EventHandler CheckForUpdatesCompleted;

        /// <summary>
        /// The project's website shown in the 'About' box.
        /// </summary>
        public Link ProductWebsite { get; set; }
        /// <summary>
        /// A link to the license, under which the project is published.
        /// </summary>
        public Link ProductLicense { get; set; }
        /// <summary>
        /// A color object of the respective technology that is used for the title of the 'About' box.
        /// </summary>
        public object ProductColorObject { get; }
        /// <summary>
        /// An icon object of the respective technology that is displayed in the 'About' box.
        /// </summary>
        public object ProductImageObject { get; }
        /// <summary>
        /// A list containing cultures supported by the application.
        /// </summary>
        public CultureInfo[] SupportedCultures { get; set; }
        /// <summary>
        /// Information about the latest update of the application.
        /// </summary>
        public AppUpdate LatestUpdate { get; private set; }

        /// <summary>
        /// Initializes a new instance of MiniAppManagerBase.
        /// </summary>
        public MiniAppManagerBase() { PortableMode = false; }

        /// <summary>
        /// Initializes a new instance of MiniAppManagerBase.
        /// </summary>
        /// <param name="portable">true if app manager should run in portable mode; otherwise false.</param>
        public MiniAppManagerBase(bool portable)
        {
            PortableMode = portable;
        }

        /// <summary>
        /// Initializes the app manager. (This method should be called before the window is initialized.)
        /// </summary>
        public virtual void Initialize()
        {
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
        /// Checks for update information at the given URL (which should provide a serialized AppUpdate object).
        /// </summary>
        /// <param name="url">The URL to check.</param>
        public void CheckForUpdates(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            WebClient client = new WebClient();
            try
            {
                client.OpenReadAsync(new Uri(url));
                client.OpenReadCompleted += Client_OpenReadCompleted;       
            } catch { }
        }

        private void Client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppUpdate));
                using (Stream str = e.Result)
                {
                    this.LatestUpdate = (AppUpdate)serializer.Deserialize(str);
                }
                UpdateAvailable = new Version(LatestUpdate.Version) > new Version(AppInfo.Version);
                UpdateCheckSuccessful = true;
                if (CheckForUpdatesCompleted != null)
                    CheckForUpdatesCompleted(this, new EventArgs());
            } catch { }
        }
    }
}
