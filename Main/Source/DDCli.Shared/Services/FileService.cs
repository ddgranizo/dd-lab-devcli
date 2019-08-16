using DDCli.Interfaces;
using DDCli.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.IO.Compression;
using DDCli.Exceptions;

namespace DDCli.Services
{
    public class FileService : IFileService
    {
        public FileService()
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



        public bool ExistsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public bool ExistsFile(string path)
        {
            return File.Exists(path);
        }

        public bool ExistsTemplateConfigFile(string path)
        {
            return ExistsFileInDirectory(path, Definitions.TemplateConfigFilename);
        }

        public bool ExistsFileInDirectory(string folder, string fileName)
        {
            return ExistsFile(ConcatDirectoryAndFile(folder, fileName));
        }

        private static string ConcatDirectoryAndFile(string folder, string fileName)
        {
            var folderCopy = folder;
            if (folderCopy.Last() != '\\' && folderCopy.Last() != '/')
            {
                folderCopy = string.Format("{0}\\", folderCopy);
            }
            return string.Format("{0}{1}", folderCopy, fileName);
        }

        public DDTemplateConfig GetTemplateConfig(string path)
        {
            try
            {
                var completePath = ConcatDirectoryAndFile(path, Definitions.TemplateConfigFilename);
                var json = File.ReadAllText(completePath);
                var parsed = JsonConvert.DeserializeObject<DDTemplateConfig>(json);
                return parsed;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<string> CloneDirectory(
            string sourceFolder,
            string destinationFolder,
            List<string> ignorePathPatterns)
        {
            var allFiles = Directory.EnumerateFiles
                (sourceFolder, "*.*", SearchOption.AllDirectories);
            var ignoredFiles = new List<string>();
            foreach (var pattern in ignorePathPatterns)
            {
                var ignored = Directory.EnumerateFiles
                    (sourceFolder, pattern, SearchOption.AllDirectories);
                ignoredFiles.AddRange(ignored);
            }
            var filesForClone = new List<string>();
            foreach (var file in allFiles)
            {
                if (ignoredFiles.IndexOf(file) == -1)
                {
                    filesForClone.Add(file.Substring(sourceFolder.Length));
                }
            }
            var destinationClonedFiles = new List<string>();
            foreach (var file in filesForClone)
            {
                var sourceCompletePath = string.Format("{0}{1}", sourceFolder, file);
                var destionationCompletePath = string.Format("{0}{1}", destinationFolder, file);
                destinationClonedFiles.Add(destionationCompletePath);
                if (!Directory.Exists(Path.GetDirectoryName(destionationCompletePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destionationCompletePath));
                }
                File.Copy(sourceCompletePath, destionationCompletePath, true);
            }
            return destinationClonedFiles;
        }

        public string GetAbsoluteCurrentPath(string absoluteRelativePath)
        {
            if (Path.IsPathRooted(absoluteRelativePath))
            {
                return absoluteRelativePath;
            }
            var currentPath = GetCurrentPath();
            return string.Format("{0}\\{1}", currentPath, absoluteRelativePath);
        }

        public string GetAbsolutePath(string absoluteRelativePath, string basePath)
        {
            if (Path.IsPathRooted(absoluteRelativePath))
            {
                return absoluteRelativePath;
            }
            var currentPath = basePath;
            return string.Format("{0}\\{1}", currentPath, absoluteRelativePath);
        }


        public void CreateDirectory(string path, bool reCreateIfExists = false)
        {
            if (reCreateIfExists)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    Directory.CreateDirectory(path);
                }
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        public void ReplaceStringInPaths(
            string rootPath,
            string oldValue,
            string newValue,
            bool includeDirectories,
            bool includeFileNames,
            bool includeFileContents,
            string filePattern)
        {
            if (includeDirectories)
            {
                ReplaceAllSubDirectoriesName(rootPath, oldValue, newValue);
            }
            if (includeFileNames)
            {
                ReplaceAllFilesName(rootPath, oldValue, newValue);
            }
            if (includeFileContents)
            {
                ReplaceFilesContents(rootPath, oldValue, newValue, filePattern);
            }
        }

        public void ReplaceFilesContents(string rootPath, string oldValue, string newValue, string filePattern)
        {
            if (string.IsNullOrEmpty(filePattern))
            {
                filePattern = "*.*";
            }
            if (IsDirectory(rootPath))
            {
                var filesInPattern = SearchFilesInPath(rootPath, filePattern);
                foreach (var filePath in filesInPattern)
                {
                    var fileContent = File.ReadAllText(filePath);
                    if (!string.IsNullOrEmpty(fileContent))
                    {
                        fileContent = fileContent.Replace(oldValue, newValue);
                    }
                    File.WriteAllText(filePath, fileContent);
                }
            }
            else
            {
                var fileContent = File.ReadAllText(rootPath);
                if (!string.IsNullOrEmpty(fileContent))
                {
                    fileContent = fileContent.Replace(oldValue, newValue);
                }
                File.WriteAllText(rootPath, fileContent);
            }
        }


        public void ReplaceAllFilesName(string rootPath, string oldValue, string newValue)
        {
            var fileWithName = SearchFilesInPath(rootPath, $"*{oldValue}*.*").FirstOrDefault();
            while (fileWithName != null)
            {
                var fileInfo = new FileInfo(fileWithName);
                var oldName = fileInfo.Name;
                var newName = oldName.Replace(oldValue, newValue);

                var oldFilename = fileInfo.FullName;
                var oldFolder = fileInfo.Directory;
                var newFileName = string.Format("{0}\\{1}", oldFolder, newName);
                MoveFile(oldFilename, newFileName, true);
                fileWithName = SearchFilesInPath(rootPath, $"*{oldValue}*.*").FirstOrDefault();
            }
        }
        public void ReplaceAllSubDirectoriesName(string rootPath, string oldValue, string newValue)
        {
            var directoryWithName = SearchDirectoriesInPath(rootPath, $"*{oldValue}*").FirstOrDefault();
            while (directoryWithName != null)
            {
                var directoryInfo = new DirectoryInfo(directoryWithName);
                var oldName = directoryInfo.Name;
                var newName = oldName.Replace(oldValue, newValue);
                var baseFolder = directoryInfo.Parent.FullName;
                var oldFolder = directoryInfo.FullName;
                var newFolder = string.Format("{0}\\{1}", baseFolder, newName);
                Directory.Move(oldFolder, newFolder);
                directoryWithName = SearchDirectoriesInPath(rootPath, $"*{oldValue}*").FirstOrDefault();
            }
        }

        public List<string> SearchDirectoriesInPath(string rootPath, string pattern)
        {
            return Directory.GetDirectories(rootPath, pattern, SearchOption.AllDirectories).ToList();
        }

        public List<string> SearchFilesInPath(string rootPath, string pattern)
        {
            return pattern.Split('|').SelectMany(k => Directory.EnumerateFiles(rootPath, k, SearchOption.AllDirectories)).ToList();
        }


        public void MoveFile(string from, string to, bool waitAccess = false)
        {
            try
            {
                File.Move(from, to);
            }
            catch (IOException)
            {
                if (!waitAccess)
                {
                    throw;
                }
                Console.WriteLine($"File '{from}' is beeing used by other process. Close the other process and press enter. For cancel this process type 'c'");
                var input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input) && input == "c")
                {
                    throw;
                }
                MoveFile(from, to, waitAccess);
            }
        }

        public bool ExistsPipelineConfigFile(string path)
        {
            return ExistsFileInDirectory(path, Definitions.PipelineConfigFilename);
        }

        public DDPipelineConfig GetPipelineConfig(string path)
        {
            try
            {
                var completePath = ConcatDirectoryAndFile(path, Definitions.PipelineConfigFilename);
                var json = File.ReadAllText(completePath);
                var parsed = JsonConvert.DeserializeObject<DDPipelineConfig>(json);
                return parsed;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsFile(string path)
        {
            return !IsDirectory(path);
        }

        public bool IsDirectory(string path)
        {
            try
            {
                FileAttributes attr = File.GetAttributes(path);
                return attr.HasFlag(FileAttributes.Directory);
            }
            catch (Exception)
            {
                throw new PathNotFoundException(path);
            }
        }

        public string GetFileDirectory(string path)
        {
            return new FileInfo(path).DirectoryName;
        }

        public void ZipDierctory(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            var destinationFileName = $"{info.Parent.FullName}\\{info.Name}.zip";
            System.IO.Compression.ZipFile.CreateFromDirectory(path, destinationFileName);
        }

        public void ZipFile(string path, string zipName = null)
        {
            var fileInfo = new FileInfo(path);
            var fileName = fileInfo.Name;
            var fileDirectoryPath = GetFileDirectory(path);
            var sourceFileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            var processedZipName = zipName ?? $"{sourceFileNameWithoutExtension}.zip";
            var destinationZipPath = $"{fileDirectoryPath}\\{processedZipName}";
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(destinationZipPath);
            var tempFolderPath = $"{fileDirectoryPath}\\{fileNameWithoutExtension}";
            Directory.CreateDirectory(tempFolderPath);
            File.Copy(path, $"{tempFolderPath}\\{fileName}");
            System.IO.Compression.ZipFile.CreateFromDirectory(tempFolderPath, destinationZipPath);
            Directory.Delete(tempFolderPath);
        }

        public void UnZipPath(string path, string destinationFolder = null)
        {
            var fileDirectoryPath = GetFileDirectory(path);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            var processedDestinationFolder = destinationFolder ?? fileNameWithoutExtension;
            var absolutePath = GetAbsolutePath(processedDestinationFolder, fileDirectoryPath);
            System.IO.Compression.ZipFile.ExtractToDirectory(path, absolutePath);
        }

        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
