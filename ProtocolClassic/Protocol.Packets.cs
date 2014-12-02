using System;
using MineLib.Network;
using ProtocolClassic.Enums;
using ProtocolClassic.Packets.Server;

namespace ProtocolClassic
{
    public partial class Protocol
    {
        private void OnPacketHandled(int id, IPacketWithSize packet, ConnectionState? state)
        {
            if (!Connected)
                return;

            // -- Debugging
            Console.WriteLine("Classic ID: 0x" + String.Format("{0:X}", id));
            Console.WriteLine(" ");
            // -- Debugging

            switch ((PacketsServer) id)
            {
                case PacketsServer.ServerIdentification:
                    State = ConnectionState.JoinedServer;
                    break;

                case PacketsServer.Ping:
                    break;

                case PacketsServer.LevelInitialize:
                    break;

                case PacketsServer.LevelDataChunk:
                    var levelDataChunkPacket = (LevelDataChunkPacket) packet;

                    //OnChunk();
                    break;

                case PacketsServer.LevelFinalize:
                    break;

                case PacketsServer.SetBlock:
                    var setBlockPacket = (SetBlockPacket) packet;

                    OnBlockChange(setBlockPacket.Coordinates, setBlockPacket.BlockType);
                    break;

                case PacketsServer.SpawnPlayer:
                    break;

                case PacketsServer.PositionAndOrientationTeleport:
                    break;

                case PacketsServer.PositionAndOrientationUpdate:
                    break;

                case PacketsServer.PositionUpdate:
                    break;

                case PacketsServer.OrientationUpdate:
                    break;

                case PacketsServer.DespawnPlayer:
                    break;

                case PacketsServer.Message:
                    var messagePacket = (MessagePacket) packet;

                    OnChatMessage(messagePacket.Message);
                    break;

                case PacketsServer.DisconnectPlayer:
                    break;

                case PacketsServer.UpdateUserType:
                    break;

                case PacketsServer.ExtInfo:
                    break;

                case PacketsServer.ExtEntry:
                    break;

                case PacketsServer.SetClickDistance:
                    break;

                case PacketsServer.CustomBlockSupportLevel:
                    break;

                case PacketsServer.HoldThis:
                    break;

                case PacketsServer.SetTextHotKey:
                    break;

                case PacketsServer.ExtAddPlayerName:
                    break;

                case PacketsServer.ExtRemovePlayerName:
                    break;

                case PacketsServer.EnvSetColor:
                    break;

                case PacketsServer.MakeSelection:
                    break;

                case PacketsServer.RemoveSelection:
                    break;

                case PacketsServer.SetBlockPermission:
                    break;

                case PacketsServer.ChangeModel:
                    break;

                case PacketsServer.EnvSetMapAppearance:
                    break;

                case PacketsServer.EnvSetWeatherType:
                    break;

                case PacketsServer.HackControl:
                    break;

                case PacketsServer.ExtAddEntity2:
                    break;

                default:
                    throw new ProtocolException("Connection error: Incorrect data.");
            }
        }
    }
}