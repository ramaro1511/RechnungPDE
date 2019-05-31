using Logic.UI;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

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
            buildProfileList();

            if (ProfileListBox.HasItems)
            {
                loadSelectedProfile();
            }
        }

        private void buildProfileList()
        {
            string[] profiles = File.ReadAllLines(mainViewModel.profilePath);

            if (profiles.Length > 0)
            {
                foreach (string profile in profiles)
                {
                    ListBoxItem listboxItem = new ListBoxItem();
                    string[] data = profile.Split(';');
                    listboxItem.Name = hasher.decrypt(data[0]).Replace(" ", "") + "ListBoxItem"; ;
                    listboxItem.Content = hasher.decrypt(data[0]);

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
                profileHandler.add(_name, mainViewModel.profilePath, false);
                ProfileListBox.Items.Refresh();
                ProfileListBox.SelectedItem = listBoxItem;
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
                        configHanlder.edit(mainViewModel, "selectedProfile=", "");
                        SelectedProfileCheckBox.IsChecked = false;
                    }

                    profileHandler.remove((string)listBoxItem.Content, mainViewModel.profilePath);
                    ProfileListBox.Items.Remove(ProfileListBox.SelectedItem);

                    if (ProfileListBox.HasItems)
                    {
                        ProfileListBox.SelectedItem = ProfileListBox.Items.GetItemAt(0);
                        ProfileListBox.Focus();
                    }
                    else
                    {
                        string[] data = new string[9];
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
                string[] data = profileHandler.read((string)listBoxItem.Content, mainViewModel.profilePath);
                ProfileTabControl.IsEnabled = true;

                if (data.Length > 0)
                {
                    ConfigHandler configHandler = new ConfigHandler();

                    string selectedProfile = configHandler.read(mainViewModel, "selectedProfile");
                    loadProfileInfo(data);

                    if (!string.IsNullOrEmpty(selectedProfile))
                    {
                        if (selectedProfile == hasher.decrypt(data[0]))
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
            }
        }

        private void loadProfileInfo(string[] _data)
        {
            ProfileCompanyNameTextBox.Text = hasher.decrypt(_data[1]);
            ProfileFirstNameTextBox.Text = hasher.decrypt(_data[2]);
            ProfileLastNameTextBox.Text = hasher.decrypt(_data[3]);
            ProfilePostalCodeTextBox.Text = hasher.decrypt(_data[4]);
            ProfileCityNameTextBox.Text = hasher.decrypt(_data[5]);
            ProfileAddressTextBox.Text = hasher.decrypt(_data[6]);
            ProfileAddressNumberTextBox.Text = hasher.decrypt(_data[7]);
            ProfileCountryNameTextBox.Text = hasher.decrypt(_data[8]);
            ProfileEMailTextBox.Text = hasher.decrypt(_data[9]);
            ProfileTelephoneNumberTextBox.Text = hasher.decrypt(_data[10]);
            ProfileMobileNumberTextBox.Text = hasher.decrypt(_data[11]);
            ProfileFaxNumberTextBox.Text = hasher.decrypt(_data[12]);
            ProfileBankNameTextBox.Text = hasher.decrypt(_data[13]);
            ProfileBankAccountNumberTextBox.Text = hasher.decrypt(_data[14]);
            ProfileBankCodeNumberTextBox.Text = hasher.decrypt(_data[15]);
            ProfileIBANTextBox.Text = hasher.decrypt(_data[16]);
            ProfileBICTextBox.Text = hasher.decrypt(_data[17]);
        }

        private void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)ProfileListBox.SelectedItem;

            profileHandler.write(mainViewModel, mainViewModel.profilePath, listBoxItem);
        }

        public MainViewModel getProfileViewModel()
        {
            return mainViewModel;
        }

        private void TextBox_MouseEnter(object sender, RoutedEventArgs e)
        {
            Control control = sender as Control;

            if (!controlGotFocus)
            {
                TooltipLabel.Content = control.ToolTip;
            }
        }

        private void TextBox_MouseLeave(object sender, RoutedEventArgs e)
        {
            if (!controlGotFocus)
            {
                TooltipLabel.Content = "";
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Control control = sender as Control;

            controlGotFocus = true;
            TooltipLabel.Content = control.ToolTip;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            controlGotFocus = false;
            TooltipLabel.Content = "";
        }

        private void SelectedProfile_Checked(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedListBoxItem = (ListBoxItem)ProfileListBox.SelectedItem;
            ConfigHandler configHandler = new ConfigHandler();

            configHandler.edit(mainViewModel, "selectedProfile=", hasher.encrypt((string)selectedListBoxItem.Content));
        }

        private void SelectProfile_Click(object sender, RoutedEventArgs e)
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

        private void ProfileCountryNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileCountryName = ProfileCountryNameTextBox.Text;
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

        private void ProfileBankNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileBankName = ProfileBankNameTextBox.Text;
        }

        private void ProfileBankAccountNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileBankAccountNumber = ProfileBankAccountNumberTextBox.Text;
        }

        private void ProfileBankCodeNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileBankCodeNumber = ProfileBankCodeNumberTextBox.Text;
        }

        private void ProfileIBANTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileIBAN = ProfileIBANTextBox.Text;
        }

        private void ProfileBICTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainViewModel.profileBIC = ProfileBICTextBox.Text;
        }
    }
}