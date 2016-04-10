namespace MineLib.Core.Interfaces
{
    public interface IStatusClient
    {
        IServerInfo GetServerInfo(string ip, ushort port, int protocolVersion);

        long GetPing(string ip, ushort port);
    }
}
