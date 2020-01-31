using System;
using System.Collections.Generic;
using System.Globalization;
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
            initCountryList();
        }

        private void initCountryList()
        {
            List<string> cultureList = new List<string>();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            RegionInfo region;

            foreach (CultureInfo culture in cultures)
            {
                region = new RegionInfo(culture.LCID);

                if (!cultureList.Contains(region.DisplayName))
                {
                    cultureList.Add(region.DisplayName);
                }
            }

            cultureList.Sort();
            
            foreach (string culture in cultureList)
            {
                CountryListComboBox.Items.Add(culture);
            }

            //ComboBoxItem item = getCountryListBoxItem(RegionInfo.CurrentRegion.DisplayName);

            //if (item != null)
            //{
            //    CountryListComboBox.SelectedItem = item;
            //}
        }

        private ComboBoxItem getCountryListBoxItem(string _country)
        {
            foreach (ComboBoxItem item in CountryListComboBox.Items)
            {
                if (_country.Contains(_country))
                    return item;
            }

            return null;
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
                string[] data = mainWindow.profileHandler.read((string)selectedItem.Content, MainViewModel.customerPath);

                if (data.Length > 0)
                {
                    CustomerCompanyNameTextBox.Text = hasher.decrypt(data[1], MainViewModel.customerPath);
                    FirstNameTextBox.Text = hasher.decrypt(data[2], MainViewModel.customerPath);
                    LastNameTextBox.Text = hasher.decrypt(data[3], MainViewModel.customerPath);
                    PostCodeTextBox.Text = hasher.decrypt(data[4], MainViewModel.customerPath);
                    CityTextBox.Text = hasher.decrypt(data[5], MainViewModel.customerPath);
                    Address.Text = hasher.decrypt(data[6], MainViewModel.customerPath);
                    HouseNumberTextBox.Text = hasher.decrypt(data[7], MainViewModel.customerPath);
                    CountryListComboBox.Text = hasher.decrypt(data[8], MainViewModel.customerPath);
                    mainWindow.invoiceInfoPage.CustomerNumberTextBox.Text = hasher.decrypt(data[9], MainViewModel.customerPath);
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

                    mainWindow.profileHandler.remove((string)listBoxItem.Content);
                    CustomerProfileListBox.Items.Remove(CustomerProfileListBox.SelectedItem);
                    CustomerProfileListBox.SelectedItem = -1;
                    CustomerCompanyNameTextBox.Text = "";
                    FirstNameTextBox.Text = "";
                    LastNameTextBox.Text = "";
                    PostCodeTextBox.Text = "";
                    CityTextBox.Text = "";
                    Address.Text = "";
                    HouseNumberTextBox.Text = "";
                    CountryListComboBox.Text = "";
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

        public void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            Control control = sender as Control;

            mainWindow.controlGotFocus = true;

            if (control.ToolTip != null)
                mainWindow.TooltipLabel.Content = control.ToolTip;
        }

        public void Control_LostFocus(object sender, RoutedEventArgs e)
        {
            mainWindow.controlGotFocus = false;
            mainWindow.TooltipLabel.Content = "";
        }

        private void CountryListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainWindow.mainViewModel.customerCountryName = CountryListComboBox.Text;
        }
    }
}
