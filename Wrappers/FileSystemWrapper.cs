using System;
using System.IO;

using Newtonsoft.Json;

using PCLStorage;

namespace MineLib.Core.Wrappers
{
    public interface IFileSystem
    {
        IFolder ContentFolder { get; }
        IFolder ProtocolsFolder { get;  }
        IFolder SettingsFolder { get; }
        IFolder LogFolder { get; }
    }

    public static class FileSystemWrapper
    {
        private static IFileSystem _instance;
        public static IFileSystem Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static IFolder ContentFolder { get { return Instance.ContentFolder; } }
        public static IFolder ProtocolsFolder { get { return Instance.ProtocolsFolder; } }
        public static IFolder SettingsFolder { get { return Instance.SettingsFolder; } }
        public static IFolder LogFolder { get { return Instance.LogFolder; } }

        public static T LoadSettings<T>(string filename, T defaultValue = default(T))
        {
            T settings;
            using (var stream = Instance.SettingsFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                var file = reader.ReadToEnd();
                if (string.IsNullOrEmpty(file))
                {
                    settings = defaultValue;
                    writer.Write(JsonConvert.SerializeObject(settings, Formatting.Indented));
                }
                else
                {
                    try { settings = JsonConvert.DeserializeObject<T>(file); }
                    catch (JsonReaderException) { settings = defaultValue; }
                }
            }

            return settings;
        }
    }

}
