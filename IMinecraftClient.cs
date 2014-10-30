using System;

namespace MineLib.Network
{
    public interface IMinecraftClient : IDisposable
    {
        string PlayerName { get; set; }

        string ClientBrand { get; set; }

        string ServerHost { get; set; }
        short ServerPort { get; set; }

        ServerState State { get; set; }

        // -- Modern
        string AccessToken { get; set; }
        string SelectedProfile { get; set; }
        // -- Modern

        NetworkMode Mode { get; set; }
    }
}