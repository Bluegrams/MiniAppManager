using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Globalization;
using Bluegrams.Application;
using Bluegrams.Application.WPF;

namespace TestWpfApp
{
    /// <summary>
    /// An example app using MiniAppManager for WPF.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Define a new variable for the manager of this app.
        MiniAppManager man;

        public MainWindow()
        {
            var baseUri = BaseUriHelper.GetBaseUri(this);
            BitmapSource img = new BitmapImage(new Uri(baseUri, @"/bluelogo.png"));
            
            // Create a new instance of MiniAppManager for WPF with some data used in the 'About' box.
            man = new MiniAppManager(this, Color.FromRgb(51, 85, 119), img, new Link("http://example.org", "Example.org"), new Link("https://opensource.org/licenses/MIT", "MIT License"));

            // (Optional) Specifiy a list of cultures your application supports to fill a combo box that allows switching between these.
            // If this property is not specified, the combo box won't be visible on the 'About' box.
            man.SupportedCultures = new CultureInfo[] { new CultureInfo("en"), new CultureInfo("de") };

            // Initialize the manager. Please make sure this method is called BEFORE you initialize your window.
            man.Initialize();

            // Tells the manager to check for updates at the given URL. An XML file containing a serialized AppUpdate object is expected
            // at that location. This method should also be called before the initialization of the window.
            man.CheckForUpdates("http://example.org/updates/TestWpfApp.xml");

            // Set this property to true to show an update notification every time the application is started. On default, a notification
            // is shown only every time a newer version is detected.
            man.UpdateNotifyEveryStartup = true;

            //Initialize the window.
            InitializeComponent();
            lblLang.Content = Properties.Resources.Text;

            // The saved settings of the manager can be accessed.
            this.DataContext = Bluegrams.Application.WPF.Properties.Settings.Default;
        }

        private void butAbout_Click(object sender, RoutedEventArgs e)
        {
            // Show the 'About' box.
            man.ShowAboutBox();
        }
    }
}
