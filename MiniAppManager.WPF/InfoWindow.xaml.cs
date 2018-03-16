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

        internal InfoWindow(MiniAppManager manager)
        {
            this.Owner = manager.Parent;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.manager = manager;
            this.DataContext = manager;
            InitializeComponent();
            if (manager.ProductImage != null)
            {
                this.imgIcon.Source = manager.ProductImage;
                this.Icon = manager.ProductImage;
                brdIcon.Background = new SolidColorBrush(Colors.Transparent);
            }
            this.Title = Bluegrams.Application.Properties.Resources.strTitle + " " + Title;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (manager.SupportedCultures.Length > 0)
            {
                foreach (CultureInfo cu in manager.SupportedCultures)
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
            if (MessageBox.Show(Application.Properties.Resources.strRestartNewLang, "", MessageBoxButton.OKCancel, MessageBoxImage.Warning) 
                == MessageBoxResult.OK)
            {
                manager.ChangeCulture(manager.SupportedCultures[comLanguages.SelectedIndex]);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }

        private void butUpdate_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(manager.LatestUpdate.DownloadLink);
        }
    }
}
