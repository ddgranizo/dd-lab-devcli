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

        bool IsValidPipelineConfiguration(DDPipelineConfig config);
        DDTemplateConfig GetTemplateConfig(string path);

        DDPipelineConfig GetPipelineConfig(string path);

        List<string> CloneDirectory(string sourceFolder, string destinationFolder, List<string> ignorePathPatterns);


        string GetAbsoluteCurrentPath(string absoluteRelativePath);
        string GetAbsolutePath(string absoluteRelativePath, string basePath);
        void CreateDirectory(string path, bool reCreateIfExists = false);


        void ReplaceFilesContents(string rootPath, string oldValue, string newValue, string filePattern);

        void ReplaceFilesContentsWithRegexPattern(string rootPath, string oldValueRegexPattern, string newValue, string filePattern);

        void ReplaceAllFilesName(string rootPath, string oldValue, string newValue);

        void ReplaceAllSubDirectoriesName(string rootPath, string oldValue, string newValue);

        List<string> SearchDirectoriesInPath(string rootPath, string pattern);

        List<string> SearchFilesInPath(string rootPath, string pattern);

        void MoveFile(string from, string to, bool waitAccess = false);

        bool IsFile(string path);

        bool IsDirectory(string path);

        string GetFileDirectory(string path);

        void ZipDierctory(string path);

        void ZipFile(string path, string zipName = null);

        void UnZipPath(string path, string destinationFolder = null);

        byte[] ReadAllBytes(string path);

        void RenameFolder(string oldPath, string newPath);
    }
}