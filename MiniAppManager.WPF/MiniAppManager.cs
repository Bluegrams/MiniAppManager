using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Bluegrams.Application.Properties;
using Bluegrams.Application.Update;

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

        /// <summary>
        /// The managed parent window.
        /// </summary>
        public Window Parent { get; private set; }
        /// <summary>
        /// The color used for the title of the 'About' box.
        /// </summary>
        [Obsolete("Please use assembly attributes instead.")]
        public Color ProductColor { get; set; }
        /// <summary>
        /// The icon of the project used for the 'About' box.
        /// </summary>
        [Obsolete("Please specify icon when showing About box instead.")]
        public BitmapSource ProductImage { get; set; }

        /// <summary>
        /// The settings of the manager.
        /// </summary>
        public override ApplicationSettingsBase Settings => Properties.Settings.Default;

        /// <summary>
        /// Creates a new instance of MiniAppManager
        /// </summary>
        /// <param name="parent">The parent window of the manager. (Project's main window.)</param>
        public MiniAppManager(Window parent) : this(parent, false) { }

        /// <summary>
        /// Creates a new instance of MiniAppManager
        /// </summary>
        /// <param name="parent">The parent window of the manager. (Project's main window.)</param>
        /// <param name="color">The color used for the title of the 'About' box.</param>
        [Obsolete]
        public MiniAppManager(Window parent, Color color) : this(parent, color, null) { }

        /// <summary>
        /// Creates a new instance of MiniAppManager
        /// </summary>
        /// <param name="parent">The parent window of the manager. (Project's main window.)</param>
        /// <param name="color">The color used for the title of the 'About' box.</param>
        /// <param name="image">The icon of the project used for the 'About' box.</param>
        [Obsolete]
        public MiniAppManager(Window parent, Color color, BitmapSource image) : this(parent, color, image, new Link(""), new Link("")) { }

        /// <summary>
        /// Creates a new instance of MiniAppManager
        /// </summary>
        /// <param name="parent">The parent window of the manager. (Project's main window.)</param>
        /// <param name="color">The color used for the title of the 'About' box.</param>
        /// <param name="image">The icon of the project used for the 'About' box.</param>
        /// <param name="website">The project's website shown in the 'About' box.</param>
        /// <param name="license">A link to the license, under which the project is published.</param>
        [Obsolete]
        public MiniAppManager(Window parent, Color color, BitmapSource image, Link website, Link license) : this(parent, false, color, image, website, license)
        { }

        /// <summary>
        /// Creates a new instance of MiniAppManager
        /// </summary>
        /// <param name="parent">The parent window of the manager. (Project's main window.)</param>
        /// <param name="portable">Indicates whether the manager should be run in portable mode.</param>
        public MiniAppManager(Window parent, bool portable) : base(parent, portable)
        {
            Parent = parent;
            SupportedCultures = new CultureInfo[0];
            setCulture();
        }

        /// <summary>
        /// Creates a new instance of MiniAppManager
        /// </summary>
        /// <param name="parent">The parent window of the manager. (Project's main window.)</param>
        /// <param name="portable">Indicates whether the manager should be run in portable mode.</param>
        /// <param name="color">The color used for the title of the 'About' box.</param>
        /// <param name="image">The icon of the project used for the 'About' box.</param>
        [Obsolete]
        public MiniAppManager(Window parent, bool portable, Color color, BitmapSource image) : this(parent, portable, color, image, new Link(""), new Link(""))
        { }

        /// <summary>
        /// Creates a new instance of MiniAppManager
        /// </summary>
        /// <param name="parent">The parent window of the manager. (Project's main window.)</param>
        /// <param name="portable">Indicates whether the manager should be run in portable mode.</param>
        /// <param name="color">The color used for the title of the 'About' box.</param>
        /// <param name="image">The icon of the project used for the 'About' box.</param>
        /// <param name="website">The project's website shown in the 'About' box.</param>
        /// <param name="license">A link to the license, under which the project is published.</param>
        [Obsolete]
        public MiniAppManager(Window parent, bool portable, Color color, BitmapSource image, Link website, Link license) : base(parent, portable)
        {
            Parent = parent;
            ProductLicense = license;
            ProductWebsite = website;
            ProductImage = image;
            ProductColor = color;
            SupportedCultures = new CultureInfo[0];
            setCulture();
        }

        private void setCulture()
        {
            if (!String.IsNullOrEmpty(Properties.Settings.Default.Culture))
            {
                var culture = new System.Globalization.CultureInfo(Properties.Settings.Default.Culture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            }
        }

        /// <summary>
        /// Sets up the automatic saving of the window state and custom properties.
        /// (This method should be called before the window is initialized.)
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            if (!Properties.Settings.Default.Updated)
            {
                Properties.Settings.Default.Upgrade();
                base.Upgrade();
                Properties.Settings.Default.Updated = true;
                Properties.Settings.Default.Save();
            }
            Parent.Loaded += Parent_Loaded;
            Parent.Closing += Parent_Closing;
            Parent.LocationChanged += Parent_LocationChanged; ;

        }

        #region "Parent Events"
        private void Parent_Loaded(object sender, RoutedEventArgs e)
        {
            base.Parent_Loaded();
            sizeable = Parent.ResizeMode == ResizeMode.CanResize || AlwaysTrackResize;
            if (sizeable) Parent.SizeChanged += Parent_SizeChanged;
            Parent.Left = Properties.Settings.Default.Left;
            Parent.Top = Properties.Settings.Default.Top;
            if (sizeable)
            {
                try
                {
                    savedWidth = Parent.Width; savedHeight = Parent.Height;
                    if ((int)Properties.Settings.Default.Width != -1)
                        Parent.Width = Properties.Settings.Default.Width;
                    if ((int)Properties.Settings.Default.Height != -1)
                        Parent.Height = Properties.Settings.Default.Height;
                    Parent.WindowState = Properties.Settings.Default.WindowState;
                }
                catch
                {
                    Properties.Settings.Default.Reset();
                    Parent.WindowState = Properties.Settings.Default.WindowState;
                }
            }
            checkOutOfBorders();
        }

        private void checkOutOfBorders()
        {
            Rect windowRect = new Rect(Parent.Left, Parent.Top, 
                                       Parent.Width, Parent.Height);
            Rect screenRect = 
                new Rect(fromPhysical(SystemParameters.VirtualScreenLeft, SystemParameters.VirtualScreenTop),
                                       fromPhysical(SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth, 
                                                    SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight));
            if (!screenRect.IntersectsWith(windowRect))
            {
                if (windowRect.Left < screenRect.Left)
                    Parent.Left = screenRect.Left;
                else if (windowRect.Left > screenRect.Right)
                    Parent.Left = screenRect.Right - Parent.Width;
                if (windowRect.Top < screenRect.Top)
                    Parent.Top = screenRect.Top;
                else if (windowRect.Top > screenRect.Bottom)
                    Parent.Top = screenRect.Bottom - Parent.Height;
            }
        }

        // transforms physical coordinates to a scaled point.
        private Point fromPhysical(double x, double y)
        {
            Matrix transform = PresentationSource.FromVisual(Parent).CompositionTarget.TransformFromDevice;
            return transform.Transform(new Point(x, y));
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
            base.Parent_Closing();
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

        #region "Update Check"

        /// <summary>
        /// This method is invoked after a check for updates is completed.
        /// </summary>
        protected override void OnCheckForUpdatesCompleted(UpdateCheckEventArgs e)
        {
            base.OnCheckForUpdatesCompleted(e);
            if (UpdateNotifyMode == UpdateNotifyMode.Never)
                return;
            bool always = UpdateNotifyMode == UpdateNotifyMode.Always;
            System.Diagnostics.Debug.WriteLine("Mode:" + always);
            if (e.NewVersion)
            {
                Properties.Settings.Default.CheckedUpdate = e.Update.Version;
                Properties.Settings.Default.Save();
            }
            if (e.Successful && e.NewVersion)
            {
                UpdateWindow updateWindow = new UpdateWindow(e.NewVersion, e.Update);
                updateWindow.Owner = Parent;
                if (updateWindow.ShowDialog().GetValueOrDefault())
                {
                    string path = Task.Run(async () => await UpdateChecker.DownloadUpdate(e.Update)).Result;
                    if (path != null)
                    {
                        if (System.IO.Path.GetExtension(path) == ".msi")
                            UpdateChecker.ApplyMsiUpdate(path);
                        else UpdateChecker.ShowUpdateDownload(path);
                    }
                    else
                    {
                        MessageBox.Show(Parent, Resources.Box_UpdateFailed, Resources.Box_UpdateFailed_Title,
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else if (always)
            {
                MessageBox.Show(Parent, Resources.Box_NoNewUpdate, Resources.strSoftwareUpdate);
            }
        }
        #endregion

        #region "InfoWindow"
        /// <summary>
        /// Shows an 'About' box with application information.
        /// </summary>
        public override void ShowAboutBox() => ShowAboutBox(ProductImage);

        /// <summary>
        /// Shows an 'About' box with application information.
        /// </summary>
        /// <param name="icon">The product icon of the application.</param>
        public void ShowAboutBox(ImageSource icon)
        {
            InfoWindow info = new InfoWindow(this, icon);
            info.Topmost = Parent.Topmost;
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
            if (Environment.GetCommandLineArgs().Length > 1)
            {
                string[] args = new string[Environment.GetCommandLineArgs().Length - 1];
                Array.Copy(Environment.GetCommandLineArgs(), 1, args, 0, args.Length);
                Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location, String.Join(" ", args));
            }
            else Process.Start(System.Reflection.Assembly.GetEntryAssembly().Location);
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
    }
}
