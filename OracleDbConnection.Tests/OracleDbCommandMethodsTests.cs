using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace Habitania.OracleDbConnection.Tests
{
    [TestClass]
    public class OracleDbCommandMethodsTests : OracleDbCommandBase
    {
        [TestMethod]
        public void WhenCallExecuteThenVerify()
        {
    
            //ARRANGE:
            SetupMock();

            //ACT:
            Execute();

            //ASSERT:
            CommonAssert();
            Assert.AreEqual(MockCommand.Object.Parameters.Count, Parameters.Count());
            
            MockCommand.Verify(m=> m.ExecuteNonQuery(), Times.Once);
            Assert.IsTrue((int)Result > 0);

        }

        [TestMethod]
        public void WhenCallExecuteInsertThenVerify()
        {

            //ARRANGE:
            SetupMock();

            //ACT:
            ExecuteInsert();

            //ASSERT:
            CommonAssert();
            Assert.AreEqual(MockCommand.Object.Parameters.Count, Parameters.Count()+1);
            
            MockCommand.Verify(m => m.ExecuteNonQuery(), Times.Once);
            Assert.IsTrue((int)Result > 0);
        }

        [TestMethod]
        public void WhenCallExecuteScalarThenVerify()
        {

            //ARRANGE:
            SetupMock();

            //ACT:
            ExecuteScalar();

            //ASSERT:
            CommonAssert();
            Assert.AreEqual(MockCommand.Object.Parameters.Count, Parameters.Count());
            

            MockCommand.Verify(m => m.ExecuteScalar(), Times.Once);
            Assert.IsTrue((int)Result > 0);

        }

        [TestMethod]
        public void WhenCallExecuteStringThenVerify()
        {

            //ARRANGE:
            SetupMock();

            //ACT:
            ExecuteScalar();

            //ASSERT:
            CommonAssert();
            Assert.AreEqual(MockCommand.Object.Parameters.Count, Parameters.Count());

            MockCommand.Verify(m => m.ExecuteScalar(), Times.Once);
            Assert.IsTrue((int)Result > 0);

        }


        [TestMethod]
        public void WhenCallExecuteNonQueryThenVerify()
        {

            //ARRANGE:
            SetupMock();

            //ACT:
            ExecuteNonQuery();

            //ASSERT:
            CommonAssert();
            Assert.AreEqual(MockCommand.Object.Parameters.Count, Parameters.Count());
            

            MockCommand.Verify(m => m.ExecuteNonQuery(), Times.Once);
            Assert.IsTrue((bool)Result);
        }


        [TestMethod]
        public void WhenCallExecuteGetListIntThenVerify()
        {

            //ARRANGE:
            SetupMock();

            //ACT:
            ExecuteGetListInt();

            //ASSERT:
            CommonAssert();
            Assert.AreEqual(MockCommand.Object.Parameters.Count, Parameters.Count());
            
            MockCommand.Verify(m => m.ExecuteReader(), Times.Once);
            var list = (List<int>) Result;
            Assert.IsTrue(list.Count==1);
            Assert.IsTrue(list.All(item => item>0));

        }


        [TestMethod]
        public void WhenCallExecuteGetListThenVerify()
        {

            //ARRANGE:
            SetupMock();

            //ACT:
            ExecuteGetList();

            //ASSERT:
            CommonAssert();
            Assert.AreEqual(MockCommand.Object.Parameters.Count, Parameters.Count());

            MockCommand.Verify(m => m.ExecuteReader(), Times.Once);
            var list = (List<ClsMyClass>) Result;
            Assert.IsTrue(list.Count == 1);
            Assert.IsTrue(list.All(item => item.ID!=string.Empty));

        }

        private void CommonAssert()
        {
            DbConnection.Verify(m => m.CreateConnection(), Times.Once);
            MockConnection.Verify(m => m.CreateCommand(), Times.Once);
            Assert.AreEqual(MockCommand.Object.CommandText, Query);
        
        }
    }




}
