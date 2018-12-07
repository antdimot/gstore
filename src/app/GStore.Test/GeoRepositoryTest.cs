using GStore.Core.Data;
using GStore.Core.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GStore.Test
{
    [TestClass]
    public class GeoRepositoryTest
    {
        DataContext _context;

        [TestInitialize]
        public void Initialize()
        {
            var configurationBuilder = new ConfigurationBuilder()
                                             .SetBasePath( Directory.GetCurrentDirectory() )
                                             .AddJsonFile( "appsettings.json" );

            var configuration = configurationBuilder.Build();

            var logger = new LoggerFactory().CreateLogger<DataContext>();

            _context = new DataContext( configuration, logger );
        }
    }
}
