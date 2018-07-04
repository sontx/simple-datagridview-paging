namespace Code4Bugs.SimpleDataGridViewPaging.Statement.Builder
{
    /// <summary>
    /// Queries number of records of the table.
    /// </summary>
    public class CountStatementBuilder : StatementBuilder<int>
    {
        public override IStatement<int> Build()
        {
            var commandText = string.IsNullOrEmpty(_commandText)
                ? $"SELECT COUNT(*) FROM {_tableName}"
                : _commandText;
            return new ScalarStatement(_connection, commandText);
        }
    }
}