using System.Runtime.Serialization;

using NC.Shared.Data;

namespace NC.Shared.Contracts
{
    /// <summary>
    /// Game information.
    /// </summary>
    [DataContract]
    public class WcfGameInfo
    {
        /// <summary>
        /// Constructor for <see cref="WcfGameInfo"/>.
        /// </summary>
        public WcfGameInfo(
            PlayerColor playerColor,
            string opponentName,
            ChessPiece[,] defaultField,
            PlayerColor turnColor)
        {
            PlayerColor = playerColor;
            OpponentName = opponentName;
            TurnColor = turnColor;
            DefaultField = defaultField.ToJaggedArray();
        }

        /// <summary>
        /// Your side color name.
        /// </summary>
        [DataMember]
        public PlayerColor PlayerColor { get; set; }

        /// <summary>
        /// Opponent player name.
        /// </summary>
        [DataMember]
        public string OpponentName { get; set; }

        /// <summary>
        /// Current turn color.
        /// </summary>
        [DataMember]
        public PlayerColor TurnColor { get; set; }

        /// <summary>
        /// Default game field.
        /// </summary>
        [DataMember]
        public ChessPiece[][] DefaultField { get; set; }
    }
}