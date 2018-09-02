using NC.Client.Models;
using NC.Shared.Contracts;

namespace NC.Client.Interfaces
{
    /// <summary>
    /// <see cref="IChessService"/> service provider.
    /// </summary>
    // TODO 4.x Prism hasn't possibility to works with RegionNavigation parameters as objects. 
    public interface IGameServiceProvider
    {
        /// <summary>
        /// <see cref="IChessService"/> client.
        /// </summary>
        IWcfClient<IChessService> ChessClient { get; }

        /// <summary>
        /// <see cref="IChessService"/> callback.
        /// </summary>
        ChessServiceCallback ServiceCallback { get; }
    }
}