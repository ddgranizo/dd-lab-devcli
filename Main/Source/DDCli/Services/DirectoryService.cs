using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DDCli.Services
{
    public class DirectoryService : IDirectoryService
    {
        public DirectoryService()
        {
        }

        public List<string> SearchDirectories(string path, string name, bool includeSubdirectories)
        {
            return Directory.GetDirectories(path)
                .Where(s => s.ToLowerInvariant()
                .IndexOf(name) > -1)
                .ToList();
        }

        public string GetCurrentPath()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
