using System;
using System.Threading.Tasks;

using Aragas.Core.Data;

using MineLib.Core.Data.Structs;

namespace MineLib.Core.Interfaces
{
    public interface ISending { }

    public abstract class SendingArgs
    {
        public Action<IPacket> SendPacket { get; private set; }
        public Func<IPacket, Task> SendPacketAsync { get; private set; }

        public void RegisterSending(Action<IPacket> sendPacket, Func<IPacket, Task> sendPacketAsync)
        {
            SendPacket += sendPacket;
            SendPacketAsync += sendPacketAsync;
        }
    }

    public struct ConnectToServer : ISending { }
    public class ConnectToServerArgs : SendingArgs
    {
        public string ServerHost { get; set; }
        public ushort Port { get; set; }
        public string Username { get; private set; }
        public VarInt Protocol { get; set; }

        public ConnectToServerArgs(string serverHost, ushort port, string username, VarInt protocol)
        {
            ServerHost = ServerHost;
            Port = port;

            Username = username;

            Protocol = protocol;
        }
    }

    public struct KeepAlive : ISending { }
    public class KeepAliveArgs : SendingArgs
    {
        public int KeepAlive { get; private set; }

        public KeepAliveArgs(int value)
        {
            KeepAlive = value;
        }
    }

    public struct SendClientInfo : ISending { }
    public class SendClientInfoArgs : SendingArgs { }

    public struct Respawn : ISending { }
    public class RespawnArgs : SendingArgs { }

    public struct PlayerMoved : ISending { }
    public class PlayerMovedArgs : SendingArgs
    {
        public PlaverMovedMode Mode { get; private set; }
        public IPlaverMovedData Data { get; private set; }

        public PlayerMovedArgs(IPlaverMovedData data)
        {
            {
                var type = data as PlaverMovedDataOnGround;
                if (type != null)
                    Mode = PlaverMovedMode.OnGround;
            }

            {
                var type = data as PlaverMovedDataVector3;
                if (type != null)
                    Mode = PlaverMovedMode.Vector3;
            }

            {
                var type = data as PlaverMovedDataYawPitch;
                if (type != null)
                    Mode = PlaverMovedMode.YawPitch;
            }

            {
                var type = data as PlaverMovedDataAll;
                if (type != null)
                    Mode = PlaverMovedMode.All;
            }

            Data = data;
        }

        public PlayerMovedArgs(PlaverMovedMode mode, IPlaverMovedData data)
        {
            Mode = mode;

            Data = data;
        }
    }

    public struct PlayerSetRemoveBlock : ISending {  }
    public class PlayerSetRemoveBlockArgs : SendingArgs
    {
        public PlayerSetRemoveBlockMode Mode { get; private set; }
        public IPlayerSetRemoveBlockData Data { get; private set; }

        public PlayerSetRemoveBlockArgs(IPlayerSetRemoveBlockData data)
        {
            {
                var type = data as PlayerSetRemoveBlockDataDig;
                if (type != null)
                    Mode = PlayerSetRemoveBlockMode.Dig;
            }

            {
                var type = data as PlayerSetRemoveBlockDataPlace;
                if (type != null)
                    Mode = PlayerSetRemoveBlockMode.Place;
            }

            {
                var type = data as PlayerSetRemoveBlockDataRemove;
                if (type != null)
                    Mode = PlayerSetRemoveBlockMode.Remove;
            }

            Data = data;
        }

        public PlayerSetRemoveBlockArgs(PlayerSetRemoveBlockMode mode, IPlayerSetRemoveBlockData data)
        {
            Mode = mode;

            Data = data;
        }
    }

    public struct SendMessage : ISending { }
    public class SendMessageArgs : SendingArgs
    {
        public string Message { get; private set; }

        public SendMessageArgs(string message)
        {
            Message = message;
        }
    }

    public struct PlayerHeldItem : ISending { }
    public class PlayerHeldItemArgs : SendingArgs
    {
        public short Slot { get; private set; }

        public PlayerHeldItemArgs(short slot)
        {
            Slot = slot;
        }
    }
}
