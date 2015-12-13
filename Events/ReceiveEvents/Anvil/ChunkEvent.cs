using MineLib.Core.Data.Anvil;

namespace MineLib.Core.Events.ReceiveEvents.Anvil
{
    public class ChunkEvent : ReceiveEvent
    {
        public Chunk Chunk { get; private set; }

        public ChunkEvent(Chunk chunk)
        {
            Chunk = chunk;
        }
    }
}