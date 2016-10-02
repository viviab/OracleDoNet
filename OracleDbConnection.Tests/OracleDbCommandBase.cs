using System;
using System.Collections.Generic;
using System.Data;
using Habitania.OracleDbConnection.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.DataExtensions;
using Oracle.DataAccess.Client;
using Ploeh.AutoFixture;

namespace Habitania.OracleDbConnection.Tests
{
    [TestClass]
    public class OracleDbCommandBase
    {

        protected Mock<IDbConnectionFactory> DbConnection;
        protected Mock<IDbConnection> MockConnection;
        protected Mock<IDbCommand> MockCommand;

        protected IOracleDbCommand ServiceOracleDbCommand;
        protected Object Result;
        protected IEnumerable<OracleParameter> Parameters;
        protected string Query;

        public class ClsMyClass
        {
            public ClsMyClass(IDataReader reader, params Object[] values)
            {
                ID = reader.GetSafe<string>("ID");
            }

            public string ID { get; set; }
        }

        [TestInitialize]
        public void BaseTestInit()
        {
            DbConnection = new Mock<IDbConnectionFactory>();
            MockConnection = new Mock<IDbConnection>();
            ServiceOracleDbCommand = new OracleDbCommand(DbConnection.Object);

            var mockFactory = new MockFactory(MockBehavior.Default)
            {
                CallBase = true, 
                DefaultValue = DefaultValue.Mock
            };

            MockCommand = mockFactory.CreateIDbCommand();
            MockConnection.Setup(m => m.CreateCommand()).Returns(MockCommand.Object);

            var fixture = new Fixture();
            Parameters = fixture.Create<IEnumerable<OracleParameter>>();

            Query = fixture.Create<string>();

        }

        protected void Execute()
        {
            Result = ServiceOracleDbCommand.Execute(Query, Parameters);
        }


        protected void ExecuteInsert()
        {
            Result = ServiceOracleDbCommand.ExecuteInsert(Query, Parameters);
        }

        protected void ExecuteScalar()
        {
            Result = ServiceOracleDbCommand.ExecuteScalar(Query, Parameters);
        }

        protected void ExecuteScalarString()
        {
            Result = ServiceOracleDbCommand.ExecuteScalarString(Query, Parameters);
        }

        protected void ExecuteNonQuery()
        {
            Result = ServiceOracleDbCommand.ExecuteNonQuery(Query, Parameters);
        }

        protected void ExecuteGetListInt()
        {
            Result = ServiceOracleDbCommand.GetListInt(Query, Parameters);
        }

        protected void ExecuteGetList()
        {
            Result = ServiceOracleDbCommand.GetList<ClsMyClass>(Query, Parameters);
        }

        protected void SetupMock()
        {
            DbConnection.Setup(m => m.CreateConnection()).Returns(MockConnection.Object);
        }


    }
}
