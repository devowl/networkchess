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
        public WcfGameInfo(string sideColor, string opponentName, ChessPiece[,] defaultField)
        {
            SideColor = sideColor;
            OpponentName = opponentName;
            DefaultField = defaultField.ToJaggedArray();
        }

        /// <summary>
        /// Side color name.
        /// </summary>
        [DataMember]
        public string SideColor { get; set; }

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