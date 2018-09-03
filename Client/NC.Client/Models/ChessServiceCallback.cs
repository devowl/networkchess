using System;
using System.ServiceModel;
using System.Windows;

using NC.Client.Constants;
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
        /// Game field updated event.
        /// </summary>
        public event EventHandler<FieldInfoArgs> FieldUpdated;

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
        public void GameFieldUpdated(
            ChessPiece[][] virtualField,
            PlayerColor turnColor,
            int fromX,
            int fromY,
            int toX,
            int toY)
        {
            FieldUpdated?.Invoke(
                this,
                new FieldInfoArgs(virtualField, turnColor, new Point(fromX, fromY), new Point(toX, toY)));
        }

        /// <inheritdoc/>
        public void GameHasStarted(WcfGameInfo gameInfo)
        {
            GameInfo = gameInfo;
            GameStarted?.Invoke(this, EventArgs.Empty);
        }
    }
}