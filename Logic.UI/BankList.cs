using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logic.UI
{
    public class BankList
    {
        public static List<string> getBankNameList()
        {
            List<string> bankNameList = new List<string>();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Logic.UI.Resources.Bankinfo.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader streamReader = new StreamReader(stream, Encoding.Default))
            {
                while (streamReader.Peek() > 0)
                {
                    string bankName = streamReader.ReadLine().Substring(9, 58).TrimEnd(' ');

                    if (!bankNameList.Contains(bankName))
                    {
                        bankNameList.Add(bankName);
                    }
                }
            }

            bankNameList.Sort();

            return bankNameList;
        }

        public static List<string> getBankInformation(string _bankName)
        {
            List<string> bankInformation = new List<string>();
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Logic.UI.Resources.Bankinfo.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader streamReader = new StreamReader(stream, Encoding.Default))
            {
                while (streamReader.Peek() > 0)
                {
                    string line;

                    if ((line = streamReader.ReadLine()).Contains(_bankName))
                    {
                        if (!bankInformation.Contains(line.Substring(0, 8).Trim()))
                        {
                            bankInformation.Add(line.Substring(0, 8).Trim());
                            bankInformation.Add(line.Substring(139, 11));
                        }
                    }
                }
            }

            return bankInformation;
        }
    }
}
