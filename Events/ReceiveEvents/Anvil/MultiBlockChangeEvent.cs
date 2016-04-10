using MineLib.Core.Data;
using MineLib.Core.Data.Structs;

namespace MineLib.Core.Events.ReceiveEvents.Anvil
{
    public class MultiBlockChangeEvent : ReceiveEvent
    {
        public Coordinates2D ChunkLocation { get; private set; }
        public BlockPosition[] Records { get; private set; }

        public MultiBlockChangeEvent(Coordinates2D chunkLocation, BlockPosition[] records)
        {
            ChunkLocation = chunkLocation;
            Records = records;
        }
    }
}