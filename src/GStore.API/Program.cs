using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace GStore.API
{
    public class Program
    {
        public static int Main( string[] args )
        {
            try
            {
                var hostBuilder = WebHost.CreateDefaultBuilder(args);

                IWebHost host = hostBuilder.UseStartup<Startup>()
                                           .Build();
                                      //.UseSerilog( ( hostingContext, loggerConfiguration ) =>
                                      //                  loggerConfiguration.ReadFrom.Configuration( hostingContext.Configuration ) ).Build();

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
}
