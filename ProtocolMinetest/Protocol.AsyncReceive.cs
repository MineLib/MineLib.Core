using MineLib.Network;

namespace ProtocolMinetest
{
    public partial class Protocol
    {
        private void OnChatMessage(string message)
        {
            _minecraft.DoReceiveEvent(typeof(OnChatMessage), new OnChatMessage(message));
        }
    }
}
