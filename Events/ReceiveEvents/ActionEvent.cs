namespace MineLib.Core.Events.ReceiveEvents
{
    public class ActionEvent : ReceiveEvent
    {
        public int EntityID { get; private set; }
        public int Action { get; private set; }

        public ActionEvent(int entityID, int action)
        {
            EntityID = entityID;
            Action = action;
        }
    }
}