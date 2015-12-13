namespace MineLib.Core.Events.ReceiveEvents
{
    public class HeldItemChangeEvent : ReceiveEvent
    {
        public byte Slot { get; private set; }

        public HeldItemChangeEvent(byte slot)
        {
            Slot = slot;
        }
    }
}