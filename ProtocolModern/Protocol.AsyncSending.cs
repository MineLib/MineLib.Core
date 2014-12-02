using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MineLib.Network;
using MineLib.Network.Data.Structs;
using ProtocolModern.Enum;
using ProtocolModern.Packets;
using ProtocolModern.Packets.Client;
using ProtocolModern.Packets.Client.Login;

namespace ProtocolModern
{
    public partial class Protocol
    {
        private Dictionary<Type, Func<IAsyncSendingParameters, IAsyncResult>> AsyncSendingHandlers { get; set; }

        public void RegisterAsyncSending(Type asyncSendingType, Func<IAsyncSendingParameters, IAsyncResult> method)
        {
            bool any = asyncSendingType.GetInterfaces().Any(p => p == typeof(IAsyncSending));
            if (!any)
                throw new InvalidOperationException("AsyncSending type must implement MineLib.Network.IAsyncSending");

            AsyncSendingHandlers[asyncSendingType] = method;
        }


        private void RegisterSupportedAsyncSendings()
        {
            RegisterAsyncSending(typeof(BeginConnectToServer), BeginConnectToServer);
            RegisterAsyncSending(typeof(BeginKeepAlive), BeginKeepAlive);
            RegisterAsyncSending(typeof(BeginSendClientInfo), BeginSendClientInfo);
            RegisterAsyncSending(typeof(BeginRespawn), BeginRespawn);
            RegisterAsyncSending(typeof(BeginPlayerMoved), BeginPlayerMoved);
            RegisterAsyncSending(typeof(BeginPlayerSetRemoveBlock), BeginPlayerSetRemoveBlock);
            RegisterAsyncSending(typeof(BeginSendMessage), BeginSendMessage);
            RegisterAsyncSending(typeof(BeginPlayerHeldItem), BeginPlayerHeldItem);
        }

        public IAsyncResult DoAsyncSending(Type asyncSendingType, IAsyncSendingParameters parameters)
        {
            bool any = asyncSendingType.GetInterfaces().Any(p => p == typeof(IAsyncSending));
            if (!any)
                throw new InvalidOperationException("AsyncSending type must implement MineLib.Network.IAsyncSending");

            if (!AsyncSendingHandlers.ContainsKey(asyncSendingType))
                return null;

            return AsyncSendingHandlers[asyncSendingType](parameters);
        }


        private IAsyncResult BeginConnectToServer(IAsyncSendingParameters parameters)
        {
            var param = (BeginConnectToServerParameters) parameters;

            State = ConnectionState.JoiningServer;

            BeginSendPacketHandled(new HandshakePacket
            {
                ProtocolVersion = 47,
                ServerAddress = _minecraft.ServerHost,
                ServerPort = _minecraft.ServerPort,
                NextState = NextState.Login,
            }, null, null);

            return BeginSendPacketHandled(new LoginStartPacket { Name = _minecraft.ClientUsername }, param.AsyncCallback, param.State);
        }

        private IAsyncResult BeginKeepAlive(IAsyncSendingParameters parameters)
        {
            var param = (BeginKeepAliveParameters) parameters;

            return BeginSendPacketHandled(new KeepAlivePacket { KeepAlive = param.KeepAlive }, param.AsyncCallback, param.State);
        }

        private IAsyncResult BeginSendClientInfo(IAsyncSendingParameters parameters)
        {
            var param = (BeginSendClientInfoParameters) parameters;

            return BeginSendPacketHandled(new PluginMessagePacket
            {
                Channel = "MC|Brand",
                Data = Encoding.UTF8.GetBytes(_minecraft.ClientBrand)
            }, param.AsyncCallback, param.State);
        }

        private IAsyncResult BeginRespawn(IAsyncSendingParameters parameters)
        {
            var param = (BeginRespawnParameters) parameters;

            return BeginSendPacketHandled(new ClientStatusPacket { Status = ClientStatus.Respawn }, param.AsyncCallback, param.State);
        }

        private IAsyncResult BeginPlayerMoved(IAsyncSendingParameters parameters)
        {
            var param = (BeginPlayerMovedParameters) parameters;
            var data = param.PlaverMovedData;

            switch (data.Mode)
            {
                case PlaverMovedMode.OnGround:
                    return BeginSendPacketHandled(new PlayerPacket { OnGround = data.OnGround }, param.AsyncCallback, param.State);

                case PlaverMovedMode.Vector3:
                    return BeginSendPacketHandled(new PlayerPositionPacket
                    {
                        X = data.Vector3.X,
                        FeetY = data.Vector3.Y,
                        Z = data.Vector3.Z,
                        OnGround = data.OnGround
                    }, param.AsyncCallback, param.State);

                case PlaverMovedMode.YawPitch:
                    return BeginSendPacketHandled(new PlayerLookPacket
                    {
                        Yaw = data.Yaw,
                        Pitch = data.Pitch,
                        OnGround = data.OnGround
                    }, param.AsyncCallback, param.State);

                case PlaverMovedMode.All:
                    return BeginSendPacketHandled(new PlayerPositionAndLookPacket
                    {
                        X = data.Vector3.X,
                        FeetY = data.Vector3.Y,
                        Z = data.Vector3.Z,
                        Yaw = data.Yaw,
                        Pitch = data.Pitch,
                        OnGround = data.OnGround
                    }, param.AsyncCallback, param.State);

                default:
                    throw new Exception("PacketError");
            }
        }

        private IAsyncResult BeginPlayerSetRemoveBlock(IAsyncSendingParameters parameters)
        {
            var param = (BeginPlayerSetRemoveBlockParameters)parameters;
            var data = param.PlayerSetRemoveBlockData;

            switch (data.Mode)
            {
                case PlayerSetRemoveBlockEnum.Place:
                    return BeginSendPacketHandled(new PlayerBlockPlacementPacket
                    {
                        Location = data.Location,
                        Slot = data.Slot,
                        CursorVector3 = data.Crosshair,
                        Direction = (Direction) data.Direction
                    }, param.AsyncCallback, param.State);

                case PlayerSetRemoveBlockEnum.Dig:
                    return BeginSendPacketHandled(new PlayerDiggingPacket
                    {
                        Status = (BlockStatus) data.Status,
                        Location = data.Location,
                        Face = data.Face
                    }, param.AsyncCallback, param.State);

                default:
                    throw new Exception("PacketError");
            }
        }

        private IAsyncResult BeginSendMessage(IAsyncSendingParameters parameters)
        {
            var param = (BeginSendMessageParameters) parameters;

            return BeginSendPacketHandled(new ChatMessagePacket { Message = param.Message }, param.AsyncCallback, param.State);
        }

        private IAsyncResult BeginPlayerHeldItem(IAsyncSendingParameters parameters)
        {
            var param = (BeginPlayerHeldItemParameters) parameters;

            return BeginSendPacketHandled(new HeldItemChangePacket { Slot = param.Slot }, param.AsyncCallback, param.State);
        }
    }
}
