namespace MineLib.Core.Events.SendingEvents
{
    public class KeepAliveEvent : SendingEvent
    {
        public int KeepAlive { get; private set; }

        public KeepAliveEvent(int value)
        {
            KeepAlive = value;
        }
    }
}