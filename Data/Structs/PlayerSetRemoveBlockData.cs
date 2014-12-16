namespace MineLib.Network.Data.Structs
{
    public enum PlayerSetRemoveBlockMode
    {
        Remove,
        Place,
        Dig
    }

    public interface IPlayerSetRemoveBlockData { }

    public class PlayerSetRemoveBlockDataPlace : IPlayerSetRemoveBlockData
    {
        public Position Location { get; set; }
        public ItemStack Slot { get; set; }
        public Position Crosshair { get; set; }
        public byte Direction { get; set; }
    }

    public class PlayerSetRemoveBlockDataDig : IPlayerSetRemoveBlockData
    {
        public byte Status { get; set; }
        public Position Location { get; set; }
        public byte Face { get; set; }
    }

    public class PlayerSetRemoveBlockDataRemove : IPlayerSetRemoveBlockData
    {
        public Position Location { get; set; }
        public int BlockID { get; set; }
        public byte Face { get; set; }
        public byte Mode { get; set; }
    }
}