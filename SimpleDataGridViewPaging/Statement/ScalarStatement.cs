using System;
using System.Data;

namespace Code4Bugs.SimpleDataGridViewPaging.Statement
{
    public class ScalarStatement : IStatement<int>
    {
        private readonly IDbConnection _connection;
        private readonly string _commandText;

        public ScalarStatement(IDbConnection connection, string commandText)
        {
            _connection = connection;
            _commandText = commandText;
        }

        public int Execute()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = _commandText;
                var result = command.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
    }
}