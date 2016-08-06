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
            connection = new SQLiteConnection("Data Source=chinook.db");
            connection.Open();

            dataGridViewPaging1.UserHardMode(connection, "tracks");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            connection.Dispose();
        }
    }
}
