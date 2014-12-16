using System;
using System.IO;
using System.Reflection;

namespace MineLib.Network.Module
{
    public static class ModuleLoader
    {
        public static T CreateModule<T>(string file)
        {
            var plugin = default(T);

            if (File.Exists(file))
            {
                var asm = Assembly.LoadFile(file);

                if (asm != null)
                {
                    for (int i = 0; i < asm.GetTypes().Length; i++)
                    {
                        var type = (Type) asm.GetTypes().GetValue(i);

                        if (IsImplementationOf(type, typeof(IModule)))
                            plugin = (T) Activator.CreateInstance(type);
                        
                    }
                }
            }

            return plugin;
        }

        private static bool IsImplementationOf(Type type, Type @interface)
        {
            var interfaces = type.GetInterfaces();

            for (int i = 0; i < interfaces.Length; i++)
            {
                if (IsSubtypeOf(ref interfaces[i], @interface))
                    return true;
            }

            return false;
        }

        private static bool IsSubtypeOf(ref Type a, Type b)
        {
            if (a == b)
                return true;
            
            if (a.IsGenericType)
            {
                a = a.GetGenericTypeDefinition();

                if (a == b)
                    return true;              
            }

            return false;
        }
    }
}