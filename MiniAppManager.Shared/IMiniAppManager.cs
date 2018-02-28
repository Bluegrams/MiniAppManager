using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Globalization;

namespace Bluegrams.Application
{
    /// <summary>
    /// The interface implemented by every MiniAppManager class.
    /// </summary>
    public interface IMiniAppManager
    {
        /// <summary>
        /// The project's website shown in the 'About' box.
        /// </summary>
        Link ProductWebsite { get; set; }
        /// <summary>
        /// A link to the license, under which the project is published.
        /// </summary>
        Link ProductLicense { get; set; }
        /// <summary>
        /// A color object of the respective technology that is used for the title of the 'About' box.
        /// </summary>
        object ProductColorObject { get; }
        /// <summary>
        /// An icon object of the respective technology that is displayed in the 'About' box.
        /// </summary>
        object ProductImageObject { get; }
        /// <summary>
        /// A list containing cultures supported by the application.
        /// </summary>
        CultureInfo[] SupportedCultures { get; set; }

        /// <summary>
        /// Initializes the app manager. (This method should be called before the window is initialized.)
        /// </summary>
        void Initialize();
        /// <summary>
        /// Shows an 'About' box with application information.
        /// </summary>
        void ShowAboutBox();
        /// <summary>
        /// Changes the application culture to the given culture.
        /// </summary>
        /// <param name="culture">The new culture to be set.</param>
        void ChangeCulture(CultureInfo culture);
    }
}
