using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Test
{
    public partial class Form2 : Form
    {
        private SQLiteConnection connection;

        public Form2()
        {
            InitializeComponent();
            // connect to database
            connection = new SQLiteConnection("Data Source=chinook.db");
            connection.Open();

            // just pass a connection and table name which you want to
            // display and then DataGridViewPaging will do everything for you
            dataGridViewPaging1.UserHardMode(connection, "tracks");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // free resource
            connection.Dispose();
        }
    }
}
