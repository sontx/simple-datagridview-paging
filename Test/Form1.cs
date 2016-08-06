using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            connection = new SQLiteConnection("Data Source=chinook.db");
            connection.Open();
            
            dataGridViewPaging1.RequestQueryData += DataGridViewPaging1_RequestQueryData;

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM tracks";
                var reader = command.ExecuteScalar();
                dataGridViewPaging1.Initialize(Convert.ToInt32(reader));
            }
        }

        private void DataGridViewPaging1_RequestQueryData(object sender, SimpleDataGridViewPaging.RequestQueryDataEventArgs e)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM tracks LIMIT " + e.MaxRecords + " OFFSET " + e.PageOffset;
                dataGridViewPaging1.DataSource = command.ExecuteReader();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            connection.Dispose();
        }
    }
}
