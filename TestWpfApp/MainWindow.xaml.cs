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
            // Create a new instance of MiniAppManager for WPF. 
            // The second parameter specifies if the manager should be run in portable mode.
            man = new MiniAppManager(this, false);

            // Fill some data used in the 'About' box.
            var baseUri = BaseUriHelper.GetBaseUri(this);
            BitmapSource img = new BitmapImage(new Uri(baseUri, @"/bluelogo.png"));
            man.ProductColor = Color.FromRgb(51, 85, 119);
            man.ProductImage = img;
            man.ProductWebsite = new Link("http://example.org", "Example.org");
            man.ProductLicense = new Link("https://opensource.org/licenses/BSD-3-Clause", "BSD-3-Clause License");

            // (Optional) If set to true (false by default), the manager checks for '/portable' or
            // '--portable' options given at startup. If it finds one of these it runs in portable mode.
            man.PortableModeArgEnabled = true;

            // (Optional) Specifiy a list of cultures your application supports to fill a combo box 
            // that allows switching between these. If this property is not specified, 
            // the combo box won't be visible on the 'About' box.
            man.SupportedCultures = new CultureInfo[] { new CultureInfo("en"), new CultureInfo("de") };

            // Initialize the manager. Please make sure this method is called BEFORE you initialize your window.
            man.Initialize();

            // (Optional) Tells the manager to check for updates at the given URL. An XML file 
            // containing a serialized AppUpdate object is expected at that location.
            // This method should also be called before the initialization of the window.
            man.CheckForUpdates("http://example.org/updates/TestWpfApp.xml");

            // (Together with update checking) Set this property to true to show an update notification every time 
            // the application is started. On default, a notification is shown only every time a newer version is detected.
            man.UpdateNotifyEveryStartup = true;

            //Initialize the window.
            InitializeComponent();

            // The saved settings of the manager can be accessed.
            this.DataContext = Bluegrams.Application.WPF.Properties.Settings.Default;
        }


        private void butAbout_Click(object sender, RoutedEventArgs e)
        {
            // Show the 'About' box.
            man.ShowAboutBox();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblLang.Content = Properties.Resources.Text;
            txtLocal.Text = Properties.Settings.Default.LocalSetting;
            txtRoamed.Text = Properties.Settings.Default.RoamedSetting;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.LocalSetting = txtLocal.Text;
            Properties.Settings.Default.RoamedSetting = txtRoamed.Text;
            Properties.Settings.Default.Save();
        }
    }
}
