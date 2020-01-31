using System;
using System.IO;

namespace Logic.UI
{
    public class ConfigHandler
    {
        Hasher hasher = new Hasher();

        public void edit(string _configOption, string _configValue)
        {
            string[] lines = File.ReadAllLines(MainViewModel.configPath);

            File.WriteAllText(MainViewModel.configPath, "");

            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(File.ReadAllText(MainViewModel.configPath)))
                    File.AppendAllText(MainViewModel.configPath, Environment.NewLine);

                if (line.Contains(_configOption))
                {
                    File.AppendAllText(MainViewModel.configPath, _configOption + _configValue);
                }
                else
                {
                    File.AppendAllText(MainViewModel.configPath, line);
                }
            }
        }

        public string read(MainViewModel _mainViewModel, string _configOption)
        {
            string ret = null;

            string[] lines = File.ReadAllLines(MainViewModel.configPath);

            foreach (string line in lines)
            {
                if (line.Contains(_configOption))
                    ret = line.Substring(line.IndexOf('=') + 1);
            }

            if (_configOption == "selectedProfile=")
                ret = hasher.decrypt(ret, MainViewModel.configPath);

            return ret;
        }
    }
}
