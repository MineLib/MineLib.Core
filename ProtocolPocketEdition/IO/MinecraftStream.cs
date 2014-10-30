using System;
using System.IO;
using System.Text;
using MineLib.Network;
using MineLib.Network.IO;
using Org.BouncyCastle.Math;

namespace ProtocolPocketEdition.IO
{
    // -- Credits to umby24 for encryption support, as taken from CWrapped.
    // -- Credits to SirCmpwn for encryption support, as taken from SMProxy.
    // -- All Write methods doesn't write to any stream. It writes to _buffer. Purge write _buffer to any stream.
    public sealed class MinecraftStream : IMinecraftStream
    {
        private delegate IAsyncResult PacketWrite(IPacket packet);
        private PacketWrite _packetWriteDelegate;

        private Stream _stream;
        private byte[] _buffer;
        private readonly Encoding _encoding = Encoding.ASCII;

        public MinecraftStream(Stream stream)
        {
            _stream = stream;
        }

        // -- String

        public void WriteString(string value)
        {
            var final = new byte[8];
            for (var i = 0; i < final.Length; i++)
                final[i] = 0x20;      

            Buffer.BlockCopy(_encoding.GetBytes(value), 0, final, 0, value.Length);

            WriteByteArray(final);
        }

        // -- VarInt

        public void WriteVarInt(int value)
        {
            throw new NotImplementedException();
        }

        // -- Boolean

        public void WriteBoolean(bool value)
        {
            WriteByte(Convert.ToByte(value));
        }

        // -- SByte & Byte

        public void WriteSByte(sbyte value)
        {
            WriteByte(unchecked((byte)value));
        }

        public void WriteByte(byte value)
        {
            if (_buffer != null)
            {
                var tempBuff = new byte[_buffer.Length + 1];

                Buffer.BlockCopy(_buffer, 0, tempBuff, 0, _buffer.Length);
                tempBuff[_buffer.Length] = value;

                _buffer = tempBuff;
            }
            else
                _buffer = new byte[] { value };
        }

        // -- Short & UShort

        public void WriteShort(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        public void WriteUShort(ushort value)
        {
            WriteByteArray(new byte[]
            {
                (byte) ((value & 0xFF00) >> 8),
                (byte) (value & 0xFF)
            });
        }

        // -- Int & UInt

        public void WriteInt(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        public void WriteUInt(uint value) // Implement
        {
            WriteByteArray(new[]
            {
                (byte)((value & 0xFF000000) >> 24),
                (byte)((value & 0xFF0000) >> 16),
                (byte)((value & 0xFF00) >> 8),
                (byte)(value & 0xFF)
            });
        }

        // -- Long & ULong

        public void WriteLong(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        public void WriteULong(ulong value)
        {
            WriteByteArray(new[]
            {
                (byte)((value & 0xFF00000000000000) >> 56),
                (byte)((value & 0xFF000000000000) >> 48),
                (byte)((value & 0xFF0000000000) >> 40),
                (byte)((value & 0xFF00000000) >> 32),
                (byte)((value & 0xFF000000) >> 24),
                (byte)((value & 0xFF0000) >> 16),
                (byte)((value & 0xFF00) >> 8),
                (byte)(value & 0xFF)
            });
        }

        // -- BigInt & UBigInt

        public void WriteBigInteger(BigInteger value)
        {
            var bytes = value.ToByteArray();
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        public void WriteUBigInteger(BigInteger value)
        {
            throw new NotImplementedException();
        }

        // -- Float

        public void WriteFloat(float value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }

        // -- Double

        public void WriteDouble(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);

            WriteByteArray(bytes);
        }


        // -- StringArray

        public void WriteStringArray(string[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
                WriteString(value[i]);
        }

        // -- VarIntArray

        public void WriteVarIntArray(int[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
                WriteVarInt(value[i]);
        }

        // -- IntArray

        public void WriteIntArray(int[] value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
                WriteInt(value[i]);            
        }

        // -- ByteArray

        public void WriteByteArray(byte[] value)
        {
            if (_buffer != null)
            {
                var tempLength = _buffer.Length + value.Length;
                var tempBuff = new byte[tempLength];

                Buffer.BlockCopy(_buffer, 0, tempBuff, 0, _buffer.Length);
                Buffer.BlockCopy(value, 0, tempBuff, _buffer.Length, value.Length);

                _buffer = tempBuff;
            }
            else
                _buffer = value;
        }


        // -- Read methods

        public byte ReadByte()
        {
            return (byte) _stream.ReadByte();
        }

        public int ReadVarInt()
        {
            var result = 0;
            var length = 0;

            while (true)
            {
                var current = ReadByte();
                result |= (current & 0x7F) << length++ * 7;

                if (length > 6)
                    throw new InvalidDataException("Invalid varint: Too long.");

                if ((current & 0x80) != 0x80)
                    break;
            }

            return result;
        }

        public byte[] ReadByteArray(int value)
        {
            var result = new byte[value];
            if (value == 0) return result;
            int n = value;
            while (true)
            {
                n -= _stream.Read(result, value - n, n);
                if (n == 0)
                    break;
            }
            return result;
        }

        
        #region BeginWrite and BeginRead

        public IAsyncResult BeginWritePacket(IPacket packet, AsyncCallback callback, object state)
        {
            _packetWriteDelegate = WriteFunction;

            return _packetWriteDelegate.BeginInvoke(packet, callback, state);
        }

        #region BeginWrite

        private IAsyncResult WriteFunction(IPacket packet)
        {
            using (var ms = new MemoryStream())
            using (var stream = new MinecraftStream(ms))
            {
                packet.WritePacket(stream);
                var data = ms.ToArray();

                return BeginWritePocketEdition(data, null, null);
            }
        }

        private IAsyncResult BeginWritePocketEdition(byte[] data, AsyncCallback callback, object state)
        {
            return _stream.BeginWrite(data, 0, data.Length, callback, state);
        }

        #endregion

        public void EndWrite(IAsyncResult asyncResult)
        {

            try { _packetWriteDelegate.EndInvoke(asyncResult); }
            catch (Exception) { }
        }


        public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _stream.BeginRead(buffer, offset, count, callback, state);
        }

        public int EndRead(IAsyncResult asyncResult)
        {
            return _stream.EndRead(asyncResult);
        }

        #endregion


        public void Purge()
        {
            _stream.Write(_buffer, 0, _buffer.Length);

            _buffer = null;
        }


        public void Dispose()
        {
            if (_stream != null)
                _stream.Dispose();

            _buffer = null;
        }
    }
}
