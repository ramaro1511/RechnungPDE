using System.Collections.Generic;
using System.Globalization;

namespace Logic.UI
{
    public class CountryList
    {
        public static List<string> getCountryList(bool _getEnglishNames = false)
        {
            List<string> countries = new List<string>();
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            RegionInfo region;

            foreach (CultureInfo culture in cultures)
            {
                region = new RegionInfo(culture.LCID);

                if (_getEnglishNames)
                {
                    if (!countries.Contains(region.EnglishName))
                    {
                        countries.Add(region.EnglishName);
                    }
                }
                else
                {
                    if (!countries.Contains(region.DisplayName))
                    {
                        countries.Add(region.DisplayName);
                    }
                }
            }

            countries.Sort();

            return countries;
        }

        public static int getCountryIndex(bool _getEnglishNames = false)
        {
            RegionInfo myRegion = new RegionInfo(CultureInfo.CurrentCulture.LCID);

            if (_getEnglishNames)
                return getCountryList(true).IndexOf(myRegion.EnglishName);
            else
                return getCountryList().IndexOf(myRegion.DisplayName);
        }

        public static string getCountryCode(int _countryIndex)
        {
            switch (_countryIndex)
            {
                case 1:
                    return "AL";

                case 6:
                    return "AT";

                case 7:
                    return "AZ";

                case 8:
                    return "BH";

                case 10:
                    return "BY";

                case 11:
                    return "BE";

                case 15:
                    return "BA";

                case 17:
                    return "BR";

                case 19:
                    return "BG";

                case 28:
                    return "CR";

                case 30:
                    return "CY";

                case 32:
                    return "CZ";

                case 33:
                    return "DK";

                case 35:
                    return "DO";

                case 38:
                    return "SV";

                case 40:
                    return "EE";

                case 42:
                    return "FO";

                case 43:
                    return "FI";

                case 44:
                    return "FR";

                case 45:
                    return "GE";

                case 46:
                    return "DE";

                case 47:
                    return "GR";

                case 48:
                    return "GL";

                case 49:
                    return "GT";

                case 53:
                    return "HU";

                case 54:
                    return "IS";

                case 58:
                    return "IQ";

                case 59:
                    return "IE";

                case 61:
                    return "IT";

                case 64:
                    return "JO";

                case 65:
                    return "KZ";

                case 68:
                    return "KW";

                case 72:
                    return "LV";

                case 73:
                    return "LB";

                case 75:
                    return "LI";

                case 76:
                    return "LT";

                case 77:
                    return "LU";

                case 83:
                    return "MT";

                case 85:
                    return "MD";

                case 86:
                    return "MC";

                case 88:
                    return "ME";

                case 92:
                    return "NL";

                case 96:
                    return "NO";

                case 98:
                    return "PK";

                case 103:
                    return "PL";

                case 104:
                    return "PT";

                case 106:
                    return "QA";

                case 108:
                    return "RO";

                case 111:
                    return "SA";

                case 113:
                    return "RS";

                case 115:
                    return "SK";

                case 116:
                    return "SI";

                case 119:
                    return "ES";

                case 121:
                    return "SE";

                case 122:
                    return "CH";

                case 128:
                    return "TN";

                case 129:
                    return "TR";

                case 131:
                    return "UA";

                case 132:
                    return "AE";

                case 133:
                    return "GB";

                default:
                    return null;
            }
        }

        public static string getNumericCountryCodeValue(char _letter)
        {
            return ((int)_letter - 55).ToString();
        }
    }
}
