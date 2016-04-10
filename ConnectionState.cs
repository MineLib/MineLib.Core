namespace MineLib.Core
{
    /// <summary>
    /// Connection state.
    /// </summary>
    public enum ConnectionState
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