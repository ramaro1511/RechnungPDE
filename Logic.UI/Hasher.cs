using System;
using System.IO;

namespace Logic.UI
{
    public class Hasher
    {
        public string encrypt(string _value, string _path)
        {
            string ret = "";

            if (!string.IsNullOrEmpty(_value))
            {
                string fileCreation = Convert.ToString(File.GetCreationTime(_path));

                if (!string.IsNullOrEmpty(fileCreation))
                {
                    string seed = "";

                    foreach (char character in fileCreation)
                    {
                        if (char.IsDigit(character))
                        {
                            seed += character;
                        }
                    }

                    if (!string.IsNullOrEmpty(seed))
                    {
                        char[] characters = _value.ToCharArray();

                        foreach (char character in characters)
                        {
                            ret += ((int)character * Convert.ToInt64(seed) ).ToString("X") + ",";
                        }

                        ret = ret.Remove(ret.LastIndexOf(','));
                    }
                }
            }

            return ret;
        }

        public string decrypt(string _value, string _path)
        {
            string ret = "";

            if (!string.IsNullOrEmpty(_value))
            {
                string fileCreation = Convert.ToString(File.GetCreationTime(_path));

                if (!string.IsNullOrEmpty(fileCreation))
                {
                    string seed = "";

                    foreach (char character in fileCreation)
                    {
                        if (char.IsDigit(character))
                        {
                            seed += character;
                        }
                    }

                    if (!string.IsNullOrEmpty(seed))
                    {
                        char[] characters = _value.ToCharArray();
                        string hexValue = "";

                        foreach (char character in characters)
                        {
                            if (!character.Equals(','))
                            {
                                hexValue += character;
                            }
                            else
                            {
                                Int64 decValue = Convert.ToInt64(hexValue, 16);
                                decValue = decValue / Convert.ToInt64(seed);
                                ret += Convert.ToChar(decValue);
                                hexValue = "";
                            }
                        }

                        if (!string.IsNullOrEmpty(hexValue))
                        {
                            Int64 decValue = Convert.ToInt64(hexValue, 16);
                            decValue = decValue / Convert.ToInt64(seed);
                            ret += Convert.ToChar(decValue);
                            hexValue = "";
                        }
                    }
                }
            }

            return ret;
        }
    }
}