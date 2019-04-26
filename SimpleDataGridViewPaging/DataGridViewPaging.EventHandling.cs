using System;
using System.Windows.Forms;

namespace Code4Bugs.SimpleDataGridViewPaging
{
    public partial class DataGridViewPaging
    {
        private void DataGridViewPaging_Disposed(object sender, EventArgs e)
        {
            Disposed -= DataGridViewPaging_Disposed;
            DbRequestHandler?.Dispose();
        }

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker) UpdateNavigatorButtons);
            else
                UpdateNavigatorButtons();
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            _currentPageOffset = 0;
            QueryData();
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            _currentPageOffset -= MaxRecords;
            QueryData();
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            _currentPageOffset += MaxRecords;
            QueryData();
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            _currentPageOffset = (TotalPages - 1) * MaxRecords;
            QueryData();
        }

        private void bindingNavigatorPositionItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char) Keys.Enter) return;

            if (int.TryParse(bindingNavigatorPositionItem.Text, out var pageNumber))
            {
                if (pageNumber == CurrentPage) return;

                var totalPages = TotalPages;
                if (pageNumber >= 1 && pageNumber <= totalPages)
                {
                    _currentPageOffset = (pageNumber - 1) * MaxRecords;
                    QueryData();
                }
                else
                {
                    MessageBox.Show($@"Page number must be from 1 to {totalPages}.", Application.ProductName);
                }
            }
            else
            {
                MessageBox.Show(@"You must enter a page number to navigate.", Application.ProductName);
                bindingNavigatorPositionItem.Text = CurrentPage.ToString();
            }
        }
    }
}