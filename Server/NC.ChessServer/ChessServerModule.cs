using Autofac;

using NC.ChessServer.GamePack;
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
            builder.RegisterType<Administrator>().AsImplementedInterfaces().AutoActivate();
            builder.RegisterType<Game>();
            builder.RegisterType<ChessService>();
            builder.RegisterType<UserService>();
        }
    }
}