using Aragas.Core.Data;

namespace MineLib.Core.Events.SendingEvents
{
    public class ConnectToServerEvent : SendingEvent { }

    public class ConnectToServerArgs : SendingEventArgs
    {
        public string ServerHost { get; private set; }
        public ushort Port { get; private set; }
        public string Username { get; private set; }
        public VarInt Protocol { get; private set; }

        public ConnectToServerArgs(string serverHost, ushort port, string username, VarInt protocol)
        {
            ServerHost = serverHost;
            Port = port;

            Username = username;

            Protocol = protocol;
        }
    }
}