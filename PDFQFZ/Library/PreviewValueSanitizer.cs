using System;

namespace PDFQFZ.Library
{
    internal static class PreviewValueSanitizer
    {
        public static string KeepDigitsOnly(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            char[] buffer = new char[text.Length];
            int index = 0;
            foreach (char c in text)
            {
                if (char.IsDigit(c))
                {
                    buffer[index++] = c;
                }
            }

            return new string(buffer, 0, index);
        }

        public static string KeepSignedInteger(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            char[] buffer = new char[text.Length];
            int index = 0;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (char.IsDigit(c))
                {
                    buffer[index++] = c;
                }
                else if (c == '-' && index == 0)
                {
                    buffer[index++] = c;
                }
            }

            return new string(buffer, 0, index);
        }

        public static int ClampInt(string text, int fallbackValue, int minValue, int maxValue)
        {
            int value;
            if (!int.TryParse(text, out value))
            {
                return fallbackValue;
            }

            if (value < minValue)
            {
                return minValue;
            }

            if (value > maxValue)
            {
                return maxValue;
            }

            return value;
        }
    }
}
