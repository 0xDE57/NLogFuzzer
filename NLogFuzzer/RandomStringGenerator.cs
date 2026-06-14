namespace NLogFuzzer
{
    internal class RandomStringGenerator
    {
        // Default character set
        private const string DefaultCharSet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz" +
            "0123456789" +
            "!@#$%^&*()-_=+[]{}|;:,.<>?";

        // Generate a random string with custom character set
        public static string RandomString(int min, int max, string? charSet = null)
        {
            string chars = charSet ?? DefaultCharSet;

            if (min < 0) min = 0;
            if (min > max)
                throw new ArgumentException($"Minimum ({min}) cannot be greater than maximum ({max})");
            if (chars.Length == 0)
                throw new ArgumentException("Character set cannot be empty");

            int length = Random.Shared.Next(min, max + 1);
            char[] charArray = new char[length];

            for (int i = 0; i < length; i++)
            {
                charArray[i] = chars[Random.Shared.Next(chars.Length)];
            }

            return new string(charArray);
        }

        // Generate a random string using only alphanumeric characters
        public static string RandomAlphanumeric(int min, int max)
        {
            string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return RandomString(min, max, charSet);
        }

        // Generate a random string using only ASCII printable characters
        public static string RandomAscii(int min, int max)
        {
            string charSet = new string(Enumerable.Range(33, 94).Select(c => (char)c).ToArray());
            return RandomString(min, max, charSet);
        }

        // Generate a random string including ANY character (ASCII 0-127)
        // This includes control characters like \n, \r, \t, null, etc.
        public static string RandomAnyChar(int min, int max)
        {
            // Full ASCII range: 0-127 (includes control characters and printable)
            string charSet = new string(Enumerable.Range(0, 128).Select(c => (char)c).ToArray());
            return RandomString(min, max, charSet);
        }

        // Generate a random string with only control characters (ASCII 0-31 and 127)
        public static string RandomControlChars(int min, int max)
        {
            string charSet = new string(Enumerable
                .Range(0, 32)
                .Select(c => (char)c)
                .Concat(new[] { (char)127 }) // DEL character
                .ToArray());
            return RandomString(min, max, charSet);
        }

        // Generate a random string with only printable ASCII characters (32-126)
        public static string RandomPrintableAscii(int min, int max)
        {
            string charSet = new string(Enumerable
                .Range(32, 95)
                .Select(c => (char)c)
                .ToArray());
            return RandomString(min, max, charSet);
        }

        // Generate a random string with a specific character range
        public static string RandomCharRange(int min, int max, int minChar, int maxChar)
        {
            if (minChar > maxChar)
                throw new ArgumentException($"Character range minimum ({minChar}) cannot be greater than maximum ({maxChar})");

            string charSet = new string(Enumerable
                .Range(minChar, maxChar - minChar + 1)
                .Select(c => (char)c)
                .ToArray());
            return RandomString(min, max, charSet);
        }

        // Generate a random string with any UTF-32 character (full Unicode)
        // Note: This can generate surrogate pairs and requires careful handling
        public static string RandomUnicode(int min, int max)
        {
            // Generate full Unicode range - note this includes all possible characters
            // For most cases, RandomAnyChar (ASCII 0-127) is sufficient
            string charSet = new string(Enumerable
                .Range(0, 0x110000) // Full Unicode range (1,114,112 characters)
                .Select(c => (char)c)
                .ToArray());

            // For better performance with large Unicode sets, consider using a 
            // HashSet or more efficient data structure
            return RandomString(min, max, charSet);
        }
    }
}
