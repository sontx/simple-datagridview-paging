using System;
using System.Data.Common;

namespace Code4Bugs.SimpleDataGridViewPaging.Helper
{
    public abstract class AutoModeHelper : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DataGridViewPaging _dataGridViewPaging;
        protected string SelectColumnsCommandText;
        protected string SelectCountCommandText;

        protected AutoModeHelper(DataGridViewPaging dataGridViewPaging, DbConnection connection)
        {
            _dataGridViewPaging = dataGridViewPaging;
            _connection = connection;
            _dataGridViewPaging.RequestQueryData += DataGridViewPaging_RequestQueryData;
        }

        public void Initialize()
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = SelectCountCommandText;
                var numberOfRecords = Convert.ToInt32(command.ExecuteScalar());
                _dataGridViewPaging.Initialize(numberOfRecords);
            }
        }

        private void DataGridViewPaging_RequestQueryData(object sender, RequestQueryDataEventArgs e)
        {
            using (var command = _connection.CreateCommand())
            {
                command.CommandText = $"{SelectColumnsCommandText} LIMIT {e.MaxRecords} OFFSET {e.PageOffset}";
                var reader = command.ExecuteReader();
                _dataGridViewPaging.DataSource = reader;
            }
        }

        public void Dispose()
        {
            _dataGridViewPaging.RequestQueryData -= DataGridViewPaging_RequestQueryData;
        }
    }
}