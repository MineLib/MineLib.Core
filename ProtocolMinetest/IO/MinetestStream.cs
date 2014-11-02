using System;
using System.IO;
using System.Text;
using MineLib.Network;
using MineLib.Network.IO;
using Org.BouncyCastle.Math;

namespace ProtocolMinetest.IO
{
    // -- Credits to umby24 for encryption support, as taken from CWrapped.
    // -- Credits to SirCmpwn for encryption support, as taken from SMProxy.
    // -- All Write methods doesn't write to any stream. It writes to _buffer. Purge write _buffer to any stream.
    public sealed class MinetestStream : IProtocolStream
    {
        private delegate IAsyncResult PacketWrite(IPacket packet);
        private PacketWrite _packetWriteDelegate;

        private Stream _stream;
        private byte[] _buffer;
        private Encoding _encoding = Encoding.UTF8;

        public MinetestStream(Stream stream)
        {
            _stream = stream;
        }


        // -- String

        public void WriteString(string value, int length)
        {
            var final = new byte[length];
            var array = _encoding.GetBytes(value);

            if (array.Length > length)
            {
                Buffer.BlockCopy(array, 0, final, 0, length);
                WriteByteArray(final);
            }

            if (array.Length < length)
            {
                for (int i = 0; i < final.Length; i++)
                    final[i] = 0x00;

                Buffer.BlockCopy(array, 0, final, 0, array.Length);
                WriteByteArray(final);
            }

            if (array.Length == length)
                WriteByteArray(array);
        }

        public void WriteString(string value)
        {
            WriteByteArray(_encoding.GetBytes(value));
        }

        // -- VarInt

        public void WriteVarInt(int value)
        {
            WriteByteArray(GetVarIntBytes(value));
        }

        public static byte[] GetVarIntBytes(long value)
        {
            var byteBuffer = new byte[10];
            short pos = 0;

            do
            {
                var byteVal = (byte)(value & 0x7F);
                value >>= 7;

                if (value != 0)
                    byteVal |= 0x80;

                byteBuffer[pos] = byteVal;
                pos += 1;
            } while (value != 0);

            var result = new byte[pos];
            Buffer.BlockCopy(byteBuffer, 0, result, 0, pos);

            return result;
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

        public void WriteUInt(uint value)
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


        public void SendPacket(IPacket packet)
        {
            using (var ms = new MemoryStream())
            using (var stream = new MinetestStream(ms))
            {
                packet.WritePacket(stream);
                var data = ms.ToArray();

                _stream.Write(data, 0, data.Length);
            }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }


        public IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback callback, object state)
        {
            _packetWriteDelegate = packet1 =>
            {
                using (var ms = new MemoryStream())
                using (var stream = new MinetestStream(ms))
                {
                    packet.WritePacket(stream);
                    var data = ms.ToArray();

                    return BeginSend(data, null, null);
                }
            };

            return _packetWriteDelegate.BeginInvoke(packet, callback, state);
        }

        public IAsyncResult BeginSend(byte[] data, AsyncCallback callback, object state)
        {
            return _stream.BeginWrite(data, 0, data.Length, callback, state);
        }

        public void EndSend(IAsyncResult asyncResult)
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
