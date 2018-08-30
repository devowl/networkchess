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

        public event EventHandler<EventArgs> GameStarted;

        /// <summary>
        /// Constructor for <see cref="ChessServiceCallback"/>.
        /// </summary>
        public ChessServiceCallback(IUserMessage userMessage)
        {
            _userMessage = userMessage;
        }

        /// <inheritdoc/>
        public void Message(string text)
        {
            _userMessage.PushInfo(text);
        }

        /// <inheritdoc/>
        public void OpponentMove(int fromX, int fromY, int toX, int toY, ChessPiece[][] virtualField)
        { 
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void GameHasStarted(WcfGameInfo gameInfo)
        {
            GameStarted?.Invoke(this, EventArgs.Empty);
        }
    }
}