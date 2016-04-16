using MineLib.Core.Data.Structs;

namespace MineLib.Core.Events.SendingEvents
{
    public class PlayerMovedEvent : SendingEvent
    {
        public PlaverMovedMode Mode { get; }
        public IPlaverMovedData Data { get; }


        public PlayerMovedEvent(PlaverMovedMode mode, IPlaverMovedData data) { Mode = mode; Data = data; }
        public PlayerMovedEvent(IPlaverMovedData data)
        {
            if (data is PlaverMovedDataOnGround)
                Mode = PlaverMovedMode.OnGround;
            else if (data is PlaverMovedDataVector3)
                Mode = PlaverMovedMode.Vector3;
            else if (data is PlaverMovedDataYawPitch)
                Mode = PlaverMovedMode.YawPitch;
            else if (data is PlaverMovedDataAll)
                Mode = PlaverMovedMode.All;
            
            Data = data;
        }
    }
}