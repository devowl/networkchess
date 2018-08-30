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
        /// User movement. 
        /// </summary>
        /// <param name="fromX">From coordinate.</param>
        /// <param name="fromY">From coordinate.</param>
        /// <param name="toX">To coordinate.</param>
        /// <param name="toY">To coordinate.</param>
        /// <param name="virtualField">Game virtual field.</param>
        [OperationContract(IsOneWay = true)] 
        void OpponentMove(int fromX, int fromY, int toX, int toY, ChessPiece[][] virtualField);

        /// <summary>
        /// Game has started event.
        /// </summary>
        /// <param name="gameInfo">Game information.</param>
        [OperationContract(IsOneWay = true)]
        void GameHasStarted(WcfGameInfo gameInfo);
    }
}