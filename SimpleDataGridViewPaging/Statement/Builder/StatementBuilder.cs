using System.Data.Common;

namespace Code4Bugs.SimpleDataGridViewPaging.Statement.Builder
{
    public abstract class StatementBuilder<T>
    {
        protected string _tableName;
        protected string _commandText;
        protected DbConnection _connection;

        public StatementBuilder<T> Connection(DbConnection connection)
        {
            _connection = connection;
            return this;
        }

        public StatementBuilder<T> TableName(string tableName)
        {
            _tableName = tableName;
            return this;
        }

        public StatementBuilder<T> CommandText(string commandText)
        {
            _commandText = commandText;
            return this;
        }

        public abstract IStatement<T> Build();
    }
}