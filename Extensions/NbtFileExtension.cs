using fNbt;

using MineLib.Core.IO;

namespace MineLib.Core.Extensions
{
    public static class NbtFileExtension
    {
        public static void SaveToProtocolStream(this NbtFile nbtFile, IProtocolStream stream, NbtCompression gZip)
        {
            var data = nbtFile.SaveToBuffer(gZip);
            stream.WriteShort((short)data.Length);
            stream.WriteByteArray(data);
        }
    }
}