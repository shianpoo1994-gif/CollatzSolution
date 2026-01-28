using System.Numerics;

namespace Collatz.Core
{
    public static class NaturalNumberValidator
    {
        public static bool TryParse(string? text, out BigInteger n, out string error)
        {
            n = default;
            error = "";

            if (string.IsNullOrWhiteSpace(text))
            {
                error = "Bitte geben Sie eine natürliche Zahl ein.";
                return false;
            }

            text = text.Trim();

            foreach(char c in text)
            {
                if (c < '0' || c > '9')
                {
                    error = "Es sind nur dezimale Ziffern erlaubt.";
                    return false;
                }
            }

            if (!BigInteger.TryParse(text, out n))
            {
                error = "Zahl konnte nicht geparst werden.";
                return false;
            }

            if (n <= 0)
            {
                error = "Eine natürliche Zahl muss größer als 0 sein.";
                return false;
            }
            return true;
        }
    }
}
