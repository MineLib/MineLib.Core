using System;

using Aragas.Core.Packets;

namespace MineLib.Core.Events
{
    public abstract class SendingEvent { }

    public abstract class SendingEventArgs
    {
        public Action<ProtobufPacket> SendPacket { get; private set; }

        public void RegisterSending(Action<ProtobufPacket> sendPacket)
        {
            SendPacket += sendPacket;
        }
    }
}
