namespace MineLib.Core.Events.ReceiveEvents
{
    public class RespawnEvent : ReceiveEvent
    {
        public object GameInfo { get; private set; }

        public RespawnEvent(object gameInfo)
        {
            GameInfo = gameInfo;
        }
    }
}