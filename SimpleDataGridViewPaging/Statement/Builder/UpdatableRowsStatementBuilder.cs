using System;
using System.Data.Common;

namespace Code4Bugs.SimpleDataGridViewPaging.Statement.Builder
{
    public class UpdatableRowsStatementBuilder : RowsStatementBuilder, IDisposable
    {
        private DbCommandBuilder _commandBuilder;

        public UpdatableRowsStatementBuilder CommandBuilder(DbCommandBuilder commandBuilder)
        {
            _commandBuilder = commandBuilder;
            return this;
        }

        public override IStatement<object> Build()
        {
            var command = _connection.CreateCommand();
            command.CommandText = GetCommandText();
            return new AdapterStatement(command, _commandBuilder);
        }

        public void Dispose()
        {
            if (_commandBuilder != null)
            {
                _commandBuilder.Dispose();
                _commandBuilder.DataAdapter?.Dispose();
            }
        }
    }
}