using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Bluegrams.Application;
using Bluegrams.Application.WPF;
using System.Diagnostics;

namespace TestWpfApp
{
    /// <summary>
    /// An example app using MiniAppManager for WPF.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Define a new variable for the manager of this app.
        MiniAppManager manager;

        public int OpenCount { get; set; }

        public MainWindow()
        {
            // Create a new instance of MiniAppManager for WPF. 
            // The second parameter specifies if the manager should be run in portable mode.
            manager = new MiniAppManager(this, true);

            // --- EXAMPLE 2: Make Settings Portable ---
            // If set to true (false by default), the manager checks for '/portable' or
            // '--portable' options given at startup. If it finds one of these it runs in portable mode.
            manager.PortableModeArgEnabled = true;
            // Make additional application settings portable.
            manager.MakePortable(Properties.Settings.Default);

            // Add any public property of the window with this method to let its value
            // be saved when the application is closed and loaded when it starts.
            manager.AddManagedProperty(nameof(this.OpenCount), System.Configuration.SettingsSerializeAs.String, -1);

            // Initialize the manager. Please make sure this method is called BEFORE the window is initialized.
            manager.Initialize();

            //Initialize the window.
            InitializeComponent();

            // The saved settings of the manager can be accessed.
            this.DataContext = manager.Settings;
        }


        private void butAbout_Click(object sender, RoutedEventArgs e)
        {
            // --- EXAMPLE 3: Application 'About' Box ---
            // Load the icon used in the 'About' box.
            var baseUri = BaseUriHelper.GetBaseUri(this);
            BitmapSource icon = new BitmapImage(new Uri(baseUri, @"/bluelogo.png"));
            // Show the 'About' box.
            // The shown data is specified as assembly attributes in AssemblyInfo.cs.
            manager.ShowAboutBox(icon);
            OpenCount++;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            loadSettings();
            // --- EXAMPLE 4: Check for App Updates ---
            // Specify if an informational message box should be shown if an update is available.
            manager.UpdateNotifyMode = UpdateNotifyMode.Always;
            // This event is fired when update checking has finished.
            manager.CheckForUpdatesCompleted += delegate (object s, UpdateCheckEventArgs args)
            {
                Debug.WriteLine("Update check completed.");
                if (!args.Successful)
                    Debug.WriteLine("Update check failed!");
                else Debug.WriteLine($"Found version: {args.Update.Version}.");
            };
            // Tell the manager to check for updates at the given URL. An XML file 
            // containing a serialized AppUpdate object is expected at that location.
            // This method should also be called before the initialization of the window.
            manager.CheckForUpdates("https://raw.githubusercontent.com/bluegrams/MiniAppManager/master/TestWpfApp/AppUpdateExample.xml");
        }

        private void loadSettings()
        {
            lblLang.Content = Properties.Resources.Text;
            txtLocal.Text = Properties.Settings.Default.LocalSetting;
            txtRoamed.Text = Properties.Settings.Default.RoamedSetting;
            lblCount.Content = OpenCount;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.LocalSetting = txtLocal.Text;
            Properties.Settings.Default.RoamedSetting = txtRoamed.Text;
            Properties.Settings.Default.Save();
        }
    }
}
