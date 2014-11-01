using System;
using System.Runtime.InteropServices;
using MineLib.Network.Data;

namespace MineLib.Network
{
    public sealed partial class NetworkHandler
    {
        private const string CustomLibraryName = "CustomModule.dll";

        private IMinecraftClient _minecraft;

        [DllImport(CustomLibraryName, EntryPoint = "Connect")]
        private static extern Boolean ModuleConnect(string ip, short port);

        [DllImport(CustomLibraryName, EntryPoint = "Disconnect")]
        private static extern Boolean ModuleDisconnect();

        [DllImport(CustomLibraryName, EntryPoint = "Dispose")]
        private static extern Boolean ModuleDispose();


        [DllImport(CustomLibraryName, EntryPoint = "ConnectToServer")]
        private static extern Boolean ModuleConnectToServer();

        [DllImport(CustomLibraryName, EntryPoint = "KeepAlive")]
        private static extern Boolean ModuleKeepAlive(Int32 value);

        [DllImport(CustomLibraryName, EntryPoint = "SendClientInfo")]
        private static extern Boolean ModuleSendClientInfo();

        [DllImport(CustomLibraryName, EntryPoint = "Respawn")]
        private static extern Boolean ModuleRespawn();

        [DllImport(CustomLibraryName, EntryPoint = "PlayerMoved")]
        private static extern Boolean ModulePlayerMoved([MarshalAs(UnmanagedType.LPStruct)] PlaverMovedData data);

        [DllImport(CustomLibraryName, EntryPoint = "PlayerSetRemoveBlock")]
        private static extern Boolean ModulePlayerSetRemoveBlock([MarshalAs(UnmanagedType.LPStruct)] PlayerSetRemoveBlockData data);

        [DllImport(CustomLibraryName, EntryPoint = "SendMessage")]
        private static extern Boolean ModuleSendMessage(String message);

        [DllImport(CustomLibraryName, EntryPoint = "PlayerHeldItem")]
        private static extern Boolean ModulePlayerHeldItem(Int16 slot);

    }
}
