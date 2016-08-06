using SimpleDataGridViewPaging.Exceptions;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;

namespace SimpleDataGridViewPaging
{
    public partial class DataGridViewPaging : UserControl
    {
        private const string CATEGORY_CONTROL = "Control";
        private const string CATEGORY_BEHAVIOR = "Behavior";

        #region Events

        /// <summary>
        /// Occurs when a request query new data page.
        /// You should query to database with some statements like 
        /// <code>
        /// SELECT * FROM my_table LIMIT maxRecords OFFSET pageOffset
        /// </code>
        /// </summary>
        public event RequestQueryDataEventHandler RequestQueryData;

        #endregion

        #region Variants

        private bool _readonly = false;
        private int _maxRecords = 100;
        private bool _autoHideNavigator = false;

        private int numberOfRecords = 0;
        private int currentPageOffset = 0;
        private IDisposable lastDataSource = null;

        #endregion

        #region Properties

        [Browsable(true)]
        [Description("DataGridView which DataGridViewPaging uses to perform data.")]
        [Category(CATEGORY_CONTROL)]
        public DataGridView DataGridView { get { return dataGridView; } }

        [Browsable(true)]
        [Description("BindingNavigator which user uses to paging data in DataGridViewPaging.")]
        [Category(CATEGORY_CONTROL)]
        public BindingNavigator BindingNavigator { get { return bindingNavigator; } }

        [Browsable(true)]
        [Description("Gets or sets a value indicating whether DataGridView in the DataGridViewPaging is read-only.")]
        [Category(CATEGORY_BEHAVIOR)]
        public bool ReadOnly
        {
            get { return _readonly; }
            set
            {
                _readonly = value;

                dataGridView.ReadOnly = _readonly;

                bindingNavigatorAddNewItem.Visible = !_readonly;
                bindingNavigatorDeleteItem.Visible = !_readonly;
                bindingNavigatorSeparator2.Visible = !_readonly;
            }
        }

        [Browsable(true)]
        [Description("Gets or sets max records for each page.")]
        [Category(CATEGORY_BEHAVIOR)]
        public int MaxRecords
        {
            get { return _maxRecords; }
            set
            {
                if (value < 1)
                    throw new QuantityRangeException("Max records must be at least 1.");
                _maxRecords = value;
            }
        }

        [Browsable(true)]
        [Description("Gets or sets max records for each page.")]
        [Category(CATEGORY_BEHAVIOR)]
        public bool AutoHideNavigator
        {
            get { return _autoHideNavigator; }
            set
            {
                _autoHideNavigator = value;
                bindingNavigator.Visible = !(_autoHideNavigator && IsNotPaging);
            }
        }

        [Browsable(false)]
        [Description("Gets current page.")]
        public int CurrentPage { get { return currentPageOffset / _maxRecords + 1; } }

        [Browsable(false)]
        [Description("Gets total pages.")]
        public int TotalPages { get { return (numberOfRecords - 1) / _maxRecords + 1; } }

        [Browsable(false)]
        [Description("Gets or sets data source for DataGridViewPaging.")]
        public object DataSource
        {
            get { return bindingSource.DataSource; }
            set
            {
                lastDataSource?.Dispose();
                if (value is IDisposable)
                    lastDataSource = value as IDisposable;
                else
                    lastDataSource = null;
                if (InvokeRequired)
                    BeginInvoke((MethodInvoker)delegate { bindingSource.DataSource = value; });
                else
                    bindingSource.DataSource = value;
            }
        }

        private bool IsNotPaging { get { return TotalPages == 1 || !HasRows; } }

        private bool HasRows { get { return numberOfRecords > 0; } }

        private bool Nextable
        {
            set
            {
                bindingNavigatorMoveLastItem.Enabled = value;
                bindingNavigatorMoveNextItem.Enabled = value;
            }
        }

        private bool Backable
        {
            set
            {
                bindingNavigatorMoveFirstItem.Enabled = value;
                bindingNavigatorMovePreviousItem.Enabled = value;
            }
        }

        #endregion

        public DataGridViewPaging()
        {
            InitializeComponent();
            SetCenterHorizontalAlignment();
            dataGridView.AutoGenerateColumns = true;
            this.Disposed += DataGridViewPaging_Disposed;
        }

        #region Methods

        private void SetCenterHorizontalAlignment()
        {
            int totalItemsWidth = 0;
            var items = bindingNavigator.Items;
            for (int i = 0; i < items.Count; i++)
            {
                totalItemsWidth += items[i].Width;
            }

            int containerWidth = bindingNavigator.Width;
            int marginLeft = (containerWidth - totalItemsWidth) / 2;

            var firstItem = items[0];
            var firstItemMargin = firstItem.Margin;
            firstItemMargin.Left = marginLeft;
            firstItem.Margin = firstItemMargin;
        }

        private void UpdateNavigator()
        {
            if (IsNotPaging)
            {
                if (_autoHideNavigator)
                    bindingNavigator.Visible = false;
            }
            else
            {
                int totalPages = this.TotalPages;
                int currentPage = this.CurrentPage;

                bindingNavigatorCountItem.Text = string.Format(bindingNavigator.CountItemFormat, totalPages);
                bindingNavigatorPositionItem.Text = currentPage.ToString();
                bindingNavigatorPositionItem.Enabled = totalPages > 1;

                if (currentPage == 1)
                {
                    Backable = false;
                    Nextable = true;
                }
                else if (currentPage == totalPages)
                {
                    Backable = true;
                    Nextable = false;
                }
                else
                {
                    Nextable = true;
                    Backable = true;
                }

                bindingNavigator.Visible = true;
            }
        }

        /// <summary>
        /// Set number of records for <see cref="DataGridViewPaging"/>.
        /// </summary>
        /// <param name="numberOfRecords">
        /// Number of records, you could use some statements like
        /// <code>
        /// SELECT COUNT(*) FROM my_table
        /// </code>
        /// </param>
        public void Initialize(int numberOfRecords)
        {
            if (numberOfRecords < 0)
                throw new QuantityRangeException("Number of records must at least 0.");
            this.numberOfRecords = numberOfRecords;
            this.currentPageOffset = 0;
            this.lastDataSource?.Dispose();
            this.lastDataSource = null;
            this.QueryData();
        }

        private void QueryData()
        {
            RequestQueryData?.Invoke(this, new RequestQueryDataEventArgs(MaxRecords, currentPageOffset));
        }

        #endregion

        #region UserControl Event Handlers

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            SetCenterHorizontalAlignment();
        }

        private void DataGridViewPaging_Disposed(object sender, EventArgs e)
        {
            this.Disposed -= DataGridViewPaging_Disposed;
            lastDataSource?.Dispose();
        }

        #endregion

        #region DataGridView Event Handlers

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker)delegate { UpdateNavigator(); });
            else
                UpdateNavigator();
        }

        #endregion

        #region BindingNavigator Event Handlers

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            currentPageOffset = 0;
            QueryData();
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            currentPageOffset -= MaxRecords;
            QueryData();
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            currentPageOffset += MaxRecords;
            QueryData();
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            currentPageOffset = (TotalPages - 1) * MaxRecords;
            QueryData();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorPositionItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                int pageNumber;
                if (int.TryParse(bindingNavigatorPositionItem.Text, out pageNumber))
                {
                    if (pageNumber != CurrentPage)
                    {
                        int totalPages = this.TotalPages;
                        if (pageNumber < 1 || pageNumber > totalPages)
                        {
                            currentPageOffset = (pageNumber - 1) * MaxRecords;
                            QueryData();
                        }
                        else
                        {
                            MessageBox.Show(string.Format("Page number must be from 1 to {0}.", totalPages), Application.ProductName);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("You must enter a page number to navigate.", Application.ProductName);
                    bindingNavigatorPositionItem.Text = CurrentPage.ToString();
                }
            }
        }

        #endregion
    }
}
