using Aragas.Core.Data;

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

    public struct PlaverMovedDataOnGround : IPlaverMovedData
    {
        public bool OnGround { get; set; }
    }
    public struct PlaverMovedDataVector3 : IPlaverMovedData
    {
        public Vector3 Vector3 { get; set; }
        public bool OnGround { get; set; }
    }
    public struct PlaverMovedDataYawPitch : IPlaverMovedData
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public bool OnGround { get; set; }
    }
    public struct PlaverMovedDataAll : IPlaverMovedData
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public Vector3 Vector3 { get; set; }
        public bool OnGround { get; set; }
    }
}