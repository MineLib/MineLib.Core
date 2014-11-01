using System;
using System.IO;
using System.Reflection;

namespace MineLib.Network.Module
{
    public static class ModuleLoader
    {
        public static T CreateModule<T>(string file)
        {
            T plugin = default(T);

            Type pluginType = null;

            if (File.Exists(file))
            {
                Assembly asm = Assembly.LoadFile(file);

                if (asm != null)
                {
                    for (int i = 0; i < asm.GetTypes().Length; i++)
                    {
                        Type type = (Type)asm.GetTypes().GetValue(i);

                        if (IsImplementationOf(type, typeof(IModule)))
                            plugin = (T)Activator.CreateInstance(type);
                        
                    }
                }
            }

            return plugin;
        }

        private static bool IsImplementationOf(Type type, Type @interface)
        {
            Type[] interfaces = type.GetInterfaces();

            for (int index = 0; index < interfaces.Length; index++)
            {
                Type current = interfaces[index];
                if (IsSubtypeOf(ref current, @interface)) 
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