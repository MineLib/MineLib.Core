using MineLib.Core.Data.Structs;

namespace MineLib.Core.Events.SendingEvents
{
    public class PlayerMovedEvent : SendingEvent { }

    public class PlayerMovedEventArgs : SendingEventArgs
    {
        public PlaverMovedMode Mode { get; private set; }
        public IPlaverMovedData Data { get; private set; }

        public PlayerMovedEventArgs(IPlaverMovedData data)
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

        public PlayerMovedEventArgs(PlaverMovedMode mode, IPlaverMovedData data)
        {
            Mode = mode;

            Data = data;
        }
    }
}