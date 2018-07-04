using System;
using System.Data;
using System.Data.Common;

namespace Code4Bugs.SimpleDataGridViewPaging.Statement
{
    /// <inheritdoc cref="IStatement{T}" />
    /// <summary>
    /// The statement supports the update capability.
    /// </summary>
    public class AdapterStatement : IStatement<DataTable>, IDisposable
    {
        private readonly DbDataAdapter _adapter;
        private readonly DbCommandBuilder _commandBuilder;
        private DataTable _dataTable;

        public AdapterStatement(DbCommand selectCommand, DbCommandBuilder commandBuilder)
        {
            _commandBuilder = commandBuilder;
            _adapter = commandBuilder.DataAdapter;
            _adapter.SelectCommand = selectCommand;
            _adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
            _adapter.InsertCommand = commandBuilder.GetInsertCommand();
            _adapter.DeleteCommand = commandBuilder.GetDeleteCommand();
        }

        public void Dispose()
        {
            DisposeTableIfNecessary();
        }

        public virtual DataTable Execute()
        {
            DisposeTableIfNecessary();
            _dataTable = new DataTable();
            _adapter.Fill(_dataTable);
            return _dataTable;
        }

        private void DisposeTableIfNecessary()
        {
            if (_dataTable != null)
            {
                _adapter.Update(_dataTable);
                _dataTable.Dispose();
            }
        }
    }
}