using System;
using System.IO;

namespace Logic.UI
{
    public class ConfigHandler
    {
        Hasher hasher = new Hasher();

        public void edit(MainViewModel _mainViewModel, string _configOption, string _configValue)
        {
            string[] lines = File.ReadAllLines(_mainViewModel.configPath);

            File.WriteAllText(_mainViewModel.configPath, "");

            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(File.ReadAllText(_mainViewModel.configPath)))
                    File.AppendAllText(_mainViewModel.configPath, Environment.NewLine);

                if (line.Contains(_configOption))
                {
                    File.AppendAllText(_mainViewModel.configPath, _configOption + _configValue);
                }
                else
                {
                    File.AppendAllText(_mainViewModel.configPath, line);
                }
            }
        }

        public string read(MainViewModel _mainViewModel, string _configOption)
        {
            string ret = null;

            string[] lines = File.ReadAllLines(_mainViewModel.configPath);

            foreach (string line in lines)
            {
                if (line.Contains(_configOption))
                    ret = line.Substring(line.IndexOf('=') + 1);
            }

            if (_configOption == "selectedProfile=")
                ret = hasher.decrypt(ret);

            return ret;
        }


    }
}
