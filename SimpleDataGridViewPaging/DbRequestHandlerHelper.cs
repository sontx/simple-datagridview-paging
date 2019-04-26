using System.Data.Common;
using System.Data.SqlClient;
using Code4Bugs.SimpleDataGridViewPaging.Statement.Builder;

namespace Code4Bugs.SimpleDataGridViewPaging
{
    public static class DbRequestHandlerHelper
    {
        private static readonly string SqlServerSelectCommandText = "SELECT * FROM {0} ORDER BY (SELECT NULL) OFFSET {2} ROWS FETCH NEXT {1} ROWS ONLY";

        public static IDbRequestHandler Create(DbConnection connection, string tableName)
        {
            var statementBuilder = connection.GetType() == typeof(SqlConnection)
                ? new RowsStatementBuilder().CommandText(SqlServerSelectCommandText) as RowsStatementBuilder
                : null;

            return new DbRequestHandler
            {
                Connection = connection,
                TableName = tableName,
                RowsStatementBuilder = statementBuilder
            };
        }

        public static IDbRequestHandler Create(
            DbConnection connection,
            string tableName,
            DbCommandBuilder commandBuilder)
        {
            var statementBuilder = new UpdatableRowsStatementBuilder().CommandBuilder(commandBuilder);
            if (connection.GetType() == typeof(SqlConnection))
                statementBuilder.CommandText(SqlServerSelectCommandText);

            return new DbRequestHandler
            {
                Connection = connection,
                TableName = tableName,
                RowsStatementBuilder = statementBuilder
            };
        }
    }
}