using System;
using System.Collections.Generic;
using System.Reflection;

namespace MineLib.Core.Wrappers
{
    public interface IAppDomain
    {
        IList<IAssembly> GetAssemblies();
        Assembly LoadAssembly(byte[] assemblyData);
    }

    public interface IAssembly
    {
        string GetName();
        IList<Type> GetTypes();
    }

    public static class AppDomainWrapper
    {
        private static IAppDomain _instance;
        public static IAppDomain Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static IList<IAssembly> GetAssemblies() { return Instance.GetAssemblies(); }
        public static Assembly LoadAssembly(byte[] assemblyData) { return Instance.LoadAssembly(assemblyData); }
    }
}
