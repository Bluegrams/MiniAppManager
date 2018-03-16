using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.IO;
using System.Xml.Serialization;

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
        /// Initializes the app manager. (This method should be called before the window is initialized.)
        /// </summary>
        public abstract void Initialize();
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
        /// Checks for update information at the given URL (which should provide a serialized AppUpdate object).
        /// </summary>
        /// <param name="url">The URL to check.</param>
        public void CheckForUpdates(string url)
        {
            WebClient client = new WebClient();
            /*try
            {*/
                XmlSerializer serializer = new XmlSerializer(typeof(AppUpdate));
                using (Stream str = client.OpenRead(url))
                {
                    this.LatestUpdate = (AppUpdate)serializer.Deserialize(str);
                }
                UpdateAvailable = new Version(LatestUpdate.Version) > new Version(AppInfo.Version);
                UpdateCheckSuccessful = true;
            /*}
            catch { }*/
        }
    }
}
