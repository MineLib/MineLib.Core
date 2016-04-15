namespace MineLib.Core.Client
{
    public interface IStatusClient
    {
        ServerInfo GetServerInfo(string ip, ushort port, int protocolVersion);

        long GetPing(string ip, ushort port);
    }
}
