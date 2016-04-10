using System;

namespace MineLib.Core.Exceptions
{
    public class MineLibClientException : Exception
    {
        public MineLibClientException() : base() { }

        public MineLibClientException(string message) : base(message) { }

        public MineLibClientException(string format, params object[] args) : base(string.Format(format, args)) { }

        public MineLibClientException(string message, Exception innerException) : base(message, innerException) { }

        public MineLibClientException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException) { }
    }
}
