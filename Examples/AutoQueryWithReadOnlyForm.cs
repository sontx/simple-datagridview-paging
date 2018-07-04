using System.Data.SQLite;
using System.Windows.Forms;

namespace Code4Bugs.SimpleDataGridViewPaging.Examples
{
    public partial class AutoQueryWithReadOnlyForm : Form
    {
        public AutoQueryWithReadOnlyForm()
        {
            InitializeComponent();

            dataGridViewPaging1.DbRequestHandler = DbRequestHandlerHelper.Create(
                new SQLiteConnection("Data Source=chinook.db"),
                "tracks");

            // OR
            // dataGridViewPaging1.DbRequestHandler = new DbRequestHandler
            // {
            //     Connection = new SQLiteConnection("Data Source=chinook.db"),
            //     TableName = "tracks"
            // };
        }
    }
}