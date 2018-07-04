using System.Data.SQLite;
using System.Windows.Forms;

namespace Code4Bugs.SimpleDataGridViewPaging.Examples
{
    public partial class AutoQueryWithEditableForm : Form
    {
        public AutoQueryWithEditableForm()
        {
            InitializeComponent();

            dataGridViewPaging1.DbRequestHandler = DbRequestHandlerHelper.Create(
                new SQLiteConnection("Data Source=chinook.db"),
                "tracks",
                new SQLiteCommandBuilder(new SQLiteDataAdapter()));

            //  OR
            //  dataGridViewPaging1.DbRequestHandler = new DbRequestHandler
            //  {
            //      Connection = new SQLiteConnection("Data Source=chinook.db"),
            //      TableName = "tracks",
            //      RowsStatementBuilder = new UpdatableRowsStatementBuilder().CommandBuilder(new SQLiteCommandBuilder(new SQLiteDataAdapter()))
            //  };
        }
    }
}