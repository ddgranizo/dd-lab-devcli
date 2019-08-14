using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DDCli.Utilities
{
    public static class StringFormats
    {

        public static string[] StringToParams(string text)
        {
            return Regex.Matches(text, @"[\""].+?[\""]|[^ ]+")
                        .Cast<Match>()
                        .Select(m => m.Value)
                        .Select(k => k.Trim('\"'))
                        .ToArray();
        }
        public static bool IsValidLogicalName(string text)
        {
            Regex r = new Regex("^[a-zA-Z0-9]*$");
            return r.IsMatch(text);
        }
    }
}
