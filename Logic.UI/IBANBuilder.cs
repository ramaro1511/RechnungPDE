using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.UI
{
    public class IBANBuilder
    {
        private static int index { get; set; }

        private static string bankCodeNumber { get; set; }

        private static string bankAccountNumber { get; set; }

        public static string getIBAN(string _bankCodeNumber, string _bankAccountNumber)
        {
            index = CountryList.getCountryIndex(true);
            bankCodeNumber = _bankCodeNumber;
            bankAccountNumber = _bankAccountNumber;

            return CountryList.getCountryCode(index) + getCheckDigit() + getBBAN();
        }

        private static string getCheckDigit()
        {
            string checkDigit = calculateCheckDigit().ToString();

            if (int.Parse(checkDigit) < 10)
                checkDigit = checkDigit.Insert(0, "0");

            return checkDigit;
        }

        private static int calculateCheckDigit()
        {
            decimal modulo97 = decimal.Parse(getIBANDividend()) % 97;

            return 98 - Convert.ToInt32(modulo97);
        }

        private static string getIBANDividend()
        {
            string numericCountryCodeValue = "";

            foreach (char letter in CountryList.getCountryCode(index))
            {
                numericCountryCodeValue += CountryList.getNumericCountryCodeValue(letter);
            }

            return getBBAN() + numericCountryCodeValue + "00";
        }

        private static string getBBAN()
        {
            while (bankAccountNumber.Length < 10)
            {
                bankAccountNumber = bankAccountNumber.Insert(0, "0");
            }

            return bankCodeNumber + bankAccountNumber;
        }

        public static string formatIBAN(string _IBAN)
        {
            string ret = "";

            for (int counter = 0; counter < _IBAN.Length; counter++)
            {
                if (counter != 0 && counter % 4 == 0)
                    ret = ret.Insert(ret.Length, " ");

                if (ret.Length < 1)
                    ret = ret.Insert(0, _IBAN.Substring(counter, 1));
                else
                    ret = ret.Insert(ret.Length, _IBAN.Substring(counter, 1));
            }

            return ret;
        }
    }
}
