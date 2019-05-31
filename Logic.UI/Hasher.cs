namespace Logic.UI
{
    public class Hasher
    {
        public string encrypt(string _value)
        {
            string ret = "";

            if (!string.IsNullOrEmpty(_value))
            {
                char[] characters = _value.ToCharArray();

                foreach (char character in characters)
                {
                    ret += encryptCharacter(character);
                }
            }

            return ret;
        }

        public string decrypt(string _value)
        {
            string ret = "";

            if (!string.IsNullOrEmpty(_value))
            {
                char[] characters = _value.ToCharArray();

                foreach (char character in characters)
                {
                    ret += decryptCharacter(character);
                }
            }

            return ret;
        }

        private char encryptCharacter(char _character)
        {
            char ret = ' ';

            switch (_character)
            {
                case 'a':
                    ret = 'J';
                    break;

                case 'b':
                    ret = '7';
                    break;

                case 'c':
                    ret = 'l';
                    break;

                case 'd':
                    ret = 'O';
                    break;

                case 'e':
                    ret = 'z';
                    break;

                case 'f':
                    ret = 'w';
                    break;

                case 'g':
                    ret = 'I';
                    break;

                case 'h':
                    ret = 'v';
                    break;

                case 'i':
                    ret = 'a';
                    break;

                case 'j':
                    ret = 'S';
                    break;

                case 'k':
                    ret = '8';
                    break;

                case 'l':
                    ret = 'P';
                    break;

                case 'm':
                    ret = 'E';
                    break;

                case 'n':
                    ret = 'q';
                    break;

                case 'o':
                    ret = 'M';
                    break;

                case 'p':
                    ret = 'r';
                    break;

                case 'q':
                    ret = '1';
                    break;

                case 'r':
                    ret = 'B';
                    break;

                case 's':
                    ret = 'c';
                    break;

                case 't':
                    ret = 'Q';
                    break;

                case 'u':
                    ret = 'U';
                    break;

                case 'v':
                    ret = 'V';
                    break;

                case 'w':
                    ret = '3';
                    break;

                case 'x':
                    ret = 'f';
                    break;

                case 'y':
                    ret = 'Z';
                    break;

                case 'z':
                    ret = '0';
                    break;

                case 'A':
                    ret = '4';
                    break;

                case 'B':
                    ret = 'W';
                    break;

                case 'C':
                    ret = 'G';
                    break;

                case 'D':
                    ret = 'K';
                    break;

                case 'E':
                    ret = 'p';
                    break;

                case 'F':
                    ret = 'N';
                    break;

                case 'G':
                    ret = 'Y';
                    break;

                case 'H':
                    ret = '5';
                    break;

                case 'I':
                    ret = 'g';
                    break;

                case 'J':
                    ret = '2';
                    break;

                case 'K':
                    ret = 'C';
                    break;

                case 'L':
                    ret = 'e';
                    break;

                case 'M':
                    ret = 't';
                    break;

                case 'N':
                    ret = 'x';
                    break;

                case 'O':
                    ret = 'D';
                    break;

                case 'P':
                    ret = 'o';
                    break;

                case 'Q':
                    ret = 's';
                    break;

                case 'R':
                    ret = 'F';
                    break;

                case 'S':
                    ret = 'd';
                    break;

                case 'T':
                    ret = 'X';
                    break;

                case 'U':
                    ret = 'i';
                    break;

                case 'V':
                    ret = 'A';
                    break;

                case 'W':
                    ret = 'h';
                    break;

                case 'X':
                    ret = 'b';
                    break;

                case 'Y':
                    ret = '6';
                    break;

                case 'Z':
                    ret = 'j';
                    break;

                case '0':
                    ret = 'k';
                    break;

                case '1':
                    ret = '9';
                    break;

                case '2':
                    ret = 'y';
                    break;

                case '3':
                    ret = 'm';
                    break;

                case '4':
                    ret = 'R';
                    break;

                case '5':
                    ret = 'T';
                    break;

                case '6':
                    ret = 'u';
                    break;

                case '7':
                    ret = 'L';
                    break;

                case '8':
                    ret = 'n';
                    break;

                case '9':
                    ret = 'H';
                    break;

                default:
                    ret = _character;
                    break;
            }

            return ret;
        }

        private char decryptCharacter(char _character)
        {
            char ret = ' ';

            switch (_character)
            {
                case 'J':
                    ret = 'a';
                    break;

                case '7':
                    ret = 'b';
                    break;

                case 'l':
                    ret = 'c';
                    break;

                case 'O':
                    ret = 'd';
                    break;

                case 'z':
                    ret = 'e';
                    break;

                case 'w':
                    ret = 'f';
                    break;

                case 'I':
                    ret = 'g';
                    break;

                case 'v':
                    ret = 'h';
                    break;

                case 'a':
                    ret = 'i';
                    break;

                case 'S':
                    ret = 'j';
                    break;

                case '8':
                    ret = 'k';
                    break;

                case 'P':
                    ret = 'l';
                    break;

                case 'E':
                    ret = 'm';
                    break;

                case 'q':
                    ret = 'n';
                    break;

                case 'M':
                    ret = 'o';
                    break;

                case 'r':
                    ret = 'p';
                    break;

                case '1':
                    ret = 'q';
                    break;

                case 'B':
                    ret = 'r';
                    break;

                case 'c':
                    ret = 's';
                    break;

                case 'Q':
                    ret = 't';
                    break;

                case 'U':
                    ret = 'u';
                    break;

                case 'V':
                    ret = 'v';
                    break;

                case '3':
                    ret = 'w';
                    break;

                case 'f':
                    ret = 'x';
                    break;

                case 'Z':
                    ret = 'y';
                    break;

                case '0':
                    ret = 'z';
                    break;

                case '4':
                    ret = 'A';
                    break;

                case 'W':
                    ret = 'B';
                    break;

                case 'G':
                    ret = 'C';
                    break;

                case 'K':
                    ret = 'D';
                    break;

                case 'p':
                    ret = 'E';
                    break;

                case 'N':
                    ret = 'F';
                    break;

                case 'Y':
                    ret = 'G';
                    break;

                case '5':
                    ret = 'H';
                    break;

                case 'g':
                    ret = 'I';
                    break;

                case '2':
                    ret = 'J';
                    break;

                case 'C':
                    ret = 'K';
                    break;

                case 'e':
                    ret = 'L';
                    break;

                case 't':
                    ret = 'M';
                    break;

                case 'x':
                    ret = 'N';
                    break;

                case 'D':
                    ret = 'O';
                    break;

                case 'o':
                    ret = 'P';
                    break;

                case 's':
                    ret = 'Q';
                    break;

                case 'F':
                    ret = 'R';
                    break;

                case 'd':
                    ret = 'S';
                    break;

                case 'X':
                    ret = 'T';
                    break;

                case 'i':
                    ret = 'U';
                    break;

                case 'A':
                    ret = 'V';
                    break;

                case 'h':
                    ret = 'W';
                    break;

                case 'b':
                    ret = 'X';
                    break;

                case '6':
                    ret = 'Y';
                    break;

                case 'j':
                    ret = 'Z';
                    break;

                case 'k':
                    ret = '0';
                    break;

                case '9':
                    ret = '1';
                    break;

                case 'y':
                    ret = '2';
                    break;

                case 'm':
                    ret = '3';
                    break;

                case 'R':
                    ret = '4';
                    break;

                case 'T':
                    ret = '5';
                    break;

                case 'u':
                    ret = '6';
                    break;

                case 'L':
                    ret = '7';
                    break;

                case 'n':
                    ret = '8';
                    break;

                case 'H':
                    ret = '9';
                    break;

                default:
                    ret = _character;
                    break;
            }

            return ret;
        }
    }
}