using DDCli.Models;
using System.Collections.Generic;

namespace DDCli.Interfaces
{
    public interface IFileService
    {
        string GetCurrentPath();
        List<string> SearchDirectories(string path, string name, bool includeSubdirectories);

        bool ExistsDirectory(string path);

        bool ExistsFile(string path);

        bool ExistsFileInDirectory(string folder, string filename);

        bool ExistsTemplateConfigFile(string path);

        DDTemplateConfig GetTemplateConfig(string path);


        List<string> CloneDirectory(string sourceFolder, string destinationFolder, List<string> ignorePathPatterns);


        string GetAbsolutePath(string absoluteRelativePath);

        void CreateDirectory(string path, bool reCreateIfExists = false);


        void ReplaceStringInPaths(string rootPath,  string oldValue, string newValue, bool includeDirectories, bool includeFileNames, bool includeFileContents, string filePattern);

        List<string> SearchDirectoriesInPath(string rootPath, string pattern);

        List<string> SearchFilesInPath(string rootPath, string pattern);

        void MoveFile(string from, string to, bool waitAccess = false);
    }
}