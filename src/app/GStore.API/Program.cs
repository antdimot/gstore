using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace GStore.API
{
    public class Program
    {
        //public static void Main(string[] args)
        //{
        //    BuildWebHost(args).Run();
        //}

        //public static IWebHost BuildWebHost( string[] args ) =>
        //WebHost.CreateDefaultBuilder( args )
        //   .UseStartup<Startup>()
        //   .Build();

        public static int Main( string[] args )
        {
            try
            {
                var hostBuilder = WebHost.CreateDefaultBuilder( args );

                var host = hostBuilder.UseStartup<Startup>()
                                      .UseSerilog( ( hostingContext, loggerConfiguration ) => loggerConfiguration
                                                        .ReadFrom.Configuration( hostingContext.Configuration ) )
                                      .Build();

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
