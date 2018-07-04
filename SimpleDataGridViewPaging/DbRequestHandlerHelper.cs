using Code4Bugs.SimpleDataGridViewPaging.Statement.Builder;
using System.Data.Common;

namespace Code4Bugs.SimpleDataGridViewPaging
{
    public static class DbRequestHandlerHelper
    {
        public static IDbRequestHandler Create(DbConnection connection, string tableName)
        {
            return new DbRequestHandler
            {
                Connection = connection,
                TableName = tableName
            };
        }

        public static IDbRequestHandler Create(DbConnection connection, string tableName, DbCommandBuilder commandBuilder)
        {
            return new DbRequestHandler
            {
                Connection = connection,
                TableName = tableName,
                RowsStatementBuilder = new UpdatableRowsStatementBuilder().CommandBuilder(commandBuilder)
            };
        }
    }
}