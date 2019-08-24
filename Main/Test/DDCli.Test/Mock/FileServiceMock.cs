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
        public string AbsoluteCurrentPathReturn { get; set; }

        public string AbsolutePathReturn { get; set; }
        public string CreatedDirectory { get; set; }

        public string ClonedDirectorySource { get; set; }
        public string ClonedDirectoryDestination { get; set; }


        public string FileDirectoryReturn { get; set; }

        public string ZippedPath { get; set; }
        public string UnzippedPath { get; set; }

        public bool IsDirectoryReturn { get; set; }

        public bool IsFileReturn { get; set; }


        public byte[] ReadAllBytesReturn { get; set; }

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

        public string GetAbsoluteCurrentPath(string absoluteRelativePath)
        {
            return AbsoluteCurrentPathReturn;
        }

        public string GetCurrentPath()
        {
            throw new NotImplementedException();
        }

        public DDTemplateConfig GetTemplateConfig(string path)
        {
            return DDTemplateConfigReturn;
        }



        public string MovedFileFrom { get; set; }
        public string MovedFileTo { get; set; }
        public void MoveFile(string from, string to, bool waitAccess = false)
        {
            MovedFileFrom = from;
            MovedFileTo = to;
        }




        public void ReplaceStringInPaths(string rootPath, string oldValue, string newValue, bool includeDirectories, bool includeFileNames, bool includeFileContents, string filePattern)
        {


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



        public DDPipelineConfig GetPipelineConfig(string path)
        {
            throw new NotImplementedException();
        }

        public bool IsFile(string path)
        {
            return IsFileReturn;
        }

        public bool IsDirectory(string path)
        {
            return IsDirectoryReturn;
        }

        public string GetFileDirectory(string path)
        {
            return FileDirectoryReturn;
        }

        public string GetAbsolutePath(string absoluteRelativePath, string basePath)
        {
            return AbsolutePathReturn;
        }

        public void ZipDierctory(string path)
        {
            ZippedPath = path;
        }

        public void ZipFile(string path, string zipName = null)
        {
            ZippedPath = path;
        }

        public void UnZipPath(string path, string destinationFolder = null)
        {
            UnzippedPath = path;
        }


        public string ReplacedFilesContentsPath { get; set; }
        public string ReplacedFilesNamesPath { get; set; }
        public string ReplacedSubDirectoriesPath { get; set; }
        public string ReplacedStringInPathsRootPath { get; set; }
        public string ReplacedStringInPathsOldValue { get; set; }
        public string ReplacedStringInPathsNewValue { get; set; }

        public void ReplaceFilesContents(string rootPath, string oldValue, string newValue, string filePattern, int times = -1)
        {
            ReplacedFilesContentsPath = rootPath;
            ReplacedStringInPathsOldValue = oldValue;
            ReplacedStringInPathsNewValue = newValue;
        }

        public void ReplaceFilesContentsWithRegexPattern(string rootPath, string oldValueRegexPattern, string newValue, string filePattern, int times = -1)
        {
            ReplacedFilesContentsPath = rootPath;
            ReplacedStringInPathsOldValue = oldValueRegexPattern;
            ReplacedStringInPathsNewValue = newValue;
        }

        public void ReplaceAllFilesName(string rootPath, string oldValue, string newValue)
        {
            ReplacedFilesNamesPath = rootPath;
            ReplacedStringInPathsOldValue = oldValue;
            ReplacedStringInPathsNewValue = newValue;
        }

        public void ReplaceAllSubDirectoriesName(string rootPath, string oldValue, string newValue)
        {
            ReplacedSubDirectoriesPath = rootPath;
            ReplacedStringInPathsOldValue = oldValue;
            ReplacedStringInPathsNewValue = newValue;
        }

        public byte[] ReadAllBytes(string path)
        {
            return ReadAllBytesReturn;
        }


        public string RenamedOldFolder { get; set; }
        public string RenamedNewFolder { get; set; }
        public void RenameFolder(string oldPath, string newPath)
        {
            RenamedOldFolder = oldPath;
            RenamedNewFolder = newPath;
        }


        public bool IsValidPipelineConfigurationReturn { get; set; }

        public bool IsValidPipelineConfiguration(DDPipelineConfig config)
        {
            return IsValidPipelineConfigurationReturn;
        }



        public bool ExistsPathReturn { get; set; }

        public bool ExistsPath(string path)
        {
            return ExistsPathReturn;
        }


        public string MovedSourceFolder { get; set; }
        public string MovedDestionationFolder { get; set; }
        public void MoveFolderContent(string sourceFolder, string destinationFolder, string filePattern)
        {
            MovedSourceFolder = sourceFolder;
            MovedDestionationFolder = destinationFolder;
        }
    }
}
