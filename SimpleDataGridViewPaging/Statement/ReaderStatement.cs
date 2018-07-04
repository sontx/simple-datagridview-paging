using System.Data;

namespace Code4Bugs.SimpleDataGridViewPaging.Statement
{
    public class ReaderStatement : IStatement<object>
    {
        private readonly IDbConnection _connection;
        private readonly string _commandText;

        public ReaderStatement(IDbConnection connection, string commandText)
        {
            _connection = connection;
            _commandText = commandText;
        }

        public object Execute()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = _commandText;
                return command.ExecuteReader();
            }
        }
    }
}