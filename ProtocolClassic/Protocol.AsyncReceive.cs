using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Anvil;

namespace ProtocolClassic
{
    public partial class Protocol
    {
        private void OnChatMessage(string message)
        {
            _minecraft.DoReceiveEvent(typeof(OnChatMessage), new OnChatMessage(message));
        }

        #region Anvil

        private void OnChunkList(ChunkList chunks)
        {
            _minecraft.DoReceiveEvent(typeof(OnChunkList), new OnChunkList(chunks));
        }

        private void OnBlockChange(Position location, int block)
        {
            _minecraft.DoReceiveEvent(typeof(OnBlockChange), new OnBlockChange(location, block)); // TODO: Implement chunk coords2D
        }

        #endregion

        private void OnPlayerPosition(Vector3 position)
        {
            _minecraft.DoReceiveEvent(typeof(OnPlayerPosition), new OnPlayerPosition(position));
        }

        private void OnPlayerLook(Vector3 look)
        {
            _minecraft.DoReceiveEvent(typeof(OnPlayerLook), new OnPlayerLook(look));
        }


        private void OnSpawnPoint(Position location)
        {
            _minecraft.DoReceiveEvent(typeof(OnSpawnPoint), new OnSpawnPoint(location));
        }


        private void OnRespawn(object gameInfo)
        {
            _minecraft.DoReceiveEvent(typeof(OnRespawn), new OnRespawn(gameInfo));
        }

    }
}
