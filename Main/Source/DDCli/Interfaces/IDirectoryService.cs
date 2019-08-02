using System.Collections.Generic;

namespace DDCli.Interfaces
{
    public interface IDirectoryService
    {
        string GetCurrentPath();
        List<string> SearchDirectories(string path, string name, bool includeSubdirectories);
    }
}