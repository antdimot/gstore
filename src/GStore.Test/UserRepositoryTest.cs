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
    public class UserRepositoryTest
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

        [TestMethod]
        public async Task GetList_Shouldreturn_Notemptylist()
        {  
            var work = new UnitOfWork ( _context );

            var result = await work.Repository<User>().GetListAsync( u => u.Enabled );

            Assert.IsTrue( result.Count > 0, "no data found" );
        }

        [TestMethod]
        public async Task GetSingle_Shouldreturn_Notnull()
        {
            var work = new UnitOfWork( _context );

            var result = await work.Repository<User>().GetSingleAsync( u => u.Firstname == "firstname" );

            Assert.IsNotNull( result, "no data found" );
        }

        [TestMethod]
        public async Task Insert_Shouldreturn_Newid()
        {
            var work = new UnitOfWork( _context );

            var result = await work.Repository<User>().InsertAsync( new User {
                Firstname = "firstname",
                Lastname = "lastname",
                Username = "username",
                Password = "password"
            } );

            Assert.IsNotNull( result.Id, "no data found" );
        }
    }
}
