namespace MineLib.Core.Events.ReceiveEvents
{
    public class ChatMessageEvent : ReceiveEvent
    {
        public string Message { get; private set; }

        public ChatMessageEvent(string message)
        {
            Message = message;
        }
    }
}