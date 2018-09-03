using System;
using System.ServiceModel;

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
            WcfChessPoint from,
            WcfChessPoint to,
            PlayerColor playerColor)
        {
            FieldUpdated?.Invoke(
                this,
                new FieldInfoArgs(virtualField, turnColor, from.ToBusiness(), to.ToBusiness(), playerColor));
        }

        /// <inheritdoc/>
        public void GameHasStarted(WcfGameInfo gameInfo)
        {
            GameInfo = gameInfo;
            GameStarted?.Invoke(this, EventArgs.Empty);
        }
    }
}