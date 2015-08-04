namespace MineLib.Core.Data
{
    public struct VarInt
    {
        private readonly int _value;

        public VarInt(int value)
        {
            _value = value;
        }

        public static implicit operator VarInt(int value)
        {
            return new VarInt(value);
        }

        public static implicit operator int(VarInt value)
        {
            return value._value;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}