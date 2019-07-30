using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Utilities
{
    public static class ClipboardManager
    {
        public static void CopyToClipboard(string val)
        {
            $"echo {val} | clip".Bat();
        }
    }
}
