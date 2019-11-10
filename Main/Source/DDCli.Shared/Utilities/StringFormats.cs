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

        public static string MillisecondsToHumanTime(long ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                    t.Hours,
                                    t.Minutes,
                                    t.Seconds,
                                    t.Milliseconds);
            return answer;
        }
    }
}
