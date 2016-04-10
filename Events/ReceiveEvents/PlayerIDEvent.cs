namespace MineLib.Core.Events.ReceiveEvents
{
    public class PlayerIDEvent : ReceiveEvent
    {
        public int PlayerID { get; private set; }

        public PlayerIDEvent(int playerID)
        {
            PlayerID = playerID;
        }
    }
}