using System;
using System.Data.Common;

namespace Code4Bugs.SimpleDataGridViewPaging.Helper
{
    public class SoftModeHelper : AutoModeHelper
    {
        public SoftModeHelper(
            DataGridViewPaging dataGridViewPaging,
            DbConnection connection,
            string selectCommandText)
            : base(dataGridViewPaging, connection)
        {
            SelectCountCommandText = $"SELECT COUNT(*) {selectCommandText.Substring(selectCommandText.IndexOf("FROM", StringComparison.Ordinal))}";
            SelectColumnsCommandText = selectCommandText;
        }
    }
}