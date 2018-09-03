using System.Runtime.Serialization;

using NC.Shared.Data;

namespace NC.Shared.Contracts
{
    /// <summary>
    /// Transport <see cref="ChessPoint"/> object.
    /// </summary>
    [DataContract]
    public class WcfChessPoint
    {
        /// <summary>
        /// Constructor for <see cref="WcfChessPoint"/>.
        /// </summary>
        public WcfChessPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// X coordinate.
        /// </summary>
        [DataMember]
        public int X { get; set; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        [DataMember]
        public int Y { get; set; }
    }
}