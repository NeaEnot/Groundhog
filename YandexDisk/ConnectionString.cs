using Core;
using System.Text.RegularExpressions;

namespace YandexDisk
{
    internal static class ConnectionString
    {
        internal static Regex connectionStringExpr = new Regex(@"^token=(?<token>.+);path=(?<path>[/.a-zA-Z0-9]+)$");

        internal static string Token => connectionStringExpr.Match(GroundhogContext.ConnectionString).Groups["token"].Value;
        internal static string Path => connectionStringExpr.Match(GroundhogContext.ConnectionString).Groups["path"].Value;
    }
}
