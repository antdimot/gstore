﻿using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace GStore.API
{
#pragma warning disable CS1591
    public class Program
    {
        public static int Main( string[] args )
        {
            try
            {
                var hostBuilder = WebHost.CreateDefaultBuilder( args );

                var host = hostBuilder.UseStartup<Startup>()
                                      .UseSerilog( ( hostingContext, loggerConfiguration ) =>
                                                        loggerConfiguration.ReadFrom.Configuration( hostingContext.Configuration ) ).Build();

                host.Run();

                return 0;
            }
            catch( Exception ex )
            {
                Log.Fatal( ex, "Host terminated unexpectedly" );

                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }     
    }
#pragma warning restore CS1591
}
