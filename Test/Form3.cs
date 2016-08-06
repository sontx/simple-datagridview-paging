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
            connection = new SQLiteConnection("Data Source=chinook.db");
            connection.Open();

            dataGridViewPaging1.UserSoftMode(connection, "SELECT Name, Composer, Bytes FROM tracks WHERE TrackId < 500");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            connection.Dispose();
        }
    }
}
