using MineLib.Core.Data.Structs;

namespace MineLib.Core.Events.SendingEvents
{
    public class PlayerMovedEvent : SendingEvent
    {
        public PlaverMovedMode Mode { get; private set; }
        public IPlaverMovedData Data { get; private set; }

        public PlayerMovedEvent(IPlaverMovedData data)
        {
            {
                var type = data as PlaverMovedDataOnGround;
                if (type != null)
                    Mode = PlaverMovedMode.OnGround;
            }

            {
                var type = data as PlaverMovedDataVector3;
                if (type != null)
                    Mode = PlaverMovedMode.Vector3;
            }

            {
                var type = data as PlaverMovedDataYawPitch;
                if (type != null)
                    Mode = PlaverMovedMode.YawPitch;
            }

            {
                var type = data as PlaverMovedDataAll;
                if (type != null)
                    Mode = PlaverMovedMode.All;
            }

            Data = data;
        }

        public PlayerMovedEvent(PlaverMovedMode mode, IPlaverMovedData data)
        {
            Mode = mode;

            Data = data;
        }
    }
}