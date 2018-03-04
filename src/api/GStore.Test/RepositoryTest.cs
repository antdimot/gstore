using GStore.Core.Data;
using GStore.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GStore.Test
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var connectionString = "mongodb://host:27017";

            var dbname = "demo";

            var dataContext = new DataContext( connectionString, dbname );

            var db = dataContext.GetDatabase();

            //var repository = new Repository<User>( dataContext );

            //var result = repository.List();

            //Assert.IsTrue( result.Count > 0, "no data found" );
        }
    }
}
