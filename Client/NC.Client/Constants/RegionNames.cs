using NC.Client.Views;

namespace NC.Client.Constants
{
    /// <summary>
    /// Prism region names.
    /// </summary>
    public static class RegionNames
    {
        /// <summary>
        /// Connection dialog.
        /// </summary>
        public const string Connection = nameof(ConnectionView);

        /// <summary>
        /// Game has started.
        /// </summary>
        public const string Game = nameof(GameView);

        /// <summary>
        /// Main region.
        /// </summary>
        public const string MainRegion = "MainRegion";

        /// <summary>
        /// User messages region.
        /// </summary>
        public const string UserMessages = nameof(UserMessagesView);
    }
}