using Logic.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace UI.Desktop.Profile
{
    /// <summary>
    /// Interaction logic for ProfilWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        MainViewModel mainViewModel;
        ProfileHandler profileHandler = new ProfileHandler();
        Hasher hasher = new Hasher();

        bool controlGotFocus = false;

        public ProfileWindow(MainViewModel _mainViewModel)
        {
            mainViewModel = _mainViewModel;
            InitializeComponent();
            initCountryList();
            initBankNameList();
            buildProfileList();

            if (ProfileListBox.HasItems)
            {
                loadSelectedProfile();
            }
        }

        private void initCountryList()
        { 
            foreach (string culture in CountryList.getCountryList())
            {
                ProfileCountryListComboBox.Items.Add(culture);
            }
        }
        private void initBankNameList()
        {
            foreach (string bankName in BankList.getBankNameList())
            {
                ProfileBankListComboBox.Items.Add(bankName);
            }
        }

        private void buildProfileList()
        {
            string[] profiles = File.ReadAllLines(MainViewModel.profilePath);

            if (profiles.Length > 0)
            {
                foreach (string profile in profiles)
                {
                    ListBoxItem listboxItem = new ListBoxItem();
                    string[] data = profile.Split(';');
                    listboxItem.Name = hasher.decrypt(data[0], MainViewModel.profilePath).Replace(" ", "") + "ListBoxItem"; ;
                    listboxItem.Content = hasher.decrypt(data[0], MainViewModel.profilePath);

                    ProfileListBox.Items.Add(listboxItem);
                }
            }
        }

        private void loadSelectedProfile()
        {
            ConfigHandler configHanlder = new ConfigHandler();
            string selectedProfile = configHanlder.read(mainViewModel, "selectedProfile=");

            if (!string.IsNullOrEmpty(selectedProfile))
            {
                ListBoxItem listBoxItem = mainViewModel.getSelectedProfile(ProfileListBox, selectedProfile);

                if (listBoxItem != null)
                {
                    ProfileListBox.SelectedItem = listBoxItem;
                    ProfileListBox.Focus();
                }
            }
        }

        private void addNewItem(string _name)
        {
            ListBoxItem listBoxItem = new ListBoxItem();
            bool isDuplicate = false;

            listBoxItem.Name = _name.Replace(" ", "") + "ListBoxItem";

            foreach (ListBoxItem currentListBoxItem in ProfileListBox.Items)
            {
                if (currentListBoxItem.Name == listBoxItem.Name)
                {
                    isDuplicate = true;
                    break;
                }
            }

            if (!isDuplicate)
            {
                listBoxItem.Content = _name;
                ProfileListBox.Items.Add(listBoxItem);
                profileHandler.add(_name, false);
                ProfileListBox.Items.Refresh();
                ProfileListBox.SelectedItem = listBoxItem;

                if (int.TryParse(CountryList.getCountryIndex().ToString(), out int index))
                    ProfileCountryListComboBox.SelectedIndex = index;
            }
            else
            {
                MessageBox.Show("Dieses Profil existiert bereits!", "Fehler!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileInputDialog profileInputDialog = new ProfileInputDialog();
            profileInputDialog.Owner = this;
            string profilename = null;

            if (profileInputDialog.ShowDialog() == false)
                profilename = profileInputDialog.profileName;

            if (!string.IsNullOrEmpty(profilename))
                if (Regex.IsMatch(profilename, @"^[\w\- ]+$"))
                    addNewItem(profilename);
        }

        private void RemoveProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProfileListBox.SelectedIndex != -1)
            {
                MessageBoxResult result = MessageBox.Show("Möchten Sie das ausgewählte Item wirklich löschen?", "Warnung!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ListBoxItem listBoxItem = (ListBoxItem)ProfileListBox.SelectedItem;

                    if (SelectedProfileCheckBox.IsChecked == true)
                    {
                        ConfigHandler configHanlder = new ConfigHandler();
                        configHanlder.edit("selectedProfile=", "");
                        SelectedProfileCheckBox.IsChecked = false;
                    }

                    profileHandler.remove((string)listBoxItem.Content);
                    ProfileListBox.Items.Remove(ProfileListBox.SelectedItem);

                    if (ProfileListBox.HasItems)
                    {
                        ProfileListBox.SelectedItem = ProfileListBox.Items.GetItemAt(0);
                        ProfileListBox.Focus();
                    }
                    else
                    {
                        string[] data = new string[18];
                        ProfileListBox.SelectedIndex = -1;
                        loadProfileInfo(data);
                        SelectedProfileCheckBox.IsChecked = false;
                    }
                }
            }
        }

        private void ProfileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProfileListBox.SelectedItem != null)
            {
                ListBoxItem listBoxItem = (ListBoxItem)ProfileListBox.SelectedItem;
                string[] data = profileHandler.read((string)listBoxItem.Content, MainViewModel.profilePath);
                ProfileTabControl.IsEnabled = true;
                SaveProfileButton.IsEnabled = true;
                ActivateProfileButton.IsEnabled = true;

                if (data.Length > 0)
                {
                    ConfigHandler configHandler = new ConfigHandler();

                    string selectedProfile = configHandler.read(mainViewModel, "selectedProfile=");
                    loadProfileInfo(data);

                    if (!string.IsNullOrEmpty(selectedProfile))
                    {
                        if (selectedProfile == hasher.decrypt(data[0], MainViewModel.profilePath))
                        {
                            SelectedProfileCheckBox.IsChecked = true;
                        }
                        else
                        {
                            SelectedProfileCheckBox.IsChecked = false;
                        }
                    }
                }
            }
            else
            {
                ProfileTabControl.IsEnabled = false;
                SelectedProfileCheckBox.IsChecked = false;
                SaveProfileButton.IsEnabled = false;
                ActivateProfileButton.IsEnabled = false;
            }
        }

        private void loadProfileInfo(string[] _data)
        {
            ProfileCompanyNameTextBox.Text = hasher.decrypt(_data[1], MainViewModel.profilePath);
            ProfileFirstNameTextBox.Text = hasher.decrypt(_data[2], MainViewModel.profilePath);
            ProfileLastNameTextBox.Text = hasher.decrypt(_data[3], MainViewModel.profilePath);
            ProfilePostalCodeTextBox.Text = hasher.decrypt(_data[4], MainViewModel.profilePath);
            ProfileCityNameTextBox.Text = hasher.decrypt(_data[5], MainViewModel.profilePath);
            ProfileAddressTextBox.Text = hasher.decrypt(_data[6], MainViewModel.profilePath);
            ProfileAddressNumberTextBox.Text = hasher.decrypt(_data[7], MainViewModel.profilePath);

            if (int.TryParse(hasher.decrypt(_data[8], MainViewModel.profilePath), out int index))
                ProfileCountryListComboBox.SelectedIndex = index;
            else
                ProfileCountryListComboBox.SelectedIndex = -1;

            ProfileEMailTextBox.Text = hasher.decrypt(_data[9], MainViewModel.profilePath);
            ProfileTelephoneNumberTextBox.Text = hasher.decrypt(_data[10], MainViewModel.profilePath);
            ProfileMobileNumberTextBox.Text = hasher.decrypt(_data[11], MainViewModel.profilePath);
            ProfileFaxNumberTextBox.Text = hasher.decrypt(_data[12], MainViewModel.profilePath);

            if (int.TryParse(hasher.decrypt(_data[13], MainViewModel.profilePath), out index))
                ProfileBankListComboBox.SelectedIndex = index;
            else
                ProfileBankListComboBox.SelectedIndex = -1;

            ProfileBankCodeNumberTextBox.Text = hasher.decrypt(_data[14], MainViewModel.profilePath);
            ProfileBICTextBox.Text = hasher.decrypt(_data[15], MainViewModel.profilePath);
            ProfileBankAccountNumberTextBox.Text = hasher.decrypt(_data[16], MainViewModel.profilePath);
            ProfileIBANTextBox.Text = hasher.decrypt(_data[17], MainViewModel.profilePath);
        }

        private void SaveProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)ProfileListBox.SelectedItem;

            profileHandler.write(mainViewModel, listBoxItem);
        }

        public MainViewModel getProfileViewModel()
        {
            return mainViewModel;
        }

        private void Control_MouseEnter(object sender, RoutedEventArgs e)
        {
            Control control = sender as Control;

            if (!controlGotFocus)
            {
                TooltipLabel.Content = control.ToolTip;
            }
        }

        private void Control_MouseLeave(object sender, RoutedEventArgs e)
        {
            if (!controlGotFocus)
            {
                TooltipLabel.Content = "";
            }
        }

        private void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            Control control = sender as Control;

            controlGotFocus = true;
            TooltipLabel.Content = control.ToolTip;
        }

        private void Control_LostFocus(object sender, RoutedEventArgs e)
        {
            controlGotFocus = false;
            TooltipLabel.Content = "";
        }

        private void SelectedProfile_Checked(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedListBoxItem = (ListBoxItem)ProfileListBox.SelectedItem;
            ConfigHandler configHandler = new ConfigHandler();

            configHandler.edit("selectedProfile=", hasher.encrypt((string)selectedListBoxItem.Content, MainViewModel.profilePath));
        }

        private void ActivateProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedProfileCheckBox.IsChecked == false)
            {
                SelectedProfileCheckBox.IsChecked = true;
            }
        }

        private void ProfileCompanyNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileCompanyName = ProfileCompanyNameTextBox.Text;
        }

        private void ProfileFirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileFirstName = ProfileFirstNameTextBox.Text;
        }

        private void ProfileLastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileLastName = ProfileLastNameTextBox.Text;
        }

        private void ProfilePostalCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profilePostalCode = ProfilePostalCodeTextBox.Text;
        }

        private void ProfileCityNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileCityName = ProfileCityNameTextBox.Text;
        }

        private void ProfileAddressTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileAddress = ProfileAddressTextBox.Text;
        }

        private void ProfileAddressNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileAddressNumber = ProfileAddressNumberTextBox.Text;
        }

        private void ProfileEMailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileEMailAddress = ProfileEMailTextBox.Text;
        }

        private void ProfileTelephoneNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileTelephoneNumber = ProfileTelephoneNumberTextBox.Text;
        }

        private void ProfileMobileNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileMobileNumber = ProfileMobileNumberTextBox.Text;
        }

        private void ProfileFaxNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileFaxNumber = ProfileFaxNumberTextBox.Text;
        }

        private void ProfileIBANTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileIBAN = ProfileIBANTextBox.Text;
        }

        private void ProfileBICTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileBIC = ProfileBICTextBox.Text;
        }

        private void ProfileCountryListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainViewModel.profileCountryIndex = ProfileCountryListComboBox.SelectedIndex.ToString();
        }

        private void ProfileBankAccountNumberTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            char[] key = e.Key.ToString().ToCharArray();

            if (!char.IsControl(key[key.Length - 1]) && !char.IsDigit(key[key.Length - 1]) && (key[key.Length - 1] != '.'))
            {
                e.Handled = true;
            }
        }

        private void ProfileBankAccountNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileBankAccountNumber = ProfileBankAccountNumberTextBox.Text;

            if (ProfileBankAccountNumberTextBox.Text.Length == ProfileBankAccountNumberTextBox.MaxLength && ProfileCountryListComboBox.SelectedIndex != -1)
            {
                ProfileIBANTextBox.Text = IBANBuilder.formatIBAN(IBANBuilder.getIBAN(ProfileBankCodeNumberTextBox.Text, ProfileBankAccountNumberTextBox.Text));
            }
        }

        private void ProfileBankCodeNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileBankCodeNumber = ProfileBankCodeNumberTextBox.Text;
        }

        private void ProfileBankNameTextBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainViewModel.profileBankIndex = ProfileBankListComboBox.SelectedIndex.ToString();
            mainViewModel.profileBankName = ProfileBankListComboBox.Text;
        }

        private void loadBankInformation()
        {
            List<string> bankInformation = BankList.getBankInformation(ProfileBankListComboBox.Text);

            if (bankInformation != null)
                if (bankInformation.Count == 2)
                {
                    ProfileBankCodeNumberTextBox.Text   = bankInformation[0];
                    ProfileBICTextBox.Text              = bankInformation[1];

                    if (ProfileBankAccountNumberTextBox.Text.Length == ProfileBankAccountNumberTextBox.MaxLength && ProfileCountryListComboBox.SelectedIndex != -1)
                    {
                        ProfileIBANTextBox.Text = IBANBuilder.formatIBAN(IBANBuilder.getIBAN(ProfileBankCodeNumberTextBox.Text, ProfileBankAccountNumberTextBox.Text));
                    }
                }
        }
        private void ProfileBankListComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Tab)
            {
                loadBankInformation();
            }
        }
    }
}