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
using System.Text.RegularExpressions;

namespace DDCli.Services
{
    public class FileService : IFileService
    {
        public FileService()
        {
        }


        public string WriteFile(string path, string content, bool overwrite)
        {
            if (ExistsFile(path))
            {
                if (overwrite)
                {
                    File.WriteAllText(path, content);
                    return path;
                }
                else
                {
                    var newPath = GetNewNameForRepeatedFile(path);
                    File.WriteAllText(newPath, content);
                    return newPath;
                }
            }
            File.WriteAllText(path, content);
            return path;
        }


        private string GetNewNameForRepeatedFile(string path)
        {
            string newPath = path;
            bool more;
            int counter = 0;
            var info = new FileInfo(path);
            var basePath = info.Directory;
            var fileName = info.Name;
            var extension = info.Extension;
            var fileWithoutExtension = fileName.Substring(0, fileName.Length - extension.Length);
            do
            {
                newPath = counter == 0
                    ? newPath
                    : $"{basePath}\\{fileWithoutExtension} ({counter}){info.Extension}";
                counter++;
                more = ExistsFile(newPath);
            } while (more);
            return newPath;
        }

        public void WriteFile(string path, string content)
        {
            File.WriteAllText(path, content);
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

        public void ReplaceFilesContents(string rootPath, string oldValue, string newValue, string filePattern, int times = -1)
        {
            var filesInPath = GetFilesInPath(rootPath, filePattern);
            foreach (var filePath in filesInPath)
            {
                var fileContent = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(fileContent))
                {
                    var regex = new Regex(Regex.Escape(oldValue));
                    if (times < 1)
                    {
                        fileContent = regex.Replace(fileContent, newValue);
                    }
                    else
                    {
                        fileContent = regex.Replace(fileContent, newValue, times);
                    }
                }
                File.WriteAllText(filePath, fileContent);
            }
        }


        public void ReplaceFilesContentsWithRegexPattern(string rootPath, string oldValueRegexPattern, string newValue, string filePattern, int times = -1)
        {
            var filesInPath = GetFilesInPath(rootPath, filePattern);
            foreach (var filePath in filesInPath)
            {
                var fileContent = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(fileContent))
                {
                    fileContent = Regex.Replace(fileContent, oldValueRegexPattern, newValue);
                }
                File.WriteAllText(filePath, fileContent);
            }
        }


        private List<string> GetFilesInPath(string rootPath, string filePattern)
        {
            if (string.IsNullOrEmpty(filePattern))
            {
                filePattern = "*.*";
            }
            if (IsDirectory(rootPath))
            {
                var filesInPattern = SearchFilesInPath(rootPath, filePattern);
                return filesInPattern;
            }
            else
            {
                return new List<string>() { rootPath };
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
                MoveFile(oldFilename, newFileName);
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


        public void MoveFile(string from, string to)
        {
            var toComposed = to;
            if (!Path.IsPathRooted(to))
            {
                toComposed = string.Format(@"{0}\{1}", to, new FileInfo(from).Name);
            }
            if (File.Exists(toComposed))
            {
                File.Delete(toComposed);
            }
            File.Move(from, toComposed);
        }



        public DDPipelineConfig GetPipelineConfig(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
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
            if (File.Exists(destinationFileName))
            {
                File.Delete(destinationFileName);
            }
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
            if (File.Exists(destinationZipPath))
            {
                File.Delete(destinationZipPath);
            }
            System.IO.Compression.ZipFile.CreateFromDirectory(tempFolderPath, destinationZipPath);
            Directory.Delete(tempFolderPath);
        }

        public void UnZipPath(string path, string destinationFolder = null)
        {
            var fileDirectoryPath = GetFileDirectory(path);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            var processedDestinationFolder = destinationFolder ?? fileNameWithoutExtension;
            var absolutePath = GetAbsolutePath(processedDestinationFolder, fileDirectoryPath);
            if (Directory.Exists(absolutePath))
            {
                Directory.Delete(absolutePath, true);
            }
            System.IO.Compression.ZipFile.ExtractToDirectory(path, absolutePath);
        }

        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public void RenameFolder(string oldPath, string newPath)
        {
            var absoluteOldPath = GetAbsoluteCurrentPath(oldPath);
            var directoryOldPathInfo = new DirectoryInfo(absoluteOldPath);
            var absoluteNewPath = newPath;
            if (!Path.IsPathRooted(newPath))
            {
                absoluteNewPath = $"{directoryOldPathInfo.Parent.FullName}\\{newPath}";
            }
            var directoryNewPathInfo = new DirectoryInfo(absoluteNewPath);
            if (directoryOldPathInfo.Parent.FullName != directoryNewPathInfo.Parent.FullName)
            {
                throw new InvalidParamException("Folders must be in the same path");
            }
            if (Directory.Exists(absoluteNewPath))
            {
                Directory.Delete(absoluteNewPath, true);
            }
            Directory.Move(absoluteOldPath, absoluteNewPath);
        }

        public bool IsValidPipelineConfiguration(DDPipelineConfig config)
        {
            return config.Commands != null && !string.IsNullOrEmpty(config.PipelineName);
        }

        public bool ExistsPath(string path)
        {
            bool existsFile = File.Exists(path);
            bool existsDirectory = Directory.Exists(path);
            return existsFile | existsDirectory;
        }

        public void MoveFolderContent(string sourceFolder, string destinationFolder, string filePattern)
        {
            var filesForMove = GetFilesInPath(sourceFolder, filePattern);
            foreach (var file in filesForMove)
            {
                MoveFile(file, destinationFolder);
            }
        }

        public void WriteAllBytes(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }
    }
}
