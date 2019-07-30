using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DDCli.Utilities
{
    public static class DirectoryUtilities
    {


        public static List<string> SearchDirectories(string path, string name, bool includeSubdirectories)
        {
            IEnumerable<string> list = Directory.GetDirectories(path).Where(s => s.ToLowerInvariant().IndexOf(name)>-1);
            return list.ToList();
        }


        public static string GetCurrentPath()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
