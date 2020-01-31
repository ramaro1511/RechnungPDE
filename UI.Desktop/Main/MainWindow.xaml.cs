using Logic.UI;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using UI.Desktop.Main.Pages;
using UI.Desktop.Options;
using UI.Desktop.Profile;
using System.IO;
using System;

namespace UI.Desktop.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel mainViewModel = new MainViewModel();
        public ProfileHandler profileHandler = new ProfileHandler();
        StartPage startPage = new StartPage();
        public CustomerPage customerPage = new CustomerPage();
        public InvoiceInfoPage invoiceInfoPage = new InvoiceInfoPage();
        public MaterialPage materialPage = new MaterialPage();
        public WorkingHoursPage workingHoursPage = new WorkingHoursPage();
        CompletionPage completionPage = new CompletionPage();
        Hasher hasher = new Hasher();

        public bool controlGotFocus;
        public static bool isValidFilename, isValidCustomerCompanyName, isValidInvoiceNumber;
        int page = 0;

        public MainWindow()
        {
            InitializeComponent();
            buildLists();
            getSelectedProfile();

            customerPage.setMainWindow(this);
            invoiceInfoPage.setMainWindow(this);
            invoiceInfoPage.setUp();
            materialPage.setMainWindow(this);
            workingHoursPage.setMainWindow(this);
            workingHoursPage.setUp();

            mainViewModel.MaterialList = new System.Collections.ObjectModel.ObservableCollection<Material>();


            Main.Content = startPage;
        }

        private void buildLists()
        {
            mainViewModel.checkFiles();
            buildCustomerList();
            buildInvoiceList();
            buildMaterialList();
        }

        private void buildCustomerList()
        {
            string text = File.ReadAllText(MainViewModel.customerPath);

            if (!string.IsNullOrWhiteSpace(text))
            {
                string[] profiles = text.Split('\n');

                foreach (string profile in profiles)
                {
                    ListBoxItem listboxItem = new ListBoxItem();
                    string[] data = profile.Split(';');
                    listboxItem.Name = hasher.decrypt(data[0], MainViewModel.customerPath).Replace(" ", "") + "ListBoxItem";
                    listboxItem.Content = hasher.decrypt(data[0], MainViewModel.customerPath);

                    customerPage.CustomerProfileListBox.Items.Add(listboxItem);
                }
            }
        }

        private void buildInvoiceList()
        {
            string[] lines = File.ReadAllLines(MainViewModel.invoicePath);

            if (lines != null)
            {
                foreach (string line in lines)
                {
                    ListBoxItem listBoxItem = new ListBoxItem();
                    listBoxItem.Content = hasher.decrypt(line, MainViewModel.invoicePath);
                    invoiceInfoPage.InvoiceListBox.Items.Add(listBoxItem);
                }
            }
        }

        public void buildMaterialList(bool rebuild = false)
        {
            string[] lines = File.ReadAllLines(MainViewModel.materialPath);

            if (lines.Length > 0)
            {
                if (rebuild)
                {
                    materialPage.MaterialListBox.Items.Clear();
                }

                foreach (string line in lines)
                {
                    string[] data = line.Split(';');

                    ListBoxItem listBoxItem = new ListBoxItem();
                    listBoxItem.Content = hasher.decrypt(data[0], MainViewModel.materialPath);
                    materialPage.MaterialListBox.Items.Add(listBoxItem);
                }
            }
        }

        private void getSelectedProfile()
        {
            ConfigHandler configHandler = new ConfigHandler();
            string selectedProfile = configHandler.read(mainViewModel, "selectedProfile=");

            if (!string.IsNullOrEmpty(selectedProfile))
            {
                string profile = mainViewModel.getSelectedProfile(selectedProfile, MainViewModel.profilePath);

                if (!string.IsNullOrEmpty(profile))
                {
                    string[] data = profile.Split(';');

                    mainViewModel.profileCompanyName = hasher.decrypt(data[1], MainViewModel.profilePath);
                    mainViewModel.profileFirstName = hasher.decrypt(data[2], MainViewModel.profilePath);
                    mainViewModel.profileLastName = hasher.decrypt(data[3], MainViewModel.profilePath);
                    mainViewModel.profilePostalCode = hasher.decrypt(data[4], MainViewModel.profilePath);
                    mainViewModel.profileCityName = hasher.decrypt(data[5], MainViewModel.profilePath);
                    mainViewModel.profileAddress = hasher.decrypt(data[6], MainViewModel.profilePath);
                    mainViewModel.profileAddressNumber = hasher.decrypt(data[7], MainViewModel.profilePath);
                    mainViewModel.profileCountryIndex = hasher.decrypt(data[8], MainViewModel.profilePath);
                    mainViewModel.profileEMailAddress = hasher.decrypt(data[9], MainViewModel.profilePath);
                    mainViewModel.profileTelephoneNumber = hasher.decrypt(data[10], MainViewModel.profilePath);
                    mainViewModel.profileMobileNumber = hasher.decrypt(data[11], MainViewModel.profilePath);
                    mainViewModel.profileFaxNumber = hasher.decrypt(data[12], MainViewModel.profilePath);
                    mainViewModel.profileBankName = hasher.decrypt(data[13], MainViewModel.profilePath);
                    mainViewModel.profileBankAccountNumber = hasher.decrypt(data[14], MainViewModel.profilePath);
                    mainViewModel.profileBankCodeNumber = hasher.decrypt(data[15], MainViewModel.profilePath);
                    mainViewModel.profileIBAN = hasher.decrypt(data[16], MainViewModel.profilePath);
                    mainViewModel.profileBIC = hasher.decrypt(data[17], MainViewModel.profilePath);
                }
            }
        }

        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;

            if (overflowGrid != null)
                overflowGrid.Visibility = Visibility.Collapsed;

            if (mainPanelBorder != null)
                mainPanelBorder.Margin = new Thickness();
        }

        private void ProfileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow(mainViewModel);
            profileWindow.Owner = this;

            if (profileWindow.ShowDialog() == false)
            {
                if (profileWindow.getProfileViewModel() != null)
                {
                    mainViewModel = profileWindow.getProfileViewModel();
                }
            }
        }

        private void OptionsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionWindow = new OptionsWindow();

            if (optionWindow.ShowDialog() == false)
            {

            }
        }

        private void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int previousPage = page;

            page = mainViewModel.switchPage(page, button, ContinueButton);

            switch (page)
            {
                case 0:
                    Main.Content = startPage;
                    BackButton.Visibility = Visibility.Hidden;
                    break;

                case 1:
                    Main.Content = customerPage;
                    BackButton.Visibility = Visibility.Visible;
                    break;

                case 2:
                    Main.Content = invoiceInfoPage;

                    if (!isValidFilename || !isValidCustomerCompanyName ||  !isValidInvoiceNumber)
                        ContinueButton.IsEnabled = false;

                    break;

                case 3:
                    Main.Content = materialPage;

                    if (!ContinueButton.IsEnabled)
                        ContinueButton.IsEnabled = true;

                    break;

                case 4:
                    Main.Content = workingHoursPage;

                    ContinueButton.Visibility = Visibility.Visible;
                    CompleteButton.Visibility = Visibility.Hidden;

                    if (!WorkingHoursPage.isValidBusinessHours || !WorkingHoursPage.isValidWage)
                        ContinueButton.IsEnabled = false;
                    break;

                case 5:
                    Main.Content = completionPage;
                    ContinueButton.Visibility = Visibility.Hidden;
                    CompleteButton.Visibility = Visibility.Visible;
                    break;

                default:
                    break;
            }

            if (page != 2 && page != 4)
            {
                ContinueButton.IsEnabled = true;
            }
        }

        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            InvoiceBuilder invoiceBuilder = new InvoiceBuilder();

            mainViewModel.saveCustomerProfiles();
            mainViewModel.saveInvoiceNumber();

            if (invoiceBuilder.build(mainViewModel))
            {
                Close();
            }
        }

        public void buildFileName(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(invoiceInfoPage.InvoiceNumberTextBox.Text) || string.IsNullOrWhiteSpace(customerPage.CustomerCompanyNameTextBox.Text))
            {
                invoiceInfoPage.FileNameTextBox.Text = "";
            }
            else
            {
                invoiceInfoPage.FileNameTextBox.Text = invoiceInfoPage.InvoiceNumberTextBox.Text + "-" + customerPage.CustomerCompanyNameTextBox.Text;
            }
        }

        public void regexCheck()
        {
            if (!string.IsNullOrWhiteSpace(invoiceInfoPage.FileNameTextBox.Text) && Regex.IsMatch(invoiceInfoPage.FileNameTextBox.Text, @"^[\w\-. ]+$"))
                isValidFilename = true;
            else
                isValidFilename = false;

            if (!string.IsNullOrWhiteSpace(customerPage.CustomerCompanyNameTextBox.Text) && Regex.IsMatch(customerPage.CustomerCompanyNameTextBox.Text, @"^[\w\-. ]+$"))
                isValidCustomerCompanyName = true;
            else
                isValidCustomerCompanyName = false;

            if (!string.IsNullOrWhiteSpace(invoiceInfoPage.InvoiceNumberTextBox.Text) && Regex.IsMatch(invoiceInfoPage.InvoiceNumberTextBox.Text, @"^[\w\-. ]+$")) //test
                isValidInvoiceNumber = true;
            else
                isValidInvoiceNumber = false;

            if (page == 2 && isValidFilename && isValidCustomerCompanyName && isValidInvoiceNumber)
                ContinueButton.IsEnabled = true;
            else
                ContinueButton.IsEnabled = false;
        }
    }
}