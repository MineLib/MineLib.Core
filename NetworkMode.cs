namespace MineLib.Network
{
    /// <summary>
    /// Which protocol type we will use
    /// </summary>
    public enum NetworkMode
    {
        /// <summary>
        /// Modern Minecraft protocol.
        /// </summary>
        ProtocolModern,

        /// <summary>
        /// Classic Minecraft protocol.
        /// </summary>
        ProtocolClassic,

        /// <summary>
        /// Pocket Edition Minecraft protocol.
        /// </summary>
        ProtocolPocketEdition,

        /// <summary>
        /// Custom C# protocol.
        /// </summary>
        CustomModule,

        /// <summary>
        /// Custom C++ CLI protocol.
        /// </summary>
        CustomModuleCLI
    }
}