using Autofac;

using NC.ChessServer.Services;

namespace NC.ChessServer
{
    /// <summary>
    /// ChessServer assembly module.
    /// </summary>
    public class ChessServerModule : Module
    {
        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChessService>();
        }
    }
}