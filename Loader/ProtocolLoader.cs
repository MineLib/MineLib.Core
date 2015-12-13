using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Aragas.Core.Wrappers;

using MineLib.Core.Protocols;

using PCLStorage;

namespace MineLib.Core.Loader
{
    public static class ProtocolAssemblyLoader
    {
        public static Type GetProtocolType(string assemblyPath)
        {
            Type protocol = null;

#if DEBUG //|| !DEBUG
            //var asm = Assembly.Load(new AssemblyName("ProtocolTrueCraft"));
            var asm = Assembly.Load(new AssemblyName("ProtocolModern"));
            //var asm = Assembly.Load(new AssemblyName("ProtocolClassic"));
            if (asm != null)
                foreach (var typeInfo in new List<TypeInfo>(asm.DefinedTypes))
                    if (typeInfo.IsSubclassOf(typeof(Protocol)))
                        protocol = typeInfo.AsType();
#elif DEBUG1
            var assemblyFolder = FileSystemWrapper.AssemblyFolder;
            if (assemblyFolder != null && assemblyFolder.CheckExistsAsync(assemblyPath).Result == ExistenceCheckResult.FileExists)
            {
                using (var stream = assemblyFolder.GetFileAsync(assemblyPath).Result.OpenAsync(FileAccess.Read).Result)
                {
                    var asm = AppDomainWrapper.LoadAssembly(stream.ReadFully());

                    foreach (var typeInfo in new List<TypeInfo>(asm.DefinedTypes))
                        if (typeInfo.IsSubclassOf(typeof(Protocol)))
                            plugin = typeInfo.AsType();
                }
            }
#endif

            return protocol;
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