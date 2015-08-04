using MineLib.Core.Data;

namespace MineLib.Core.Interfaces
{
    public interface IStatusClient
    {
        ResponseData GetResponseData(string ip, ushort port, int protocolVersion);

        ServerInfo GetServerInfo(string ip, ushort port, int protocolVersion);

        long GetPing(string ip, ushort port);
    }
}
