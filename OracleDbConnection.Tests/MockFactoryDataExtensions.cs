using System;
using System.Collections;
using System.Data;
using System.Linq;
using Moq;
using Ploeh.AutoFixture;

namespace Moq.DataExtensions {

  public static class MockFactoryDataExtensions {

    public static Mock<IDbCommand> CreateIDbCommand(this MockFactory factory) {
      var command = factory.Create<IDbCommand>();

      command.SetupAllProperties();
      command.Setup(c => c.CreateParameter()).Returns(() => factory.CreateIDbDataParameter().Object);
      command.Setup(c => c.Parameters).Returns(factory.CreateIDataParameterCollection().Object);

      command.Setup(m => m.ExecuteNonQuery()).Returns(GenerateInt());
      command.Setup(m => m.ExecuteScalar()).Returns(GenerateInt());
      command.Setup(m => m.ExecuteReader()).Returns(factory.CreateIDataReader().Object);

      return command;
    }

    public static Mock<IDataParameterCollection> CreateIDataParameterCollection(this MockFactory factory) {
      var list = new ArrayList(); // ArrayList more closely matches IDataParameterCollection.
      var parameters = factory.Create<IDataParameterCollection>();

      parameters.Setup(p => p.Add(It.IsAny<IDataParameter>())).Returns((IDataParameter p) => list.Add(p));
      parameters.Setup(p => p[It.IsAny<int>()]).Returns((int i) => list[i]);
      parameters.Setup(p => p[It.IsAny<string>()]).Returns((string i) => GenerateInt().ToString());
      parameters.Setup(p => p.Count).Returns(() => list.Count);

      return parameters;
    }

    public static Mock<IDbDataParameter> CreateIDbDataParameter(this MockFactory factory) {
      var parameter = factory.Create<IDbDataParameter>();

      parameter.SetupAllProperties();

      return parameter;
    }

    public static Mock<IDataRecord> CreateIDataRecord(this MockFactory factory, params object[] fields) {
      var record = factory.Create<IDataRecord>();

      for(var index = 0; index < fields.Length; index++) {
        var column = fields[index];
        var type = column.GetType();
        var name = (string)type.GetProperty("Name").GetValue(column, null);
        var value = type.GetProperty("Value").GetValue(column, null);

        record.Setup(r => r.IsDBNull(index)).Returns(value == DBNull.Value);
        record.Setup(r => r.GetOrdinal(name)).Returns(index);
        record.Setup(r => r[index]).Returns(value);
      }

      return record;
    }

      public static Mock<IDataReader> CreateIDataReader(this MockFactory factory)
      {
          var reader = factory.Create<IDataReader>();
          var readToggle = true;


          reader.Setup(r => r.Read()).Returns(() => readToggle).Callback(() => readToggle = false);
          reader.Setup(x => x[It.IsAny<string>()]).Returns(GenerateString());
          reader.Setup(x => x.GetInt32(It.IsAny<int>())).Returns(GenerateInt());

          return reader;
      }

      private static int GenerateInt()
      {
          var fixture = new Fixture();
          var generator = fixture.Create<Generator<int>>();

          return generator.Where(x => x > 0).Distinct().FirstOrDefault();
      }

      private static string GenerateString()
      {
          var fixture = new Fixture();
          return fixture.Create<string>();
      }

  }
}
