using System.Data.SQLite;
using System.Windows.Forms;
using Code4Bugs.SimpleDataGridViewPaging.Statement.Builder;

namespace Code4Bugs.SimpleDataGridViewPaging.Examples
{
    public partial class CustomQueryTextWithReadOnlyForm : Form
    {
        public CustomQueryTextWithReadOnlyForm()
        {
            InitializeComponent();

            var countStatementBuilder = new CountStatementBuilder();
            countStatementBuilder.CommandText("SELECT COUNT(*) FROM tracks");

            var rowsStatementBuilder = new RowsStatementBuilder();
            rowsStatementBuilder.CommandText("SELECT * FROM tracks LIMIT {1} OFFSET {2}");

            dataGridViewPaging1.DbRequestHandler = new DbRequestHandler
            {
                Connection = new SQLiteConnection("Data Source=chinook.db"),
                CountStatementBuilder = countStatementBuilder,
                RowsStatementBuilder = rowsStatementBuilder
            };
        }
    }
}