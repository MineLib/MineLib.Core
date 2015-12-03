using Aragas.Core.Interfaces;

using fNbt;

namespace MineLib.Core.Extensions
{
    public static class NbtFileExtension
    {
        public static void SaveToProtocolStream(this NbtFile nbtFile, IPacketStream stream, NbtCompression gZip)
        {
            var data = nbtFile.SaveToBuffer(gZip);
            // TODO: Check short
            stream.Write((short) data.Length);
            stream.Write(data);
        }
    }
}