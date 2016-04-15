using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using Aragas.Core.Extensions;
using Aragas.Core.Wrappers;

using PCLStorage;

namespace MineLib.Core.Loader
{
    public static class AssemblyParser
    {
        private static bool FitsMask(string sFileName, string sFileMask)
        {
            var mask = new Regex(sFileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
            return mask.IsMatch(sFileName);
        }
        public static IEnumerable<AssemblyInfo> GetAssemblyInfos(string mask)
        {
            var files = FileSystemWrapper.AssemblyFolder.GetFilesAsync().Result;
            return files.Where(file => FitsMask(file.Name, mask)).Select(assembly => new AssemblyInfo(assembly.Path));
        }

        public static Type FindType<TLoadType>(AssemblyInfo assemblyInfo, string debugDefault = "")
        {
            Type protocol = null;

            var assemblyFile = FileSystem.Current.GetFileFromPathAsync(assemblyInfo.FilePath).Result;
            if (assemblyFile != null)
            {
                using (var stream = assemblyFile.OpenAsync(FileAccess.Read).Result)
                {
                    var asm = AppDomainWrapper.LoadAssembly(stream.ReadFully());
                    if (asm != null)
                        protocol = asm.DefinedTypes.Single(typeInfo => typeInfo.IsSubclassOf(typeof(TLoadType))).AsType();
                }
            }

            /*
            var assemblyFolder = FileSystemWrapper.AssemblyFolder;
            if (assemblyFolder != null && assemblyFolder.CheckExistsAsync(assemblyInfo.FileName).Result == ExistenceCheckResult.FileExists)
            {
                using (var stream = assemblyFolder.GetFileAsync(assemblyInfo.FileName).Result.OpenAsync(FileAccess.Read).Result)
                {
                    var asm = AppDomainWrapper.LoadAssembly(stream.ReadFully());
                    if (asm != null)
                        protocol = asm.DefinedTypes.Single(typeInfo => typeInfo.IsSubclassOf(typeof (TLoadType))).AsType();
                }
            }
            */

            #region Debug
            if (protocol == null)
            {
                var asm = Assembly.Load(new AssemblyName(debugDefault));
                if (asm != null)
                    protocol = asm.DefinedTypes.Single(typeInfo => typeInfo.IsSubclassOf(typeof (TLoadType))).AsType();
            }
            #endregion Debug


            return protocol;
        }
    }
}