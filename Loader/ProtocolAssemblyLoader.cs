using System;
using System.Collections.Generic;
using System.Reflection;

using Aragas.Core.Extensions;
using Aragas.Core.Wrappers;

using PCLStorage;

namespace MineLib.Core.Loader
{
    public static class ProtocolAssemblyLoader
    {
        public static Type GetProtocolType(string assemblyPath)
        {
            Type protocol = null;


            var assemblyFolder = FileSystemWrapper.AssemblyFolder;
            if (assemblyFolder != null && assemblyFolder.CheckExistsAsync(assemblyPath).Result == ExistenceCheckResult.FileExists)
            {
                using (var stream = assemblyFolder.GetFileAsync(assemblyPath).Result.OpenAsync(FileAccess.Read).Result)
                {
                    var asm = AppDomainWrapper.LoadAssembly(stream.ReadFully());
                    if (asm != null)
                        foreach (var typeInfo in new List<TypeInfo>(asm.DefinedTypes))
                            if (typeInfo.IsSubclassOf(typeof(Protocol)))
                                protocol = typeInfo.AsType();
                }
            }

            #region Debug
            if (protocol == null)
            {
                var asm = Assembly.Load(new AssemblyName("ProtocolModern_1.7.10"));
                if (asm != null)
                    foreach (var typeInfo in new List<TypeInfo>(asm.DefinedTypes))
                        if (typeInfo.IsSubclassOf(typeof(Protocol)))
                            protocol = typeInfo.AsType();
            }
            #endregion Debug


            return protocol;
        }
    }
}