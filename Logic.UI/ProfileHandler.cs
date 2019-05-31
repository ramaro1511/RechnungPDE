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

                if (hasher.decrypt(data[0]) == _profilename)
                {
                    return data;
                }
            }

            return null;
        }

        public void writeCustomer(MainViewModel _mainViewModel, string _path)
        {
            string[] lines = File.ReadAllLines(_path);

            File.WriteAllText(_path, "");

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                if (!string.IsNullOrEmpty(File.ReadAllText(_path)))
                    File.AppendAllText(_path, Environment.NewLine);

                if (hasher.decrypt(data[0]) == _mainViewModel.customerCompanyName)
                {
                    data[0] = hasher.encrypt(_mainViewModel.customerCompanyName);
                    data[1] = hasher.encrypt(_mainViewModel.customerCompanyName);
                    data[2] = hasher.encrypt(_mainViewModel.customerFirstName);
                    data[3] = hasher.encrypt(_mainViewModel.customerLastName);
                    data[4] = hasher.encrypt(_mainViewModel.customerPostalCode);
                    data[5] = hasher.encrypt(_mainViewModel.customerCityName);
                    data[6] = hasher.encrypt(_mainViewModel.customerAddress);
                    data[7] = hasher.encrypt(_mainViewModel.customerAddressNumber);
                    data[8] = hasher.encrypt(_mainViewModel.customerCountryName);
                    data[9] = hasher.encrypt(_mainViewModel.customerNumber);

                    File.AppendAllText(_path, string.Join(";", data));
                }
                else
                {
                    File.AppendAllText(_path, line);
                }
            }
        }

        public void write(MainViewModel _mainViewModel, string _path, ListBoxItem _listBoxItem = null)
        {
            string[] lines = File.ReadAllLines(_path);

            File.WriteAllText(_path, "");

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                if (!string.IsNullOrWhiteSpace(File.ReadAllText(_path)))
                    File.AppendAllText(_path, Environment.NewLine);

                if (hasher.decrypt(data[0]) == (string)_listBoxItem.Content)
                {
                    data[0] = hasher.encrypt((string)_listBoxItem.Content);
                    data[1] = hasher.encrypt(_mainViewModel.profileCompanyName);
                    data[2] = hasher.encrypt(_mainViewModel.profileFirstName);
                    data[3] = hasher.encrypt(_mainViewModel.profileLastName);
                    data[4] = hasher.encrypt(_mainViewModel.profilePostalCode);
                    data[5] = hasher.encrypt(_mainViewModel.profileCityName);
                    data[6] = hasher.encrypt(_mainViewModel.profileAddress);
                    data[7] = hasher.encrypt(_mainViewModel.profileAddressNumber);
                    data[8] = hasher.encrypt(_mainViewModel.profileCountryName);
                    data[9] = hasher.encrypt(_mainViewModel.profileEMailAddress);
                    data[10] = hasher.encrypt(_mainViewModel.profileTelephoneNumber);
                    data[11] = hasher.encrypt(_mainViewModel.profileMobileNumber);
                    data[12] = hasher.encrypt(_mainViewModel.profileFaxNumber);
                    data[13] = hasher.encrypt(_mainViewModel.profileBankName);
                    data[14] = hasher.encrypt(_mainViewModel.profileBankAccountNumber);
                    data[15] = hasher.encrypt(_mainViewModel.profileBankCodeNumber);
                    data[16] = hasher.encrypt(_mainViewModel.profileIBAN);
                    data[17] = hasher.encrypt(_mainViewModel.profileBIC);

                    File.AppendAllText(_path, string.Join(";", data));
                }
                else
                {
                    File.AppendAllText(_path, line);
                }
            }
        }

        public void add(string _name, string _path, bool _isCustomerProfile)
        {
            checkForFile(_path);

            int pLength;

            if (_isCustomerProfile)
                pLength = customerProfileLength;
            else
                pLength = profileLength;

            string[] newProfileLines = new string[pLength];

            for (int i = 0; i < pLength; i++)
            {
                if (i == 0)
                    newProfileLines[i] += hasher.encrypt(_name);
                else
                    newProfileLines[i] = "";
            }

            if (!string.IsNullOrEmpty(File.ReadAllText(_path)))
                File.AppendAllText(_path, "\n");


            File.AppendAllText(_path, string.Join(";", newProfileLines));
        }

        public void remove(string _profilename, string _path)
        {
            checkForFile(_path);

            string[] lines = File.ReadAllLines(_path);

            File.WriteAllText(_path, "");

            foreach (string line in lines)
            {
                string[] data = line.Split(';');

                if (hasher.decrypt(data[0]) != _profilename)
                {
                    if (!string.IsNullOrEmpty(File.ReadAllText(_path)))
                    {
                        File.AppendAllText(_path, Environment.NewLine);
                    }

                    File.AppendAllText(_path, line);
                }
            }
        }

        private void checkForFile(string _path)
        {
            if (!File.Exists(_path))
            {
                File.WriteAllText(_path, "");
            }
        }
    }
}