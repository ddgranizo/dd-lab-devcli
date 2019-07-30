using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DDCli.Utilities
{
    public static class PathUtilities
    {
        public static string GetCurrentPath()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
