namespace MineLib.Core.Data.Structs
{
    public enum PlayerSetRemoveBlockMode
    {
        Remove,
        Place,
        Dig
    }

    public interface IPlayerSetRemoveBlockData { }

    public struct PlayerSetRemoveBlockDataPlace : IPlayerSetRemoveBlockData
    {
        public Position Location { get; set; }
        public ItemStack Slot { get; set; }
        public Position Crosshair { get; set; }
        public byte Direction { get; set; }
    }
    public struct PlayerSetRemoveBlockDataDig : IPlayerSetRemoveBlockData
    {
        public byte Status { get; set; }
        public Position Location { get; set; }
        public sbyte Face { get; set; }
    }
    public struct PlayerSetRemoveBlockDataRemove : IPlayerSetRemoveBlockData
    {
        public Position Location { get; set; }
        public int BlockID { get; set; }
        public byte Face { get; set; }
        public byte Mode { get; set; }
    }
}