using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bluegrams.Application;

namespace Bluegrams.Application.WPF
{
    /// <summary>
    /// A class managing settings such as size and location for a WPF application. Also includes an 'About' form for applications.
    /// </summary>
    public class MiniAppManager : MiniAppManagerBase
    {
        private bool sizeable;
        private double savedWidth, savedHeight;
        private double savedLeft, savedTop;
        private WindowState savedWindowState;

        public Window Parent { get; private set; }
        /// <summary>
        /// The color used for the title of the 'About' box.
        /// </summary>
        public Color ProductColor { get; set; }
        /// <summary>
        /// The icon of the project used for the 'About' box.
        /// </summary>
        public BitmapSource ProductImage { get; set; }

        /// <summary>
        /// Creates a new instance of MiniAppManager
        /// </summary>
        /// <param name="parent">The parent window of the manager. (Project's main window.)</param>
        public MiniAppManager(Window parent) : this(parent, Colors.Gray, null) { }

        /// <summary>
        /// Creates a new instance of MiniAppManager
        /// </summary>
        /// <param name="parent">The parent window of the manager. (Project's main window.)</param>
        /// <param name="color">The color used for the title of the 'About' box.</param>
        public MiniAppManager(Window parent, Color color) : this(parent, color, null) { }

        /// <summary>
        /// Creates a new instance of MiniAppManager
        /// </summary>
        /// <param name="parent">The parent window of the manager. (Project's main window.)</param>
        /// <param name="color">The color used for the title of the 'About' box.</param>
        /// <param name="image">The icon of the project used for the 'About' box.</param>
        public MiniAppManager(Window parent, Color color, BitmapSource image) : this(parent, color, image, new Link(""), new Link("")) { }

        /// <summary>
        /// Creates a new instance of MiniAppManager
        /// </summary>
        /// <param name="parent">The parent window of the manager. (Project's main window.)</param>
        /// <param name="color">The color used for the title of the 'About' box.</param>
        /// <param name="image">The icon of the project used for the 'About' box.</param>
        /// <param name="website">The project's website shown in the 'About' box.</param>
        /// <param name="license">A link to the license, under which the project is published.</param>
        public MiniAppManager(Window parent, Color color, BitmapSource image, Link website, Link license)
        {
            Parent = parent;
            ProductLicense = license;
            ProductWebsite = website;
            ProductImage = image;
            ProductColor = color;
            SupportedCultures = new CultureInfo[0];
        }

        /// <summary>
        /// Initializes the app manager. (This method should be called before the window is initialized.)
        /// </summary>
        public override void Initialize()
        {
            if (!Properties.Settings.Default.Updated)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.Updated = true;
                Properties.Settings.Default.Save();
            }
            if (!String.IsNullOrEmpty(Properties.Settings.Default.Culture))
            {
                var culture = new System.Globalization.CultureInfo(Properties.Settings.Default.Culture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            }
            sizeable = Parent.ResizeMode == ResizeMode.CanResize;
            Parent.Loaded += Parent_Loaded;
            Parent.Closing += Parent_Closing;
            Parent.LocationChanged += Parent_LocationChanged; ;
            if (sizeable)
                Parent.SizeChanged += Parent_SizeChanged;
        }

        #region "Parent Events"
        private void Parent_Loaded(object sender, RoutedEventArgs e)
        {
            Parent.Left = Properties.Settings.Default.Left;
            Parent.Top = Properties.Settings.Default.Top;
            if (sizeable)
            {
                try
                {
                    Parent.Width = Properties.Settings.Default.Width;
                    Parent.Height = Properties.Settings.Default.Height;
                    Parent.WindowState = Properties.Settings.Default.WindowState;
                }
                catch
                {
                    Properties.Settings.Default.Reset();
                    Parent.Width = Properties.Settings.Default.Width;
                    Parent.Height = Properties.Settings.Default.Height;
                    Parent.WindowState = Properties.Settings.Default.WindowState;
                }
            }
            checkOutOfBorders();
            if (UpdateAvailable)
            {
                if (MessageBox.Show(Parent, 
                    String.Format(Application.Properties.Resources.strNewUpdate, AppInfo.ProductName, LatestUpdate.Version),
                    Application.Properties.Resources.strNewUpdateTitle, MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes) 
                {
                    Process.Start(LatestUpdate.DownloadLink);
                }
            }
        }

        private void checkOutOfBorders()
        {
            Rect windowRect = new Rect(Parent.Left, Parent.Top, Parent.Width, Parent.Height);
            Rect screenRect = new Rect(SystemParameters.VirtualScreenLeft, SystemParameters.VirtualScreenTop,
                SystemParameters.VirtualScreenWidth, SystemParameters.VirtualScreenHeight);
            if (!screenRect.IntersectsWith(windowRect))
            {
                if (Parent.Left < screenRect.Left)
                    Parent.Left = screenRect.Left;
                else if (Parent.Left > screenRect.Right)
                    Parent.Left = screenRect.Right - Parent.Width;
                if (Parent.Top < screenRect.Top)
                    Parent.Top = screenRect.Top;
                else if (Parent.Top > screenRect.Bottom)
                    Parent.Top = screenRect.Bottom - Parent.Height;
            }

        }

        private void Parent_LocationChanged(object sender, EventArgs e)
        {
            if (Parent.WindowState == WindowState.Normal)
            {
                savedLeft = Parent.Left;
                savedTop = Parent.Top;
            }
        }

        private void Parent_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Parent.WindowState == WindowState.Minimized)
                return;
            savedWindowState = Parent.WindowState;
            if (Parent.WindowState == WindowState.Normal)
            {
                savedWidth = Parent.Width;
                savedHeight = Parent.Height;
            }
        }

        private void Parent_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Left = savedLeft;
            Properties.Settings.Default.Top = savedTop;
            if (sizeable)
            {
                Properties.Settings.Default.Width = savedWidth;
                Properties.Settings.Default.Height = savedHeight;
                Properties.Settings.Default.WindowState = savedWindowState;
            }
            Properties.Settings.Default.Save();
        }
        #endregion

        #region "InfoWindow"
        /// <summary>
        /// Shows an 'About' box with application information.
        /// </summary>
        public override void ShowAboutBox()
        {
            InfoWindow info = new InfoWindow(this);
            info.ShowDialog();
        }

        /// <summary>
        /// Changes the application culture to the given culture.
        /// </summary>
        /// <param name="culture">The new culture to be set.</param>
        public override void ChangeCulture(CultureInfo culture)
        {
            Properties.Settings.Default.Culture = culture.Name;
            Parent_Closing(null, null);
            Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
    }
}
