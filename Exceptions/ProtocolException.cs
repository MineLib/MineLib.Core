using System;

namespace MineLib.Core.Exceptions
{
    public class ProtocolException : Exception
    {
        public ProtocolException() : base() { }

        public ProtocolException(string message) : base(message) { }

        public ProtocolException(string format, params object[] args) : base(string.Format(format, args)) { }

        public ProtocolException(string message, Exception innerException) : base(message, innerException) { }

        public ProtocolException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }
    }
}
