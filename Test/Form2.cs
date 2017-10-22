using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Code4Bugs.SimpleDataGridViewPaging.Test
{
    public partial class Form2 : Form
    {
        private readonly SQLiteConnection _connection;

        public Form2()
        {
            InitializeComponent();
            // connect to database
            _connection = new SQLiteConnection("Data Source=chinook.db");
            _connection.Open();

            // just pass a connection and table name which you want to
            // display and then DataGridViewPaging will do everything for you
            dataGridViewPaging1.UserHardMode(_connection, "tracks");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // free resource
            _connection.Dispose();
        }
    }
}