using System;
using System.ServiceModel;

using NC.Shared.Contracts;

namespace NC.ChessServer.GamePack
{
    /// <summary>
    /// Player object.
    /// </summary>
    public class Player : IDisposable
    {
        /// <summary>
        /// Constructor for <see cref="Player"/>.
        /// </summary>
        public Player(string sessionId, string playerName)
        {
            SessionId = sessionId;
            PlayerName = playerName;
            LastActivity = DateTime.Now;
        }

        /// <summary>
        /// Create <see cref="Player"/> instance.
        /// </summary>
        /// <param name="sessionId">Session Id.</param>
        /// <param name="playerName">Player name.</param>
        /// <returns><see cref="Player"/> instance.</returns>
        public delegate Player Factory(string sessionId, string playerName);

        /// <summary>
        /// Is ready to play.
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// Session ID.
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        /// Player name.
        /// </summary>
        public string PlayerName { get; }

        /// <summary>
        /// Player callback.
        /// </summary>
        public IChessServiceCallback Callback { get; private set; }

        /// <summary>
        /// Last player activity time.
        /// </summary>
        public DateTime LastActivity { get; private set; }

        /// <summary>
        /// Player color.
        /// </summary>
        public PlayerColor PlayerColor { get; private set; }

        /// <summary>
        /// Set last activity time.
        /// </summary>
        /// <param name="player"><see cref="Player"/> instance.</param>
        internal static void SetActive(Player player)
        {
            player.LastActivity = DateTime.Now;
        }

        /// <summary>
        /// Set last activity time.
        /// </summary>
        /// <param name="player"><see cref="Player"/> instance.</param>
        /// <param name="playerColor">Player color.</param>
        internal static void SetColor(Player player, PlayerColor playerColor)
        {
            player.PlayerColor = playerColor;
        }

        /// <summary>
        /// Set player ready.
        /// </summary>
        /// <param name="player"><see cref="Player"/> instance.</param>
        /// <param name="isReady"><see cref="IsReady"/> status.</param>
        internal static void SetIsReady(Player player, bool isReady = true)
        {
            player.IsReady = isReady;
        }

        /// <summary>
        /// Set callback function.
        /// </summary>
        /// <param name="player"><see cref="Player"/> instance.</param>
        /// <param name="callback">Callback interface.</param>
        internal static void SetCallback(Player player, IChessServiceCallback callback)
        {
            player.Callback = callback;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (Callback == null)
            {
                return;
            }

            var client = Callback as ICommunicationObject;
            try
            {
                client.Close();
            }
            catch (CommunicationException)
            {
                client.Abort();
            }
            catch (TimeoutException)
            {
                client.Abort();
            }
            catch (Exception)
            {
                client.Abort();
            }

            Callback = null;
        }
    }
}