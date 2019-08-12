using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Test.Mock
{
    public class FileServiceMock : IFileService
    {

        public bool ExistsFileReturn { get; set; }
        public bool ExistsDirectoryReturn { get; set; }
        public bool ExistsTemplateConfigFileReturn { get; set; }
        public DDTemplateConfig DDTemplateConfigReturn { get; set; }
        public string AbsolutePathReturn { get; set; }

        public string CreatedDirectory { get; set; }

        public string ClonedDirectorySource { get; set; }
        public string ClonedDirectoryDestination { get; set; }


        public FileServiceMock()
        {

        }

        public List<string> CloneDirectory(string sourceFolder, string destinationFolder, List<string> ignorePathPatterns)
        {
            ClonedDirectorySource = sourceFolder;
            ClonedDirectoryDestination = destinationFolder;
            return new List<string>();
        }

        public void CreateDirectory(string path, bool reCreateIfExists = false)
        {
            CreatedDirectory = path;
        }

        public bool ExistsDirectory(string path)
        {
            return ExistsDirectoryReturn;
        }

        public bool ExistsFile(string path)
        {
            return ExistsFileReturn;
        }

        public bool ExistsFileInDirectory(string folder, string filename)
        {
            throw new NotImplementedException();
        }

        public bool ExistsTemplateConfigFile(string path)
        {
            return ExistsTemplateConfigFileReturn;
        }

        public string GetAbsolutePath(string absoluteRelativePath)
        {
            return AbsolutePathReturn;
        }

        public string GetCurrentPath()
        {
            throw new NotImplementedException();
        }

        public DDTemplateConfig GetTemplateConfig(string path)
        {
            return DDTemplateConfigReturn;
        }

        public void MoveFile(string from, string to, bool waitAccess = false)
        {
            throw new NotImplementedException();
        }


        public string ReplacedStringInPathsRootPath { get; set; }
        public string ReplacedStringInPathsOldValue { get; set; }
        public string ReplacedStringInPathsNewValue { get; set; }

        public void ReplaceStringInPaths(string rootPath, string oldValue, string newValue, bool includeDirectories, bool includeFileNames, bool includeFileContents, string filePattern)
        {
            ReplacedStringInPathsRootPath = rootPath;
            ReplacedStringInPathsOldValue = oldValue;
            ReplacedStringInPathsNewValue = newValue;
        }

        public List<string> SearchDirectories(string path, string name, bool includeSubdirectories)
        {
            throw new NotImplementedException();
        }

        public List<string> SearchDirectoriesInPath(string rootPath, string pattern)
        {
            throw new NotImplementedException();
        }

        public List<string> SearchFilesInPath(string rootPath, string pattern)
        {
            throw new NotImplementedException();
        }
    }
}
