using System;
using MineLib.Network.Data;

namespace MineLib.Network.IO
{
    public enum ConnectionState
    {
        None,
        Connecting,
        Connected,
    }

    public interface IPacketSender
    {
        event Action ConnectedToServer;
        event Action Disconnected;

        ConnectionState ConnectionState { get; set; }

        IPacketSender Create(IMinecraftClient minecraft, IProtocol protocol);

        void ConnectToServer();

        void KeepAlive(int value);

        void SendClientInfo();

        void Respawn();

        /// <summary>
        /// 0x00 - OnGround, 0x01 - 0x00 & Vector3, 0x02 - 0x00 & Yaw Pitch, 0x03 - 0x01 & 0x02.
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="vector3"></param>
        /// <param name="onGround"></param>
        /// <param name="yaw"></param>
        /// <param name="pitch"></param>
        void PlayerMoved(byte mode, Vector3 vector3 = default(Vector3), bool onGround = false, float yaw = 0f, float pitch = 0f);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode">0x00 Block destroyed, 0x01 Block placed, 0x02 Block digging</param>
        /// <param name="location"></param>
        /// <param name="blockID"></param>
        /// <param name="status"></param>
        /// <param name="face"></param>
        /// <param name="direction"></param>
        /// <param name="slot"></param>
        /// <param name="crosshair"></param>
        void PlayerSetRemoveBlock(byte mode, Position location = default(Position), int blockID = 0, byte status = 0x00,byte face = 0x00, 
            byte direction = 0x00, ItemStack slot = default(ItemStack), Position crosshair = default(Position));

        void SendMessage(string message);

    }
}
