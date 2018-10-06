using System;
using System.Linq;
using System.Text;

namespace Nexaas.ID.Client
{
    public static class Extensions
    {
        public static string UrlEncode(this string value) =>
            System.Net.WebUtility.UrlEncode(value);

        public static string UrlDecode(this string value) =>
            System.Net.WebUtility.UrlDecode(value);

        public static string AddPath(this string url, string path) =>
            url.EndsWith("/") ? $"{url}{path}" : $"{url}/{path}";

        public static string AddQueryStringParameter(this string url, string parameter, string value)
        {
            var builder = new UriBuilder(url);

            if (builder.Query.Length > 1)
            {
                builder.Query = $"{builder.Query.Substring(1)}&{parameter}={value}";
                return builder.Uri.ToString();
            }

            builder.Query = $"{parameter}={value}";

            return builder.Uri.ToString();
        }
    }
}