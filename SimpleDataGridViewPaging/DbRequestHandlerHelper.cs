using System.Data.Common;
using Code4Bugs.SimpleDataGridViewPaging.Statement.Builder;

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

        public static IDbRequestHandler Create(DbConnection connection, string tableName,
            DbCommandBuilder commandBuilder)
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