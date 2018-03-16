using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluegrams.Application
{
    /// <summary>
    /// A class containing information about the latest app update.
    /// </summary>
    [Serializable]
    public class AppUpdate
    {
        /// <summary>
        /// The latest released version of the application.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The link to the download of the latest version.
        /// </summary>
        public string DownloadLink { get; set; }

        /// <summary>
        /// Some notes to the latest version.
        /// </summary>
        public string VersionNotes { get; set; }
    }
}
