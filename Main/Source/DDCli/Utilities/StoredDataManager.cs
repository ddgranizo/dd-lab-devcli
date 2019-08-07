using DDCli.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DDCli.Utilities
{
    public static class StoredDataManager
    {

        public const string CliAppDataFolder = "DDCli";
        public const string CliAppDataFile = "Data{0}.xml";

        public static StoredCliData GetStoredData()
        {
            CheckIfExistsFolder();
            CheckIfFileFolder();

            var content = File.ReadAllText(GetFilePath());
            if (!string.IsNullOrEmpty(content))
            {
                StoredCliData data = null;
                using (var stream = System.IO.File.OpenRead(GetFilePath()))
                {
                    var serializer = new XmlSerializer(typeof(StoredCliData));
                    data = serializer.Deserialize(stream) as StoredCliData;
                }
                return data;
            }
            throw new Exception();
        }

        public static void SaveStoredCliData(StoredCliData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            CheckIfExistsFolder();
            MakeFileBackup();

            try
            {
                using (var writer = new System.IO.StreamWriter(GetFilePath()))
                {
                    var serializer = new XmlSerializer(typeof(StoredCliData));
                    serializer.Serialize(writer, data);
                    writer.Flush();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void MakeFileBackup()
        {
            if (File.Exists(GetFilePath()))
            {
                File.Copy(GetFilePath(), GetFilePath(true), true);
            }
        }

        private static void CheckIfExistsFolder()
        {
            if (!Directory.Exists(GetFolderPath()))
            {
                Directory.CreateDirectory(GetFolderPath());
            }
        }

        private static void CheckIfFileFolder()
        {
            if (!File.Exists(GetFilePath()))
            {
                SaveStoredCliData(new StoredCliData());
            }
        }


        public static string GetFilePath(bool backup = false)
        {
            string basePath = GetFolderPath();
            return string.Format("{0}\\{1}", basePath, 
                string.Format(CliAppDataFile, backup ? $"_Backup_{DateTime.Now.ToString("yyyyMMddHHmmss")}" : string.Empty));
        }

        public static string GetFolderPath()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return string.Format("{0}\\{1}", basePath, CliAppDataFolder);
        }
    }
}
