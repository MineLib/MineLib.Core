namespace MineLib.Core.Events.ReceiveEvents
{
    public class UpdateHealthEvent : ReceiveEvent
    {
        public float Health { get; private set; }

        public UpdateHealthEvent(float health)
        {
            Health = health;
        }
    }
}