namespace MineLib.Core.Events.ReceiveEvents
{
    public class UpdateFoodEvent : ReceiveEvent
    {
        public int Food { get; set; }
        public float FoodSaturation { get; set; }

        public UpdateFoodEvent(int food, float foodSaturation)
        {
            Food = food;
            FoodSaturation = foodSaturation;
        }
    }
}