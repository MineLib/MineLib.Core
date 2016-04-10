namespace MineLib.Core.Events.ReceiveEvents
{
    public class GameInfoEvent : ReceiveEvent
    {
        public byte GameMode { get; private set; }
        public byte Difficulty { get; private set; }
        public byte Dimension { get; private set; }

        public GameInfoEvent(byte gameMode, byte difficulty, byte dimension)
        {
            GameMode = gameMode;
            Difficulty = difficulty;
            Dimension = dimension;
        }
    }
}