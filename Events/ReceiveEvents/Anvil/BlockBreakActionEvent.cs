using MineLib.Core.Data;

namespace MineLib.Core.Events.ReceiveEvents.Anvil
{
    public class BlockBreakActionEvent : ReceiveEvent
    {
        public int EntityID { get; private set; }
        public Position Location { get; private set; }
        public byte Stage { get; private set; }

        public BlockBreakActionEvent(int entityID, Position location, byte stage)
        {
            EntityID = entityID;
            Location = location;
            Stage = stage;
        }
    }
}