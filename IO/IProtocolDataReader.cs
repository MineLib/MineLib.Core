using System;
using Org.BouncyCastle.Math;

namespace MineLib.Network.IO
{
    /// <summary>
    /// Object that reads data from IPacket.
    /// </summary>
    public interface IProtocolDataReader : IDisposable
    {
        string ReadString(int length = 0);

        int ReadVarInt();

        bool ReadBoolean();

        sbyte ReadSByte();
        byte ReadByte();

        short ReadShort();
        ushort ReadUShort();

        int ReadInt();
        uint ReadUInt();

        long ReadLong();
        ulong ReadULong();

        BigInteger ReadBigInteger();
        BigInteger ReadUBigInteger();

        float ReadFloat();

        double ReadDouble();


        string[] ReadStringArray(int value);

        int[] ReadVarIntArray(int value);

        int[] ReadIntArray(int value);

        byte[] ReadByteArray(int value);
    }
}