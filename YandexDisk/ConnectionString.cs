using System;
using System.Text.RegularExpressions;

namespace YandexDisk
{
    internal class ConnectionString
    {
        internal static Regex connectionStringExpr = new Regex(@"^token=(?<token>.+);path=(?<path>[/.a-zA-Z0-9]+)$");

        internal string Token => connectionStringExpr.Match(connectionString()).Groups["token"].Value;
        internal string Path => connectionStringExpr.Match(connectionString()).Groups["path"].Value;

        private Func<string> connectionString;

        internal ConnectionString(Func<string> connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
