using MineLib.Core.Data;

namespace MineLib.Core.Events.ReceiveEvents.Anvil
{
    public class BlockChangeEvent : ReceiveEvent
    {
        public Position Location { get; private set; }
        public int Block { get; private set; }

        public BlockChangeEvent(Position location, int block)
        {
            Location = location;
            Block = block;
        }
    }
}