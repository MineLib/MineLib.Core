namespace MineLib.Core.Events.ReceiveEvents
{
    public class SetExperienceEvent : ReceiveEvent
    {
        public float ExperienceBar { get; private set; }
        public int Level { get; private set; }
        public int TotalExperience { get; private set; }

        public SetExperienceEvent(float experienceBar, int level, int totalExperience)
        {
            ExperienceBar = experienceBar;
            Level = level;
            TotalExperience = totalExperience;
        }
    }
}