using System;

namespace MineLib.Core.Events.ReceiveEvents
{
    public class TimeUpdateEvent : ReceiveEvent
    {
        public TimeSpan TimeOfDay { get; private set; }

        public TimeUpdateEvent(TimeSpan timeOfDay)
        {
            TimeOfDay = timeOfDay;
        }
    }
}