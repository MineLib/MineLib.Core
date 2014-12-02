using MineLib.Network;

namespace ProtocolPocketEdition
{
    public partial class Protocol
    {
        private void OnChatMessage(string message)
        {
            _minecraft.DoReceiveEvent(typeof(OnChatMessage), new OnChatMessage(message));
        }
    }
}
