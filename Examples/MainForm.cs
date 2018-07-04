using System;
using System.Windows.Forms;

namespace Code4Bugs.SimpleDataGridViewPaging.Examples
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var form = new AutoQueryWithReadOnlyForm())
            {
                form.ShowDialog(this);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var form = new AutoQueryWithEditableForm())
            {
                form.ShowDialog(this);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var form = new CustomQueryTextWithReadOnlyForm())
            {
                form.ShowDialog(this);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var form = new ManualQueryWithReadOnlyForm())
            {
                form.ShowDialog(this);
            }
        }
    }
}