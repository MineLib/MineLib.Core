namespace MineLib.Core.Data.Structs
{
    public enum PlaverMovedMode
    {
        OnGround,
        Vector3,
        YawPitch,
        All
    }

    public interface IPlaverMovedData
    {
        bool OnGround { get; set; }
    }

    public class PlaverMovedDataOnGround : IPlaverMovedData
    {
        public bool OnGround { get; set; }
    }

    public class PlaverMovedDataVector3 : IPlaverMovedData
    {
        public Vector3 Vector3 { get; set; }
        public bool OnGround { get; set; }
    }

    public class PlaverMovedDataYawPitch : IPlaverMovedData
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool OnGround { get; set; }
    }

    public class PlaverMovedDataAll : IPlaverMovedData
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public Vector3 Vector3 { get; set; }
        public bool OnGround { get; set; }
    }
}