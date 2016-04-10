namespace MineLib.Core.Events.SendingEvents
{
    public class SendMessageEvent : SendingEvent
    {
        public string Message { get; private set; }

        public SendMessageEvent(string message)
        {
            Message = message;
        }
    }
}