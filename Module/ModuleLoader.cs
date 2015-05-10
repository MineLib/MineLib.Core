using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PCLStorage;

namespace MineLib.Core.Module
{
    public delegate Assembly LoadAssembly(object sender, byte[] assembly);
    public delegate IFolder Storage(object sender);

    public static class ModuleLoader
    {
        public static event LoadAssembly LoadAssembly;
        public static event Storage GetStorage;

        public static T CreateModule1<T>(string file) where T : class
        {
            var plugin = default(T);

            if (FileSystem.Current.LocalStorage.CheckExistsAsync(file).Result == ExistenceCheckResult.FileExists)
            {
                var asm = Assembly.Load(new AssemblyName(file.Replace(".dll", "")));

                if (asm != null)
                    foreach (var type in new List<Type>(asm.ExportedTypes))
                        if (type.Name == "Protocol")
                            plugin = Activator.CreateInstance(type) as T;
            }

            return plugin;
        }

        public static T CreateModule<T>(string file)
        {
            var plugin = default(T);

            //if (GetStorage != null && GetStorage().CheckExistsAsync(file).Result == ExistenceCheckResult.FileExists)
            //{
                var asm = Assembly.Load(new AssemblyName("ProtocolModern.Portable"));
                //var asm = Assembly.Load(new AssemblyName("ProtocolClassic.Portable"));
                //var asm = LoadAssembly(null, GetStorage(null).GetFileAsync(file).Result.OpenAsync(FileAccess.Read).Result.ReadFully());

                if (asm != null)
                    foreach (var typeInfo in new List<TypeInfo>(asm.DefinedTypes))
                        foreach (var type in new List<Type>(typeInfo.ImplementedInterfaces))
                            if (type == typeof (T))
                                plugin = (T) Activator.CreateInstance(typeInfo.AsType());
            //}

            return plugin;
        }

        private static byte[] ReadFully(this Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                
                return ms.ToArray();
            }
        }
    }
}