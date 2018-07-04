using System;
using System.Data;
using System.Data.Common;

namespace Code4Bugs.SimpleDataGridViewPaging.Statement
{
    public class AdapterStatement : IStatement<DataTable>, IDisposable
    {
        private readonly DbCommandBuilder _commandBuilder;
        private readonly DbDataAdapter _adapter;
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

        public void Dispose()
        {
            DisposeTableIfNecessary();
        }
    }
}