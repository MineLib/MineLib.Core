namespace MineLib.Core.Client
{
    /// <summary>
    /// Client connection state.
    /// </summary>
    public enum ClientState
    {
        /// <summary>
        /// Not initialized.
        /// </summary>
        None,
        /// <summary>
        /// Joining.
        /// </summary>
        Joining,

        /// <summary>
        /// Joined.
        /// </summary>
        Joined,

        /// <summary>
        /// Getting an info request.
        /// </summary>
        InfoRequest
    }
}