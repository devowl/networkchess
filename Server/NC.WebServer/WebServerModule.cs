﻿using Autofac;

using NC.ChessServer;
using NC.Shared;

namespace NC.WebServer
{
    /// <summary>
    /// WebServer assembly module file.
    /// </summary>
    public class WebServerModule : Module
    {
        /// <inheritdoc/>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ChessServerModule>();
            builder.RegisterModule<SharedModule>();
        }
    }
}