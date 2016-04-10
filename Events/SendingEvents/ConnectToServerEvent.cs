namespace MineLib.Core.Events.SendingEvents
{
    public class ConnectToServer : SendingEvent
    {
        public string ServerHost { get; private set; }
        public ushort Port { get; private set; }
        public string Username { get; private set; }

        public ConnectToServer(string serverHost, ushort port, string username)
        {
            ServerHost = serverHost;
            Port = port;

            Username = username;
        }
    }
}