using GStore.Core.Data;
using GStore.Core.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using System.IO;
using Microsoft.Extensions.Logging;

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
        public void GetList_shouldreturn_notempty_list()
        {  
            var work = new UnitOfWork ( _context );

            var result = work.Repository<User>().GetListAsync();

            Assert.IsTrue( result.Result.Count > 0, "no data found" );
        }

        [TestMethod]
        public void GetSingle_shouldreturn_notnull()
        {
            var work = new UnitOfWork( _context );

            var result = work.Repository<User>().GetSingleAsync( u => u.Firstname == "Antonio" );

            Assert.IsNotNull( result.Result, "no data found" );
        }

        [TestMethod]
        public void Insert_shouldreturn_newid()
        {
            var work = new UnitOfWork( _context );

            var result = work.Repository<User>().Insert( new User {
                Firstname = "firstname",
                Lastname = "lastname",
                Username = "username",
                Password = "password",
                Email = "email"
            } );

            Assert.IsNotNull( result.Id, "no data found" );
        }
    }
}
