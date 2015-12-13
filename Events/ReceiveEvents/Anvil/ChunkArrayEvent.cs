using MineLib.Core.Data.Anvil;

namespace MineLib.Core.Events.ReceiveEvents.Anvil
{
    public class ChunkArrayEvent : ReceiveEvent
    {
        public Chunk[] Chunks { get; private set; }

        public ChunkArrayEvent(Chunk[] chunks)
        {
            Chunks = chunks;
        }
    }
}