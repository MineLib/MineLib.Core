using System;

namespace MineLib.Core.Exceptions
{
    public class NetworkHandlerException : Exception
    {
        public NetworkHandlerException() : base() { }

        public NetworkHandlerException(string message) : base(message) { }

        public NetworkHandlerException(string format, params object[] args) : base(string.Format(format, args)) { }

        public NetworkHandlerException(string message, Exception innerException) : base(message, innerException) { }

        public NetworkHandlerException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }
    }
}
