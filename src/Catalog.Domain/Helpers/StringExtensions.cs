using System;
using System.Text.RegularExpressions;

namespace Catalog.Domain.Helpers
{
    public static class StringExtensions
    {
        /// <summary>
        /// Checks string object's value to array of string values
        /// </summary>        
        /// <param name="stringValues">Array of string values to compare</param>
        /// <returns>Return true if any string value matches</returns>
        public static bool In(this string value, params string[] stringValues)
        {
            foreach (string otherValue in stringValues)
                if (string.Compare(value, otherValue) == 0)
                    return true;

            return false;
        }

        /// <summary>
        /// Converts string to enum object
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <param name="value">String value to convert</param>
        /// <returns>Returns enum object</returns>
        public static T ToEnum<T>(this string value)
            where T : struct
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Returns characters from right of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from right</returns>
        public static string Right(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(value.Length - length) : value;
        }

        /// <summary>
        /// Returns characters from left of specified length
        /// </summary>
        /// <param name="value">String value</param>
        /// <param name="length">Max number of charaters to return</param>
        /// <returns>Returns string from left</returns>
        public static string Left(this string value, int length)
        {
            return value != null && value.Length > length ? value.Substring(0, length) : value;
        }

        /// <summary>
        /// Makes first character upper case.
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Returns string first character upper</returns>
        public static string UcFirst(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            char[] theChars = value.ToCharArray();
            theChars[0] = char.ToUpper(theChars[0]);

            return new string(theChars);

        }

        /// <summary>
        /// Checks if a string value is numeric according to you system culture
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Return if string is numeric</returns>
        public static bool IsNumeric(this string value)
        {
            return long.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out _);
        }

        /// <summary>
        /// Completely removes html tags not replace with xml tags. Not sanitizing
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Returns cleared string from html tags</returns>
        public static string StripHtml(this string value)
        {
            var tagsExpression = new Regex(@"</?.+?>");
            return tagsExpression.Replace(value, " ");
        }

        /// <summary>
        /// Validates whether a string is compliant with a strong password policy
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Returns if password is strong</returns>
        public static bool IsStrongPassword(this string value)
        {
            bool isStrong = Regex.IsMatch(value, @"[\d]");
            if (isStrong) isStrong = Regex.IsMatch(value, @"[a-z]");
            if (isStrong) isStrong = Regex.IsMatch(value, @"[A-Z]");
            if (isStrong) isStrong = Regex.IsMatch(value, @"[\s~!@#\$%\^&\*\(\)\{\}\|\[\]\\:;'?,.`+=<>\/]");
            if (isStrong) isStrong = value.Length > 7;
            return isStrong;
        }

        /// <summary>
        /// Check wheter a string is an valid e-mail address
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Returns if email is valid</returns>
        public static bool IsValidEmailAddress(this string value)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(value);
        }

        /// <summary>
        /// Convert a string to CamelCase
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Returns camel cased string</returns>
        public static string ToCamelCase(this string value)
        {
            if (value == null || value.Length < 2)
                return value;

            string[] words = value.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            string result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result +=
                    words[i].Substring(0, 1).ToUpper() +
                    words[i].Substring(1);
            }

            return result;
        }

        /// <summary>
        /// Changes filename string to criterion of seo 
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Returns seo based filename string</returns>
        public static string ToSeoFilename(this string value)
        {

            value = value.Replace("ş", "s");
            value = value.Replace("Ş", "s");
            value = value.Replace("İ", "i");
            value = value.Replace("I", "i");
            value = value.Replace("ı", "i");
            value = value.Replace("ö", "o");
            value = value.Replace("Ö", "o");
            value = value.Replace("ü", "u");
            value = value.Replace("Ü", "u");
            value = value.Replace("Ç", "c");
            value = value.Replace("ç", "c");
            value = value.Replace("ğ", "g");
            value = value.Replace("Ğ", "g");
            value = value.Replace(" ", "-");
            value = value.Replace("---", "-");
            value = value.Replace("--", "-");
            value = value.Replace("?", "");
            value = value.Replace("/", "");
            value = value.Replace(".", "");
            value = value.Replace("'", "");
            value = value.Replace("#", "");
            value = value.Replace("%", "");
            value = value.Replace("&", "");
            value = value.Replace("*", "");
            value = value.Replace("!", "");
            value = value.Replace("@", "");
            value = value.Replace("+", "");

            value = value.ToLowerInvariant();
            value = value.Trim();

            return value;
        }

        /// <summary>
        /// Checks if string has html in it
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Returns if string has html tags</returns>
        public static bool IsHtml(this string value)
        {
            Regex tagRegex = new Regex(@"<[^>]+>");
            bool hasTags = tagRegex.IsMatch(value);

            return hasTags;
        }
    }
}
