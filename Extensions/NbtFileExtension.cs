using Aragas.Core.IO;

using fNbt;

namespace MineLib.Core.Extensions
{
    public static class NbtFileExtension
    {
        public static void SaveToProtocolStream(this NbtFile nbtFile, PacketStream stream, NbtCompression gZip)
        {
            var data = nbtFile.SaveToBuffer(gZip);
            // TODO: Check short
            stream.Write((short) data.Length);
            stream.Write(data);
        }
    }
}