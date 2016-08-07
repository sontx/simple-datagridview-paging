using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Test
{
    public partial class Form3 : Form
    {
        private SQLiteConnection connection;

        public Form3()
        {
            InitializeComponent();
            // connect to database
            connection = new SQLiteConnection("Data Source=chinook.db");
            connection.Open();

            // pass a connection with a query string which will be used
            // to query data from database to display for you
            dataGridViewPaging1.UserSoftMode(connection, "SELECT Name, Composer, Bytes FROM tracks WHERE TrackId < 500");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // free resource
            connection.Dispose();
        }
    }
}
