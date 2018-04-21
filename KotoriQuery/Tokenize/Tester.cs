using System.Text.RegularExpressions;

namespace KotoriQuery.Tokenize
{
    public static class Tester
    {
        public static bool IsIdentifierHead(Char32 c)
        {
            return Regex.IsMatch(c.ToString(), "[a-zA-Z_$]");
        }

        public static bool IsIdentifierTailing(Char32 c)
        {
            return Regex.IsMatch(c.ToString(), "[a-zA-Z_0-9]");
        }

        public static bool IsDigit(Char32 c)
        {
            return Regex.IsMatch(c.ToString(), "[0-9]");
        }

        public static bool IsWhiteSpace(Char32 c)
        {
            return Regex.IsMatch(c.ToString(), @"\s");
        }
    }
}