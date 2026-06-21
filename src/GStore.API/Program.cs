using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace GStore.API
{
    public class Program
    {
        public static int Main( string[] args )
        {
            try
            {
                var hostBuilder = Host.CreateDefaultBuilder(args)
                                       .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

                IHost host = hostBuilder.Build();
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
