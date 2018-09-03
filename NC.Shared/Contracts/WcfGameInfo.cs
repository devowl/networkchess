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
        public WcfGameInfo(PlayerColor playerColor, string opponentName, ChessPiece[,] defaultField)
        {
            PlayerColor = playerColor;
            OpponentName = opponentName;
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
        /// Default game field.
        /// </summary>
        [DataMember]
        public ChessPiece[][] DefaultField { get; set; }
    }
}