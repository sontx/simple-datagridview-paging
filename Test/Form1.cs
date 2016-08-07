using System;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Test
{
    public partial class Form1 : Form
    {
        private SQLiteConnection connection;

        public Form1()
        {
            InitializeComponent();
            // connect to database
            connection = new SQLiteConnection("Data Source=chinook.db");
            connection.Open();
            
            // register request query event
            dataGridViewPaging1.RequestQueryData += DataGridViewPaging1_RequestQueryData;

            // set number of rows(records) and start query data
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM tracks";
                var reader = command.ExecuteScalar();
                dataGridViewPaging1.Initialize(Convert.ToInt32(reader));
            }
        }

        private void DataGridViewPaging1_RequestQueryData(object sender, SimpleDataGridViewPaging.RequestQueryDataEventArgs e)
        {
            // query data and then set result to display
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM tracks LIMIT " + e.MaxRecords + " OFFSET " + e.PageOffset;
                dataGridViewPaging1.DataSource = command.ExecuteReader();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            // free resource
            connection.Dispose();
        }
    }
}
