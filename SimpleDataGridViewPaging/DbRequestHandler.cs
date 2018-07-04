using System;
using System.Data;
using System.Data.Common;
using Code4Bugs.SimpleDataGridViewPaging.Statement;
using Code4Bugs.SimpleDataGridViewPaging.Statement.Builder;

namespace Code4Bugs.SimpleDataGridViewPaging
{
    public class DbRequestHandler : IDbRequestHandler
    {
        private Helper<int> _countHelper;
        private bool _disposed;
        private bool _initialized;
        private int _numberOfRecords = -1;
        private Helper<object> _rowsHelper;
        private bool _shouldCloseConnection;

        public DbConnection Connection { get; set; }
        public string TableName { get; set; }
        public CountStatementBuilder CountStatementBuilder { get; set; }
        public RowsStatementBuilder RowsStatementBuilder { get; set; }

        public int NumberOfRecords => _numberOfRecords < 0
            ? _numberOfRecords = QueryNumberOfRecords()
            : _numberOfRecords;

        public virtual object DataSource(int maxRecords, int pageOffset)
        {
            InitializeIfNecessary();

            _rowsHelper.StatementBuilder = RowsStatementBuilder
                .MaxRecords(maxRecords)
                .PageOffset(pageOffset);

            return _rowsHelper
                .Build()
                .Execute();
        }

        public void Dispose()
        {
            Disposing(true);
            GC.SuppressFinalize(this);
        }

        ~DbRequestHandler()
        {
            Disposing(false);
        }

        private int QueryNumberOfRecords()
        {
            InitializeIfNecessary();

            return _countHelper
                .Build()
                .Execute();
        }

        private void InitializeIfNecessary()
        {
            if (!_initialized)
            {
                if (CountStatementBuilder == null)
                    CountStatementBuilder = new CountStatementBuilder();

                _countHelper = new Helper<int>
                {
                    StatementBuilder = CountStatementBuilder,
                    TableName = TableName,
                    Connection = Connection
                };

                if (RowsStatementBuilder == null)
                    RowsStatementBuilder = new RowsStatementBuilder();

                _rowsHelper = new Helper<object>
                {
                    StatementBuilder = RowsStatementBuilder,
                    TableName = TableName,
                    Connection = Connection
                };

                if (Connection != null && Connection.State == ConnectionState.Closed)
                {
                    Connection.Open();
                    _shouldCloseConnection = true;
                }

                _initialized = true;
            }
        }

        protected virtual void Disposing(bool disposing)
        {
            if (!_disposed)
                lock (this)
                {
                    _countHelper?.Dispose();
                    _rowsHelper?.Dispose();
                    if (Connection != null && _shouldCloseConnection)
                        Connection.Close();
                    _disposed = true;
                }
        }

        private class Helper<T> : IDisposable
        {
            private IStatement<T> _previousStatement;

            public StatementBuilder<T> StatementBuilder { get; set; }
            public string TableName { get; set; }
            public DbConnection Connection { get; set; }

            public void Dispose()
            {
                DisposeStatement();
                if (StatementBuilder is IDisposable disposable)
                    disposable.Dispose();
            }

            public IStatement<T> Build()
            {
                DisposeStatement();

                _previousStatement = StatementBuilder
                    .TableName(TableName)
                    .Connection(Connection)
                    .Build();

                return _previousStatement;
            }

            private void DisposeStatement()
            {
                if (_previousStatement is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }
}