namespace MineLib.Core.Events.ReceiveEvents
{
    public class UpdateHealthEvent : ReceiveEvent
    {
        public float Health { get; private set; }
        public int Food { get; set; }
        public float FoodSaturation { get; set; }

        public UpdateHealthEvent(float health, int food, float foodSaturation)
        {
            Health = health;
            Food = food;
            FoodSaturation = foodSaturation;
        }
    }
}