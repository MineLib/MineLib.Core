using System;
using MineLib.Network.Data;
using Org.BouncyCastle.Math;

namespace MineLib.Network.IO
{
    /// <summary>
    /// Object that reads VarInt (or Byte) and ByteArray for handling Data later 
    /// and writes any data from packet to user-defined object, that will interact with Minecraft Server.
    /// </summary>
    public interface IProtocolStream : IDisposable
    {
        void WriteString(String value, Int32 length = 0);

        void WriteVarInt(VarInt value);

        void WriteBoolean(Boolean value);

        void WriteSByte(SByte value);
        void WriteByte(Byte value);

        void WriteShort(Int16 value);
        void WriteUShort(UInt16 value);

        void WriteInt(Int32 value);
        void WriteUInt(UInt32 value);

        void WriteLong(Int64 value);
        void WriteULong(UInt64 value);

        void WriteBigInteger(BigInteger value);
        void WriteUBigInteger(BigInteger value);

        void WriteDouble(Double value);

        void WriteFloat(Single value);


        void WriteStringArray(String[] value);

        void WriteVarIntArray(Int32[] value);

        void WriteIntArray(Int32[] value);

        void WriteByteArray(Byte[] value);


        Byte ReadByte();

        VarInt ReadVarInt();

        Byte[] ReadByteArray(Int32 value);


        IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback callback, Object state);
        IAsyncResult BeginSend(Byte[] data, AsyncCallback callback, Object state);
        void EndSend(IAsyncResult asyncResult);

        IAsyncResult BeginRead(Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state);
        Int32 EndRead(IAsyncResult asyncResult);

        void SendPacket(IPacket packet);
        Int32 Read(Byte[] buffer, Int32 offset, Int32 count);

        void Purge();
    }
}