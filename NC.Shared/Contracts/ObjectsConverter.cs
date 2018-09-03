using NC.Shared.Data;

namespace NC.Shared.Contracts
{
    /// <summary>
    /// Business objects converters.
    /// </summary>
    public static class ObjectsConverter
    {
        /// <summary>
        /// Convert from <see cref="WcfChessPoint"/> to <see cref="ChessPoint"/>.
        /// </summary>
        /// <param name="wcfPoint">Transport object.</param>
        /// <returns>Business object.</returns>
        public static ChessPoint ToBusiness(this WcfChessPoint wcfPoint)
        {
            return new ChessPoint(wcfPoint.X, wcfPoint.Y);
        }

        /// <summary>
        /// Convert from <see cref="ChessPoint"/> to <see cref="WcfChessPoint"/>.
        /// </summary>
        /// <param name="point">Business object.</param>
        /// <returns>Transport object.</returns> 
        public static WcfChessPoint FromBusiness(this ChessPoint point)
        {
            return new WcfChessPoint(point.X, point.Y);
        }
    }
}