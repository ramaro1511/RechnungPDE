using System.Windows;
using System.Windows.Controls;
using Logic.UI;

namespace UI.Desktop.Main.Pages
{
    /// <summary>
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        MainWindow mainWindow;
        Hasher hasher = new Hasher();

        public CustomerPage()
        {
            InitializeComponent();
        }

        public void setMainWindow(MainWindow _mainWindow)
        {
            mainWindow = _mainWindow;
        }

        private void CustomerProfileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerProfileListBox.SelectedIndex != -1)
            {
                ListBoxItem selectedItem = (ListBoxItem)CustomerProfileListBox.SelectedItem;
                string[] data = mainWindow.profileHandler.read((string)selectedItem.Content, mainWindow.mainViewModel.customerPath);

                if (data.Length > 0)
                {
                    CustomerCompanyNameTextBox.Text = hasher.decrypt(data[1]);
                    FirstNameTextBox.Text = hasher.decrypt(data[2]);
                    LastNameTextBox.Text = hasher.decrypt(data[3]);
                    PostCodeTextBox.Text = hasher.decrypt(data[4]);
                    CityTextBox.Text = hasher.decrypt(data[5]);
                    Address.Text = hasher.decrypt(data[6]);
                    HouseNumberTextBox.Text = hasher.decrypt(data[7]);
                    CountryTextBox.Text = hasher.decrypt(data[8]);
                    mainWindow.invoiceInfoPage.CustomerNumberTextBox.Text = hasher.decrypt(data[9]);
                }
            }
        }

        private void RemoveProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerProfileListBox.SelectedIndex != -1)
            {
                MessageBoxResult result = MessageBox.Show("Möchten Sie das ausgewählte Item wirklich löschen?", "Warnung!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ListBoxItem listBoxItem = (ListBoxItem)CustomerProfileListBox.SelectedItem;

                    mainWindow.profileHandler.remove((string)listBoxItem.Content, mainWindow.mainViewModel.customerPath);
                    CustomerProfileListBox.Items.Remove(CustomerProfileListBox.SelectedItem);
                    CustomerProfileListBox.SelectedItem = -1;
                    CustomerCompanyNameTextBox.Text = "";
                    FirstNameTextBox.Text = "";
                    LastNameTextBox.Text = "";
                    PostCodeTextBox.Text = "";
                    CityTextBox.Text = "";
                    Address.Text = "";
                    HouseNumberTextBox.Text = "";
                    CountryTextBox.Text = "";
                }
            }
        }

        private void CustomerCompanyNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.customerCompanyName = CustomerCompanyNameTextBox.Text;

            if (mainWindow.invoiceInfoPage.FileNameCheckBox.IsChecked == false)
                mainWindow.buildFileName(sender, e);
        }

        private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.customerFirstName = FirstNameTextBox.Text;
        }

        private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.customerLastName = LastNameTextBox.Text;
        }

        private void PostCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.customerPostalCode = PostCodeTextBox.Text;
        }

        private void CityTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.customerCityName = CityTextBox.Text;
        }

        private void AddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.customerAddress = Address.Text;
        }

        private void HouseNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.customerAddressNumber = HouseNumberTextBox.Text;
        }

        private void CountryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainWindow.mainViewModel.customerCountryName = CountryTextBox.Text;
        }


        private void Control_MouseEnter(object sender, RoutedEventArgs e)
        {
            Control control = sender as Control;

            if (!mainWindow.controlGotFocus)
            {
                mainWindow.TooltipLabel.Content = control.ToolTip;
            }
        }

        private void Control_MouseLeave(object sender, RoutedEventArgs e)
        {
            if (!mainWindow.controlGotFocus)
            {
                mainWindow.TooltipLabel.Content = "";
            }
        }

        public void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox control = sender as TextBox;

            mainWindow.controlGotFocus = true;

            if (control.ToolTip != null)
                mainWindow.TooltipLabel.Content = control.ToolTip;
        }

        public void Control_LostFocus(object sender, RoutedEventArgs e)
        {
            mainWindow.controlGotFocus = false;
            mainWindow.TooltipLabel.Content = "";
        }
    }
}
