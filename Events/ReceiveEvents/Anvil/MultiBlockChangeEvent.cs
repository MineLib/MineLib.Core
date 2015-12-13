using MineLib.Core.Data;
using MineLib.Core.Data.Structs;

namespace MineLib.Core.Events.ReceiveEvents.Anvil
{
    public class MultiBlockChangeEvent : ReceiveEvent
    {
        public Coordinates2D ChunkLocation { get; private set; }
        public Record[] Records { get; private set; }

        public MultiBlockChangeEvent(Coordinates2D chunkLocation, Record[] records)
        {
            ChunkLocation = chunkLocation;
            Records = records;
        }
    }
}