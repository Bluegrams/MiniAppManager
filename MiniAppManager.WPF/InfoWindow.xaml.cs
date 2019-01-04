using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Globalization;

namespace Bluegrams.Application.WPF
{
    internal partial class InfoWindow : Window
    {
        private MiniAppManager manager;

        public Link ProductWebsite
        {
            get { return AppInfo.ProductWebsite ?? manager.ProductWebsite; }
        }

        public Link ProductLicense
        {
            get { return AppInfo.ProductLicense ?? manager.ProductLicense; }
        }

        public Color ProductColor
        {
            get
            {
                if (AppInfo.ProductColor == null) return manager.ProductColor;
                else return (Color)AppInfo.ProductColor;
            }
        }

        public CultureInfo[] SupportedCultures
        {
            get { return AppInfo.SupportedCultures ?? manager.SupportedCultures; }
        }

        internal InfoWindow(MiniAppManager manager, ImageSource icon)
        {
            this.Owner = manager.Parent;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.manager = manager;
            this.DataContext = this;
            InitializeComponent();
            if (icon != null)
            {
                this.imgIcon.Source = icon;
                this.Icon = icon;
                brdIcon.Background = new SolidColorBrush(Colors.Transparent);
            }
            this.Title = Bluegrams.Application.Properties.Resources.strAbout + " " + Title;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (SupportedCultures.Length > 0)
            {
                foreach (CultureInfo cu in SupportedCultures)
                {
                    comLanguages.Items.Add(cu.DisplayName);
                    if (cu.TwoLetterISOLanguageName == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName)
                        comLanguages.SelectedIndex = comLanguages.Items.Count - 1;
                }
            }
            else
            {
                stackLang.Visibility = Visibility.Collapsed;
            }
            if (manager.UpdateAvailable)
                butUpdate.Visibility = Visibility.Visible;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Uri.OriginalString))
                Process.Start(e.Uri.OriginalString);
        }

        private void butRestart_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(Application.Properties.Resources.InfoWindow_RestartNewLang, "", MessageBoxButton.OKCancel, MessageBoxImage.Warning) 
                == MessageBoxResult.OK)
            {
                manager.ChangeCulture(SupportedCultures[comLanguages.SelectedIndex]);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void butUpdate_Click(object sender, RoutedEventArgs e)
        {
            manager.CheckForUpdates();
        }
    }
}
