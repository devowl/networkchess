using System;
using System.ServiceModel;

using NC.Client.Interfaces;
using NC.Shared.Contracts;
using NC.Shared.Data;

namespace NC.Client.Models
{
    /// <summary>
    /// Chess service callback.
    /// </summary>
    [CallbackBehavior]
    public class ChessServiceCallback : IChessServiceCallback
    {
        private readonly IUserMessage _userMessage;

        /// <summary>
        /// Constructor for <see cref="ChessServiceCallback"/>.
        /// </summary>
        public ChessServiceCallback(IUserMessage userMessage)
        {
            _userMessage = userMessage;
        }

        public event EventHandler<EventArgs> GameStarted;

        /// <summary>
        /// Server game info.
        /// </summary>
        public WcfGameInfo GameInfo { get; private set; }

        /// <inheritdoc/>
        public void Message(string text)
        {
            _userMessage.PushInfo(text);
        }

        /// <inheritdoc/>
        public void OpponentMove(int fromX, int fromY, int toX, int toY, ChessPiece[][] virtualField)
        {
        }

        /// <inheritdoc/>
        public void GameHasStarted(WcfGameInfo gameInfo)
        {
            GameInfo = gameInfo;
            GameStarted?.Invoke(this, EventArgs.Empty);
        }
    }
}