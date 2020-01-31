using System;
using System.IO;
using System.Windows.Controls;

namespace Logic.UI
{
    public class ProfileHandler
    {
        Hasher hasher = new Hasher();

        readonly int profileLength = 18;
        readonly int customerProfileLength = 10;

        public string[] read(string _profilename, string _path)
        {
            string[] lines = File.ReadAllLines(_path);

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                if (hasher.decrypt(data[0], _path) == _profilename)
                {
                    return data;
                }
            }

            return null;
        }

        public void writeCustomer(MainViewModel _mainViewModel)
        {
            string[] lines = File.ReadAllLines(MainViewModel.customerPath);

            File.WriteAllText(MainViewModel.customerPath, "");

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                if (!string.IsNullOrEmpty(File.ReadAllText(MainViewModel.customerPath)))
                    File.AppendAllText(MainViewModel.customerPath, Environment.NewLine);

                if (hasher.decrypt(data[0], MainViewModel.customerPath) == _mainViewModel.customerCompanyName)
                {
                    data[0] = hasher.encrypt(_mainViewModel.customerCompanyName, MainViewModel.customerPath);
                    data[1] = hasher.encrypt(_mainViewModel.customerCompanyName, MainViewModel.customerPath);
                    data[2] = hasher.encrypt(_mainViewModel.customerFirstName, MainViewModel.customerPath);
                    data[3] = hasher.encrypt(_mainViewModel.customerLastName, MainViewModel.customerPath);
                    data[4] = hasher.encrypt(_mainViewModel.customerPostalCode, MainViewModel.customerPath);
                    data[5] = hasher.encrypt(_mainViewModel.customerCityName, MainViewModel.customerPath);
                    data[6] = hasher.encrypt(_mainViewModel.customerAddress, MainViewModel.customerPath);
                    data[7] = hasher.encrypt(_mainViewModel.customerAddressNumber, MainViewModel.customerPath);
                    data[8] = hasher.encrypt(_mainViewModel.customerCountryName, MainViewModel.customerPath);
                    data[9] = hasher.encrypt(_mainViewModel.customerNumber, MainViewModel.customerPath);

                    File.AppendAllText(MainViewModel.customerPath, string.Join(";", data));
                }
                else
                {
                    File.AppendAllText(MainViewModel.customerPath, line);
                }
            }
        }

        public void write(MainViewModel _mainViewModel, ListBoxItem _listBoxItem = null)
        {
            string[] lines = File.ReadAllLines(MainViewModel.profilePath);

            File.WriteAllText(MainViewModel.profilePath, "");

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                if (!string.IsNullOrWhiteSpace(File.ReadAllText(MainViewModel.profilePath)))
                    File.AppendAllText(MainViewModel.profilePath, Environment.NewLine);

                if (hasher.decrypt(data[0], MainViewModel.profilePath) == (string)_listBoxItem.Content)
                {
                    data[0] = hasher.encrypt((string)_listBoxItem.Content, MainViewModel.profilePath);
                    data[1] = hasher.encrypt(_mainViewModel.profileCompanyName, MainViewModel.profilePath);
                    data[2] = hasher.encrypt(_mainViewModel.profileFirstName, MainViewModel.profilePath);
                    data[3] = hasher.encrypt(_mainViewModel.profileLastName, MainViewModel.profilePath);
                    data[4] = hasher.encrypt(_mainViewModel.profilePostalCode, MainViewModel.profilePath);
                    data[5] = hasher.encrypt(_mainViewModel.profileCityName, MainViewModel.profilePath);
                    data[6] = hasher.encrypt(_mainViewModel.profileAddress, MainViewModel.profilePath);
                    data[7] = hasher.encrypt(_mainViewModel.profileAddressNumber, MainViewModel.profilePath);
                    data[8] = hasher.encrypt(_mainViewModel.profileCountryIndex, MainViewModel.profilePath); 
                    data[9] = hasher.encrypt(_mainViewModel.profileEMailAddress, MainViewModel.profilePath);
                    data[10] = hasher.encrypt(_mainViewModel.profileTelephoneNumber, MainViewModel.profilePath);
                    data[11] = hasher.encrypt(_mainViewModel.profileMobileNumber, MainViewModel.profilePath);
                    data[12] = hasher.encrypt(_mainViewModel.profileFaxNumber, MainViewModel.profilePath);
                    data[13] = hasher.encrypt(_mainViewModel.profileBankIndex, MainViewModel.profilePath);
                    data[14] = hasher.encrypt(_mainViewModel.profileBankCodeNumber, MainViewModel.profilePath);
                    data[15] = hasher.encrypt(_mainViewModel.profileBIC, MainViewModel.profilePath);
                    data[16] = hasher.encrypt(_mainViewModel.profileBankAccountNumber, MainViewModel.profilePath);
                    data[17] = hasher.encrypt(_mainViewModel.profileIBAN, MainViewModel.profilePath);
                    

                    File.AppendAllText(MainViewModel.profilePath, string.Join(";", data));
                }
                else
                {
                    File.AppendAllText(MainViewModel.profilePath, line);
                }
            }
        }

        public void add(string _name, bool _isCustomerProfile)
        {
            checkForFile();

            int pLength;
            string path;

            if (_isCustomerProfile)
            {
                pLength = customerProfileLength;
                path = MainViewModel.customerPath;
            }
            else
            {
                pLength = profileLength;
                path = MainViewModel.profilePath;
            }

            string[] newProfileLines = new string[pLength];

            for (int i = 0; i < pLength; i++)
            {
                if (i == 0)
                    newProfileLines[i] += hasher.encrypt(_name, path);
                else 
                    if (i == 8)
                        newProfileLines[i] = hasher.encrypt(CountryList.getCountryIndex().ToString(), path);
                    else
                        newProfileLines[i] = "";
            }

            if (!string.IsNullOrEmpty(File.ReadAllText(path)))
                File.AppendAllText(path, "\n");


            File.AppendAllText(path, string.Join(";", newProfileLines));
        }

        public void remove(string _profilename)
        {
            checkForFile();

            string[] lines = File.ReadAllLines(MainViewModel.profilePath);

            File.WriteAllText(MainViewModel.profilePath, "");

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                if (hasher.decrypt(data[0], MainViewModel.profilePath) != _profilename)
                {
                    if (!string.IsNullOrEmpty(File.ReadAllText(MainViewModel.profilePath)))
                    {
                        File.AppendAllText(MainViewModel.profilePath, Environment.NewLine);
                    }

                    File.AppendAllText(MainViewModel.profilePath, line);
                }
            }
        }

        private void checkForFile()
        {
            if (!File.Exists(MainViewModel.profilePath))
            {
                File.WriteAllText(MainViewModel.profilePath, "");
            }
        }
    }
}