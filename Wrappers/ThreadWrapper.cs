using System;

namespace MineLib.Core.Wrappers
{
    public interface IThreadWrapper
    {
        int StartThread(Action action, bool isBackground, string threadName);

        void AbortThread(int id);

        bool IsRunning(int id);
    }

    /// <summary>
    /// Exception handling in Task? Newer heard of that.
    /// </summary>
    public static class ThreadWrapper
    {
        private static IThreadWrapper _instance;
        public static IThreadWrapper Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }

        public static int StartThread(Action action, bool isBackground, string threadName) { return Instance.StartThread(action, isBackground, threadName); }

        public static void AbortThread(int id) { Instance.AbortThread(id); }

        public static bool IsRunning(int id) { return Instance.IsRunning(id); }
    }
}
