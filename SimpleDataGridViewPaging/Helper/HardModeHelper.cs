using System.Data.Common;

namespace Code4Bugs.SimpleDataGridViewPaging.Helper
{
    public class HardModeHelper : AutoModeHelper
    {
        public HardModeHelper(DataGridViewPaging dataGridViewPaging, DbConnection connection, string tableName)
            : base(dataGridViewPaging, connection)
        {
            SelectCountCommandText = $"SELECT COUNT(*) FROM {tableName}";
            SelectColumnsCommandText = $"SELECT * FROM {tableName}";
        }
    }
}