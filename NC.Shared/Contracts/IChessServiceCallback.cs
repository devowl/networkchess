using System.ServiceModel;

using NC.Shared.Data;

namespace NC.Shared.Contracts
{
    /// <summary>
    /// Chess service callback service.
    /// </summary>
    public interface IChessServiceCallback
    {
        /// <summary>
        /// Service message.
        /// </summary>
        /// <param name="text">Message text.</param>
        [OperationContract(IsOneWay = true)]
        void Message(string text);

        /// <summary>
        /// Game field has updated.
        /// </summary>
        /// <param name="virtualField">Virtual field.</param>
        /// <param name="turnColor">Color turn.</param>
        /// <param name="from">Point from.</param>
        /// <param name="to">Point to.</param>
        /// <param name="playerColor">Player color.</param>
        /// <param name="checkedPlayer">Checked player color.</param>
        [OperationContract(IsOneWay = true)]
        void GameFieldUpdated(
            ChessPiece[][] virtualField,
            PlayerColor turnColor,
            WcfChessPoint from,
            WcfChessPoint to,
            PlayerColor playerColor,
            PlayerColor? checkedPlayer);

        /// <summary>
        /// Game has started event.
        /// </summary>
        /// <param name="gameInfo">Game information.</param>
        [OperationContract(IsOneWay = true)]
        void GameHasStarted(WcfGameInfo gameInfo);

        /// <summary>
        /// Game has ended event.
        /// </summary>
        /// <param name="gameInfo">Game information.</param>
        /// <param name="from">Point from.</param>
        /// <param name="to">Point to.</param>
        [OperationContract(IsOneWay = true)]
        void GameHasEnded(WcfGameInfo gameInfo, WcfChessPoint from, WcfChessPoint to);

        /// <summary>
        /// Keep channel alive.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Alive();
    }
}