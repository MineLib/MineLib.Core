using MineLib.Core.Data;

namespace MineLib.Core.Events.ReceiveEvents.Anvil
{
    public class BlockActionEvent : ReceiveEvent
    {
        public Position Location { get; private set; }
        public int Block { get; private set; }
        public object BlockAction { get; private set; }

        public BlockActionEvent(Position location, int block, object blockAction)
        {
            Location = location;
            Block = block;
            BlockAction = blockAction;
        }
    }
}