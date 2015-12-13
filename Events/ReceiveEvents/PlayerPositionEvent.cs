using Aragas.Core.Data;

namespace MineLib.Core.Events.ReceiveEvents
{
    public class PlayerPositionEvent : ReceiveEvent
    {
        public Vector3 Position { get; private set; }

        public PlayerPositionEvent(Vector3 position)
        {
            Position = position;
        }
    }
}