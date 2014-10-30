using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MineLib.Network.IO;

namespace MineLib.Network
{
    public interface IPluginBase
    {
        string Name { get; }
        string Version { get; }
    }

    public static class Plugin
    {
        public static T CreatePlugin<T>(string file)
        {
            T plugin = default(T);

            Type pluginType = null;

            if (File.Exists(file))
            {
                Assembly asm = Assembly.LoadFile(file);

                if (asm != null)
                {
                    for (int i = 0; i < asm.GetTypes().Length; i++)
                    {
                        Type type = (Type)asm.GetTypes().GetValue(i);

                        if (IsImplementationOf(type, typeof(IPluginBase)))
                        {
                            plugin = (T)Activator.CreateInstance(type);
                        }
                    }
                }
            }

            return plugin;
        }

        private static bool IsImplementationOf(Type type, Type @interface)
        {
            Type[] interfaces = type.GetInterfaces();

            for (int index = 0; index < interfaces.Length; index++)
            {
                Type current = interfaces[index];
                if (IsSubtypeOf(ref current, @interface)) return true;
            }
            return false;
        }

        private static bool IsSubtypeOf(ref Type a, Type b)
        {
            if (a == b)
            {
                return true;
            }

            if (a.IsGenericType)
            {
                a = a.GetGenericTypeDefinition();

                if (a == b)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public sealed partial class NetworkHandler : INetworkHandler
    {
        // -- Debugging
        public List<IPacket> PacketsReceived { get; set; }
        public List<IPacket> PacketsSended { get; set; }
        // -- Debugging

        #region Properties

        public NetworkMode NetworkMode { get { return _minecraft.Mode; } }

        public bool DebugPackets { get; set; }

        public bool Connected { get { return _protocol.Connected; } }

        public bool Crashed { get { return _protocol.Crashed; } }

        #endregion

        public IPacketSender PacketSender { get { return _protocol.PacketSender; } }

        private readonly IMinecraftClient _minecraft;
        private readonly IProtocol _protocol;

        public NetworkHandler(IMinecraftClient client)
        {
            _minecraft = client;

            PacketsReceived = new List<IPacket>();
            PacketsSended = new List<IPacket>();

            _protocol = Plugin.CreatePlugin<IProtocol>(string.Format(Environment.CurrentDirectory + "\\" + "{0}.dll", NetworkMode));
        }


        /// <summary>
        /// Start NetworkHandler.
        /// </summary>
        public void Connect(bool debugPackets = true)
        {
            DebugPackets = debugPackets;
            
            _protocol.PacketHandled += RaisePacketHandled;
            _protocol.Connect(_minecraft);
        }


        public IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new Exception("Connection error");

            if (DebugPackets)
                PacketsSended.Add(packet);

            IAsyncResult result = BeginSend(packet, asyncCallback, state);
            EndSend(result);
            return result;
        }

        public IAsyncResult BeginSend(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            return _protocol.BeginSend(packet, asyncCallback, state);
        }

        public void EndSend(IAsyncResult asyncResult)
        {
            _protocol.EndSend(asyncResult);
        }


        /// <summary>
        /// Dispose NetworkHandler.
        /// </summary>
        public void Dispose()
        {
            if (_protocol != null)
                _protocol.Dispose();

            if (PacketsReceived != null)
                PacketsReceived.Clear();

            if (PacketsSended != null)
                PacketsSended.Clear();
        }
    }
}