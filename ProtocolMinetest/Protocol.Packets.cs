using System;
using MineLib.Network;
using ProtocolMinetest.Packets;

namespace ProtocolMinetest
{
    public partial class Protocol
    {
        private void RaisePacketHandled(int id, IPacket packet, ConnectionState? state)
        {
            if(!Connected)
                return;

            // -- Debugging
            Console.WriteLine("Minetest ID: 0x" + String.Format("{0:X}", id));
            Console.WriteLine(" ");

            switch (state)
            {
                case ConnectionState.JoiningServer:

                    #region JoiningServer

                    switch ((PacketsServer)id)
                    {
                        case PacketsServer.TOCLIENT_INIT:
                            break;

                        case PacketsServer.TOCLIENT_ACCESS_DENIED:
                            break;
                    }
                    break;

                    #endregion

                case ConnectionState.JoinedServer:

                    #region JoinedServer

                    switch ((PacketsServer)id)
                    {
                        case PacketsServer.TOCLIENT_BLOCKDATA:
                            break;

                        case PacketsServer.TOCLIENT_ADDNODE:
                            break;

                        case PacketsServer.TOCLIENT_REMOVENODE:
                            break;

                        case PacketsServer.TOCLIENT_INVENTORY:
                            break;

                        case PacketsServer.TOCLIENT_TIME_OF_DAY:
                            break;

                        case PacketsServer.TOCLIENT_CHAT_MESSAGE:
                            break;

                        case PacketsServer.TOCLIENT_ACTIVE_OBJECT_REMOVE_ADD:
                            break;

                        case PacketsServer.TOCLIENT_ACTIVE_OBJECT_MESSAGES:
                            break;

                        case PacketsServer.TOCLIENT_HP:
                            break;

                        case PacketsServer.TOCLIENT_MOVE_PLAYER:
                            break;

                        //case PacketsServer.TOCLIENT_ACCESS_DENIED:
                        //    break;

                        case PacketsServer.TOCLIENT_DEATHSCREEN:
                            break;

                        case PacketsServer.TOCLIENT_MEDIA:
                            break;

                        case PacketsServer.TOCLIENT_TOOLDEF:
                            break;

                        case PacketsServer.TOCLIENT_NODEDEF:
                            break;

                        case PacketsServer.TOCLIENT_CRAFTITEMDEF:
                            break;

                        case PacketsServer.TOCLIENT_ANNOUNCE_MEDIA:
                            break;

                        case PacketsServer.TOCLIENT_ITEMDEF:
                            break;

                        case PacketsServer.TOCLIENT_PLAY_SOUND:
                            break;

                        case PacketsServer.TOCLIENT_STOP_SOUND:
                            break;

                        case PacketsServer.TOCLIENT_PRIVILEGES:
                            break;

                        case PacketsServer.TOCLIENT_INVENTORY_FORMSPEC:
                            break;

                        case PacketsServer.TOCLIENT_DETACHED_INVENTORY:
                            break;

                        case PacketsServer.TOCLIENT_SHOW_FORMSPEC:
                            break;

                        case PacketsServer.TOCLIENT_MOVEMENT:
                            break;

                        case PacketsServer.TOCLIENT_SPAWN_PARTICLE:
                            break;

                        case PacketsServer.TOCLIENT_ADD_PARTICLESPAWNER:
                            break;

                        case PacketsServer.TOCLIENT_DELETE_PARTICLESPAWNER:
                            break;

                        case PacketsServer.TOCLIENT_HUDADD:
                            break;

                        case PacketsServer.TOCLIENT_HUDRM:
                            break;

                        case PacketsServer.TOCLIENT_HUDCHANGE:
                            break;

                        case PacketsServer.TOCLIENT_HUD_SET_FLAGS:
                            break;

                        case PacketsServer.TOCLIENT_HUD_SET_PARAM:
                            break;

                        case PacketsServer.TOCLIENT_BREATH:
                            break;

                        case PacketsServer.TOCLIENT_SET_SKY:
                            break;

                        case PacketsServer.TOCLIENT_OVERRIDE_DAY_NIGHT_RATIO:
                            break;

                        case PacketsServer.TOCLIENT_LOCAL_PLAYER_ANIMATIONS:
                            break;

                        case PacketsServer.TOCLIENT_EYE_OFFSET:
                            break;
                    }

                    #endregion

                    break;

                case ConnectionState.InfoRequest:

                    #region InfoRequest

                    switch ((PacketsServer) id)
                    {
                    }

                    #endregion

                    break;

                default:
                    throw new ProtocolException("Connection error: Incorrect data.");
            }
        }
    }
}