using GStore.Core.Data;
using GStore.Core.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace GStore.Test
{
    [TestClass]
    public class RepositoryTest
    {
        private static string _connectionString;
        private static string _dbname;

        //[ClassInitialize]
        //public static void ClassInit( TestContext context ) { }

        [TestInitialize]
        public void Initialize()
        {
            _connectionString = "mongodb://localhost:27017";
            _dbname = "demo";
        }

        [TestMethod]
        public void GetList_shouldreturn_notempty_list()
        {
            

            //var context = new DataContext( _connectionString, _dbname );

            //var work = new UnitOfWork( context );

            //var result = work.Repository<User>().GetList();

            //Assert.IsTrue( result.Count > 0, "no data found" );
        }

        //[TestMethod]
        //public void GetSingle_shouldreturn_notnull()
        //{
        //    var context = new DataContext( _connectionString, _dbname );

        //    var work = new UnitOfWork( context );

        //    var result = work.Repository<User>().GetSingle( u =>u.Firstname == "antonio" );

        //    Assert.IsNotNull( result, "no data found" );
        //}

        //[TestMethod]
        //public void Insert_shouldreturn_newid()
        //{
        //    var context = new DataContext( _connectionString, _dbname );

        //    var work = new UnitOfWork( context );

        //    var result = work.Repository<User>().Insert( new User {
        //        Firstname = "firstname",
        //        Lastname = "lastname"
        //    } );

        //    Assert.IsNotNull( result.Id, "no data found" );
        //}
    }
}
