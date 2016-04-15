using System.Collections.Generic;

namespace MineLib.Core.Client
{
    public class ServerInfo
    {
        public string Name { get; }
        public ServerAddress Address { get; set; }

        public IServerResponse ServerResponse { get; set; }
    }
    public class ServerAddress
    {
        public string IP { get; set; }

        public ushort Port { get; set; }
    }
    public interface IServerResponse
    {
        long Ping { get; }

        byte[] Icon { get; }

        string Brand { get; }
        int Protocol { get; }

        string Description { get; }

        int Max { get; }
        int Online { get; }
        List<IServerPlayer> Players { get; }
    }
    public interface IServerPlayer
    {
        string Name { get; }
    }
}
