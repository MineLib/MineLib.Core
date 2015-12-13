namespace MineLib.Core.Events.SendingEvents
{
    public class SendMessageEvent : SendingEvent { }

    public class SendMessageEventArgs : SendingEventArgs
    {
        public string Message { get; private set; }

        public SendMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}