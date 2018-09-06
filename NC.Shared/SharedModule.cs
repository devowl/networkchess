using Autofac;

using NC.Shared.GameField;

namespace NC.Shared
{
    /// <summary>
    /// WebServer assembly module file.
    /// </summary>
    public class SharedModule : Module
    {
        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PieceMasterFactory>().As<IPieceMasterFactory>();
        }
    }
}