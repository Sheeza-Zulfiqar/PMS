using System.Text.RegularExpressions;
using System.Text;

namespace PMSApi.Utils
{
    public static partial class StringExtenstionMethods
    {
        public static string ToZodProperty(this string str)
        {
            var propertySplit = str.Split('.');
            var result = string.Empty;
            foreach (var property in propertySplit)
            {
                if (result.Length > 0)
                    result += ".";
                result += property.ToCamelCase();
            }
            return result.Replace("[", ".").Replace("]", "");
        }

        public static string ToCamelCase(this string str)
        {
            if (str.Length > 1)
                return char.ToLower(str[0]) + str.Substring(1);
            if (str.Length > 0)
                return char.ToLower(str[0]).ToString();
            return string.Empty;
        }

        public static string DecodeBase64(this string str)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(str));
            }
            catch (Exception)
            {
                return str;
            }
        }

        public static bool IsEmail(this string str)
        {
            return EmailRegex().IsMatch(str);
        }

        public static bool IsPhoneNumber(this string str)
        {
            return PhoneNumberRegex().IsMatch(str);
        }

        public static bool IsUrl(this string str)
        {
            return UrlRegex().IsMatch(str);
        }

        [GeneratedRegex("^\\S+@\\S+\\.\\S+$")]
        private static partial Regex EmailRegex();

        [GeneratedRegex(@"^971\d{9}$")]
        private static partial Regex PhoneNumberRegex();

        [GeneratedRegex(@"^https?:\/\/[\w\-]+(\.[\w\-]+)+[/#?]?.*$")]
        private static partial Regex UrlRegex();
    }

}
