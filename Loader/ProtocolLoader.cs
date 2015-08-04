using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using MineLib.Core.Interfaces;
using MineLib.Core.Wrappers;

using PCLStorage;

namespace MineLib.Core.Loader
{
    public static class ProtocolAssemblyLoader
    {
        public static IProtocol CreateProtocol(string assemblyPath)
        {
            var plugin = default(IProtocol);

#if DEBUG
            //var asm = Assembly.Load(new AssemblyName("ProtocolTrueCraft"));
            var asm = Assembly.Load(new AssemblyName("ProtocolModern"));
            //var asm = Assembly.Load(new AssemblyName("ProtocolClassic"));
            if (asm != null)
                    foreach (var typeInfo in new List<TypeInfo>(asm.DefinedTypes))
                        foreach (var type in new List<Type>(typeInfo.ImplementedInterfaces))
                            if (type == typeof(IProtocol))
                                plugin = (IProtocol) Activator.CreateInstance(typeInfo.AsType());
#elif DEBUG1
            var assemblyFolder = FileSystemWrapper.Instance.AssemblyFolder;
            if (assemblyFolder != null && assemblyFolder.CheckExistsAsync(assemblyPath).Result == ExistenceCheckResult.FileExists)
            {
                using (var stream = assemblyFolder.GetFileAsync(assemblyPath).Result.OpenAsync(FileAccess.Read).Result)
                {
                    var asm = AppDomainWrapper.Instance.LoadAssembly(stream.ReadFully());

                    foreach (var typeInfo in new List<TypeInfo>(asm.DefinedTypes))
                        foreach (var type in new List<Type>(typeInfo.ImplementedInterfaces))
                            if (type == typeof (IProtocol))
                                plugin = (IProtocol) Activator.CreateInstance(typeInfo.AsType());
                }
            }
#endif

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