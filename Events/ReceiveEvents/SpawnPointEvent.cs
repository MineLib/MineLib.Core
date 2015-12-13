using MineLib.Core.Data;

namespace MineLib.Core.Events.ReceiveEvents
{
    public class SpawnPointEvent : ReceiveEvent
    {
        public Position Location { get; private set; }

        public SpawnPointEvent(Position location)
        {
            Location = location;
        }
    }
}