namespace MineLib.Core.Events.ReceiveEvents
{
    public class TimeUpdateEvent : ReceiveEvent
    {
        public long WorldAge { get; private set; }
        public long TimeOfDay { get; private set; }

        public TimeUpdateEvent(long worldAge, long timeOfDay)
        {
            WorldAge = worldAge;
            TimeOfDay = timeOfDay;
        }
    }
}