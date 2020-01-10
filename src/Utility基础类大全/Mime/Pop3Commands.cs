using System;
namespace Maticsoft.Common.Mail
{
    internal static class Pop3Commands
    {
        public const string User = "USER ";
        public const string Crlf = "\r\n";
        public const string Quit = "QUIT\r\n";
        public const string Stat = "STAT\r\n";
        public const string List = "LIST ";
        public const string Retr = "RETR ";
        public const string Noop = "NOOP\r\n";
        public const string Dele = "DELE ";
        public const string Rset = "RSET\r\n";
        public const string Pass = "PASS ";
        public const string Top = "TOP ";
    }
}
