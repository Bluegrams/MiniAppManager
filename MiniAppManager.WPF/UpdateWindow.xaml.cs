using System;
using System.Windows;

namespace Bluegrams.Application.WPF
{
    public partial class UpdateWindow : Window
    {
        /// <summary>
        /// Creates a new instance of UpdateWindow.
        /// </summary>
        public UpdateWindow(bool hasUpdate, AppUpdate update)
        {
            InitializeComponent();
            this.DataContext = update;
            txtTitle.Text = String.Format(Application.Properties.Resources.UpdateWindow_Header, AppInfo.ProductName);
            txtVersion.Text = String.Format(Application.Properties.Resources.UpdateWindow_Version,
                update.Version, update.ReleaseDate.ToShortDateString(), AppInfo.Version);
            txtWhatsNew.Text = String.Format(Application.Properties.Resources.UpdateWindow_WhatsNew, update.Version);
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
