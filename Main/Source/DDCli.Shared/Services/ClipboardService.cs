using DDCli.Extensions;
using DDCli.Interfaces;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Services
{
    public class ClipboardService : IClipboardService
    {
        public ClipboardService()
        {
        }

        public void CopyToClipboard(string val)
        {
            throw new NotImplementedException();//$"echo {val} | clip".Bat();
        }
    }
}
