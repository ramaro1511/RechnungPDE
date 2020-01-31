using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace Logic.UI
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>

        ProfileHandler profileHandler = new ProfileHandler();
        Hasher hasher = new Hasher();

        public static string customerPath = Directory.GetCurrentDirectory() + @"\customerProfile.txt";
        public static string invoicePath = Directory.GetCurrentDirectory() + @"\invoiceNumbers.txt";
        public static string materialPath = Directory.GetCurrentDirectory() + @"\materialList.txt"; 
        public static string profilePath = Directory.GetCurrentDirectory() + @"\profile.txt";
        public static string configPath = Directory.GetCurrentDirectory() + @"\config.txt";

        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                WindowTitle = "Hausservice Amaro (Design)";
            }
            else
            {
                WindowTitle = "Hausservice Amaro";
            }

            MainColor = "#FFC5C5C5";
            OffColor = "#FFE4E4E4";
            ToolBarColor = "#FFEEF5FD";
        }

        public void checkFiles()
        {
            if (!File.Exists(customerPath))
            {
                File.WriteAllText(customerPath, "");
            }

            if (!File.Exists(invoicePath))
            {
                File.WriteAllText(invoicePath, "");
            }

            if (!File.Exists(materialPath))
            {
                File.WriteAllText(materialPath, "");
            }

            if (!File.Exists(profilePath))
            {
                File.WriteAllText(profilePath, "");
            }

            if (!File.Exists(configPath))
            {
                File.WriteAllText(configPath, "");

                string[] config = { "selectedProfile=", "invoiceStandardFont=Times New Roman", "invoiceTableFont=Consolas" };

                foreach (string line in config)
                {
                    if (!string.IsNullOrEmpty(File.ReadAllText(configPath)))
                        File.AppendAllText(configPath, Environment.NewLine);

                    File.AppendAllText(configPath, line);
                }
            }
        }

        public string WindowTitle { get; private set; }

        public string MainColor { get; private set; }

        public string OffColor { get; private set; }

        public string ToolBarColor { get; private set; }

        public ObservableCollection<Material> MaterialList { get; set; }

        public ObservableCollection<CountryList> CountryList { get; set; }

        public string customerCompanyName { get; set; }

        public string customerFirstName { get; set; }

        public string customerLastName { get; set; }

        public string customerPostalCode { get; set; }

        public string customerCityName { get; set; }

        public string customerAddress { get; set; }

        public string customerAddressNumber { get; set; }

        public string customerCountryName { get; set; }

        public string invoiceNumber { get; set; }

        public string invoiceDate { get; set; }

        public string customerNumber { get; set; }

        public string fileName { get; set; }

        public string materialDescription { get; set; }

        public int materialCount { get; set; }

        public decimal materialPrice { get; set; }

        public decimal businessHours { get; set; }

        public decimal hourlyWage { get; set; }

        public string invoiceText { get; set; }

        public string invoiceComment { get; set; }

        public decimal assistentBusinessHours { get; set; }

        public decimal assistentWage { get; set; }

        public bool assistentIsEnabled { get; set; }

        public bool assistent2IsEnabled { get; set; }

        public decimal assistent2BusinessHours { get; set; }

        public decimal assistent2Wage { get; set; }

        public string profileCompanyName { get; set; }

        public string profileFirstName { get; set; }

        public string profileLastName { get; set; }

        public string profilePostalCode { get; set; }

        public string profileCityName { get; set; }

        public string profileAddress { get; set; }

        public string profileAddressNumber { get; set; }

        public string profileCountryIndex { get; set; }

        public string profileEMailAddress { get; set; }

        public string profileTelephoneNumber { get; set; }

        public string profileMobileNumber { get; set; }

        public string profileFaxNumber { get; set; }

        public string profileBankIndex { get; set; }

        public string profileBankName { get; set; }

        public string profileBankAccountNumber { get; set; }

        public string profileBankCodeNumber { get; set; }

        public string profileIBAN { get; set; }

        public string profileBIC { get; set; }

        public int switchPage(int _page, Button _button, Button _continueButton)
        {
            if (_button.Name == _continueButton.Name)
            {
                _page++;
            }
            else
            {
                if (_page > 0)
                {
                    _page--;
                }
            }

            return _page;
        }

        public void saveCustomerProfiles()
        {
            if (!string.IsNullOrWhiteSpace(customerCompanyName))
            {
                string[] data = profileHandler.read(customerCompanyName, MainViewModel.customerPath);

                if (data == null)
                    profileHandler.add(customerCompanyName, true);

                profileHandler.writeCustomer(this);
            }
        }

        public void saveInvoiceNumber()
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string linesText = File.ReadAllText(invoicePath);
                string[] lines = File.ReadAllLines(invoicePath);
                string newInvoiceNumbers = null;

                if (!string.IsNullOrEmpty(linesText))
                {
                    int lineCount = 0;
                    int maximumLines = 14;

                    foreach (string line in lines)
                    {
                        if (string.IsNullOrEmpty(newInvoiceNumbers))
                        {
                            newInvoiceNumbers = hasher.encrypt(fileName, invoicePath);
                            lineCount++;
                        }

                        newInvoiceNumbers += "\n" + line;
                        lineCount++;

                        if (lineCount >= maximumLines)
                        {
                            break;
                        }
                    }

                    File.WriteAllText(invoicePath, newInvoiceNumbers);
                }
                else
                {
                    File.AppendAllText(invoicePath, hasher.encrypt(fileName, invoicePath));
                }
            }
        }

        public void addMaterialToList(string _path)
        {
            string[] lines = File.ReadAllLines(_path);
            string newLines = null;
            bool lineReplaced = false;

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                if (!string.IsNullOrEmpty(newLines))
                {
                    newLines += "\n";
                }

                if (data[0] == hasher.encrypt(materialDescription, materialPath))
                {
                    data[1] = hasher.encrypt(materialCount.ToString(), materialPath);
                    data[2] = hasher.encrypt(materialPrice.ToString(), materialPath);

                    newLines += string.Join(";", data);
                    lineReplaced = true;
                }
                else
                {
                    newLines += line;
                }
            }

            if (!lineReplaced)
            {
                if (!string.IsNullOrEmpty(newLines))
                {
                    newLines += "\n";
                }

                newLines += hasher.encrypt(materialDescription, materialPath) + ";" + hasher.encrypt(materialCount.ToString(), materialPath) + ";" + hasher.encrypt(materialPrice.ToString(), materialPath) + ";";
            }

            File.WriteAllText(_path, newLines);
        }

        public void removeMaterialFromList(string _description, string _path)
        {
            string[] lines = File.ReadAllLines(_path);
            string newLines = null;

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                if (data[0] != _description)
                {
                    if (!string.IsNullOrEmpty(newLines))
                    {
                        newLines += "\n";
                    }

                    newLines += line;
                }
            }

            File.WriteAllText(_path, newLines);
        }

        public ListBoxItem getSelectedProfile(ListBox _profileListbox, string _line)
        {
            foreach (ListBoxItem listBoxItem in _profileListbox.Items)
            {
                if (_line.Contains((string)listBoxItem.Content))
                {
                    return listBoxItem;
                }
            }

            return null;
        }

        public string getSelectedProfile(string _profile, string _path)
        {
            if (File.Exists(_path))
            {
                string[] lines = File.ReadAllLines(_path);

                foreach (string line in lines)
                {
                    string[] data = line.Split(';');

                    if (_profile.Contains(hasher.decrypt(data[0], profilePath)))
                    {
                        return line;
                    }
                }
            }

            return null;
        }
    }
}