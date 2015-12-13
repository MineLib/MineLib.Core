using Aragas.Core.Data;

namespace MineLib.Core.Events.ReceiveEvents
{
    public class PlayerLookEvent : ReceiveEvent
    {
        public Vector3 Look { get; private set; }

        public PlayerLookEvent(Vector3 look)
        {
            Look = look;
        }
    }
}