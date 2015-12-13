namespace MineLib.Core.Events.SendingEvents
{
    public class KeepAliveEvent : SendingEvent { }

    public class KeepAliveEventArgs : SendingEventArgs
    {
        public int KeepAlive { get; private set; }

        public KeepAliveEventArgs(int value)
        {
            KeepAlive = value;
        }
    }
}