using System;
using System.Windows;
using System.Windows.Input;

namespace UI.Desktop.Profile
{
    /// <summary>
    /// Interaction logic for profileInputDialog.xaml
    /// </summary>
    public partial class ProfileInputDialog : Window
    {
        string ret = "";

        public ProfileInputDialog()
        {
            InitializeComponent();
            ProfilenameTextBox.Focus();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(ProfilenameTextBox.Text))
            {
                ret = ProfilenameTextBox.Text;
                Close();
            }
            else
            {
                MessageBox.Show("Ungültiger Name für Profil!", "Warnung!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public string profileName
        {
            get { return ret; }
        }

        private void ProfilenameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OKButton_Click(sender, e);
            }
        }
    }
}
