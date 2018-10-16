using System.Text.RegularExpressions;

namespace Nexaas.ID.Client
{
    internal class Validations
    {
        private const string EmailPattern =
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        private const string UrlPattern = @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$";
        
        public static bool IsEmail(string email) => Regex.IsMatch(email, EmailPattern);

        public static bool IsUrl(string url)
        {
            if (url.Contains("http://localhost") || url.Contains("https://localhost")) return true;
            return Regex.IsMatch(url, UrlPattern);
        }
    }
}