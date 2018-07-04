using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Windows.Forms;
using Code4Bugs.SimpleDataGridViewPaging.Statement;
using Code4Bugs.SimpleDataGridViewPaging.Statement.Builder;

namespace Code4Bugs.SimpleDataGridViewPaging.Examples
{
    public partial class ManualQueryWithReadOnlyForm : Form
    {
        public ManualQueryWithReadOnlyForm()
        {
            InitializeComponent();

            var rowsStatementBuilder = new RowsStatementBuilder();
            rowsStatementBuilder.CommandText("SELECT * FROM tracks LIMIT {1} OFFSET {2}");

            dataGridViewPaging1.DbRequestHandler = new DbRequestHandler
            {
                Connection = new SQLiteConnection("Data Source=chinook.db"),
                CountStatementBuilder = new CustomCountStatementBuilder(),
                RowsStatementBuilder = new CustomRowsStatementBuilder()
            };
        }

        private class CustomCountStatementBuilder : CountStatementBuilder
        {
            public override IStatement<int> Build()
            {
                return new CountStatement {Connection = _connection};
            }

            private class CountStatement : IStatement<int>
            {
                public DbConnection Connection { get; set; }

                public int Execute()
                {
                    using (var command = Connection.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM tracks";
                        var result = command.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
        }

        private class CustomRowsStatementBuilder : RowsStatementBuilder
        {
            public override IStatement<object> Build()
            {
                return new RowsStatement
                {
                    Connection = _connection,
                    MaxRecords = _maxRecords,
                    PageOffset = _pageOffset
                };
            }

            private class RowsStatement : IStatement<object>
            {
                public DbConnection Connection { get; set; }
                public int MaxRecords { get; set; }
                public int PageOffset { get; set; }

                public object Execute()
                {
                    using (var command = Connection.CreateCommand())
                    {
                        command.CommandText = $"SELECT * FROM tracks LIMIT {MaxRecords} OFFSET {PageOffset}";
                        return command.ExecuteReader();
                    }
                }
            }
        }
    }
}