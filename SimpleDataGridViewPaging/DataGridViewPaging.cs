using Code4Bugs.SimpleDataGridViewPaging.Exceptions;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Windows.Forms;
using Code4Bugs.SimpleDataGridViewPaging.Helper;

namespace Code4Bugs.SimpleDataGridViewPaging
{
    /// <inheritdoc />
    /// <summary>
    ///     The <see cref="T:System.Windows.Forms.UserControl" /> contains <see cref="T:System.Windows.Forms.DataGridView" />
    ///     and <see cref="T:System.Windows.Forms.BindingNavigator" /> and it's capable of automatically paging.
    ///     This <see cref="T:System.Windows.Forms.UserControl" /> provide two choices are auto query and manual query.
    /// </summary>
    public partial class DataGridViewPaging : UserControl
    {
        private const string CategoryControl = "Control";
        private const string CategoryBehavior = "Behavior";

        public DataGridViewPaging()
        {
            InitializeComponent();
            AlignNavigatorToCenter();
            DataGridView.AutoGenerateColumns = true;
            ReadOnly = true;
            Disposed += DataGridViewPaging_Disposed;
        }

        #region Events

        /// <summary>
        ///     Occurs when a request query new data page.
        ///     You should query to database with some statements like
        ///     <code>
        ///         SELECT * FROM my_table LIMIT maxRecords OFFSET pageOffset
        ///     </code>
        /// </summary>
        public event RequestQueryDataEventHandler RequestQueryData;

        #endregion Events

        #region DataGridView Event Handlers

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke((MethodInvoker) UpdateNavigator);
            else
                UpdateNavigator();
        }

        #endregion DataGridView Event Handlers

        #region Variants

        private bool _readonly;
        private int _maxRecords = 100;
        private bool _autoHideNavigator;

        private int _numberOfRecords;
        private int _currentPageOffset;
        private IDisposable _lastDataSource;

        private AutoModeHelper _autoModeHelper;

        #endregion Variants

        #region Properties

        [Browsable(true)]
        [Description("DataGridView which DataGridViewPaging uses to perform data.")]
        [Category(CategoryControl)]
        public DataGridView DataGridView => dataGridView;

        [Browsable(true)]
        [Description("BindingNavigator which user uses to paging data in DataGridViewPaging.")]
        [Category(CategoryControl)]
        public BindingNavigator BindingNavigator => bindingNavigator;

        [Browsable(false)]
        [Description("Gets or sets a value indicating whether DataGridView in the DataGridViewPaging is read-only.")]
        [Category(CategoryBehavior)]
        private bool ReadOnly
        {
            get => _readonly;
            set
            {
                _readonly = value;

                DataGridView.ReadOnly = _readonly;

                if (ReadOnly)
                {
                    bindingNavigator.Items.Remove(bindingNavigatorAddNewItem);
                    bindingNavigator.Items.Remove(bindingNavigatorDeleteItem);
                    bindingNavigator.Items.Remove(bindingNavigatorSeparator2);
                }
                else
                {
                    bindingNavigator.Items.Remove(bindingNavigatorSeparator2);
                    bindingNavigator.Items.Remove(bindingNavigatorAddNewItem);
                    bindingNavigator.Items.Remove(bindingNavigatorDeleteItem);
                }

                AlignNavigatorToCenter();
            }
        }

        [Browsable(true)]
        [Description("Gets or sets max records for each page.")]
        [Category(CategoryBehavior)]
        public int MaxRecords
        {
            get => _maxRecords;
            set
            {
                if (value < 1)
                    throw new QuantityRangeException("Max records must be at least 1.");
                _maxRecords = value;
            }
        }

        [Browsable(true)]
        [Description("Gets or sets max records for each page.")]
        [Category(CategoryBehavior)]
        public bool AutoHideNavigator
        {
            get => _autoHideNavigator;
            set
            {
                _autoHideNavigator = value;
                bindingNavigator.Visible = !(_autoHideNavigator && IsNotPaging);
            }
        }

        [Browsable(false)]
        [Description("Gets current page.")]
        public int CurrentPage => _currentPageOffset / _maxRecords + 1;

        [Browsable(false)]
        [Description("Gets total pages.")]
        public int TotalPages => (_numberOfRecords - 1) / _maxRecords + 1;

        [Browsable(false)]
        [Description("Gets or sets data source for DataGridViewPaging.")]
        public object DataSource
        {
            get => bindingSource.DataSource;
            set
            {
                _lastDataSource?.Dispose();
                if (value is IDisposable disposable)
                    _lastDataSource = disposable;
                else
                    _lastDataSource = null;
                if (InvokeRequired)
                    BeginInvoke((MethodInvoker) delegate { bindingSource.DataSource = value; });
                else
                    bindingSource.DataSource = value;
            }
        }

        private bool IsNotPaging => TotalPages == 1 || !HasRows;

        private bool HasRows => _numberOfRecords > 0;

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

        #endregion Properties

        #region Methods

        private void AlignNavigatorToCenter()
        {
            var totalItemsWidth = 0;
            var items = bindingNavigator.Items;
            for (var i = 0; i < items.Count; i++)
                totalItemsWidth += items[i].Width;

            var containerWidth = bindingNavigator.Width;
            var marginLeft = (containerWidth - totalItemsWidth) / 2;

            var firstItem = items[0];
            var firstItemMargin = firstItem.Margin;
            firstItemMargin.Left = marginLeft;
            firstItem.Margin = firstItemMargin;
        }

        private void UpdateNavigator()
        {
            if (IsNotPaging)
            {
                if (_autoHideNavigator && ReadOnly)
                    bindingNavigator.Visible = false;
            }
            else
            {
                var totalPages = TotalPages;
                var currentPage = CurrentPage;

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
        ///     Set number of records for <see cref="DataGridViewPaging" />.
        /// </summary>
        /// <param name="numberOfRecords">
        ///     Number of records, you could use some statements like
        ///     <code>
        ///         SELECT COUNT(*) FROM my_table
        ///     </code>
        /// </param>
        public void Initialize(int numberOfRecords)
        {
            if (numberOfRecords < 0)
                throw new QuantityRangeException("Number of records must >= 0.");
            _numberOfRecords = numberOfRecords;
            _currentPageOffset = 0;
            _lastDataSource?.Dispose();
            _lastDataSource = null;
            bindingNavigator.Visible = !(!HasRows && AutoHideNavigator && ReadOnly);
            QueryData();
        }

        /// <summary>
        ///     Using auto mode, <see cref="DataGridViewPaging" /> will auto query
        ///     and display data to <see cref="System.Windows.Forms.DataGridView" />.
        /// </summary>
        /// <param name="connection">Database connection.</param>
        /// <param name="tableName"><see cref="DataGridViewPaging" /> will query from this table name.</param>
        public void UserHardMode(DbConnection connection, string tableName)
        {
            _autoModeHelper?.Dispose();
            _autoModeHelper = new HardModeHelper(this, connection, tableName);
            _autoModeHelper.Initialize();
        }

        /// <summary>
        ///     Using auto mode, <see cref="DataGridViewPaging" /> will auto query
        ///     and display data to <see cref="System.Windows.Forms.DataGridView" />.
        /// </summary>
        /// <param name="connection">Database connection.</param>
        /// <param name="selectCommandText"><see cref="DataGridViewPaging" /> .</param>
        public void UserSoftMode(DbConnection connection, string selectCommandText)
        {
            _autoModeHelper?.Dispose();
            _autoModeHelper = new SoftModeHelper(this, connection, selectCommandText);
            _autoModeHelper.Initialize();
        }

        private void QueryData()
        {
            RequestQueryData?.Invoke(this, new RequestQueryDataEventArgs(MaxRecords, _currentPageOffset));
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            AlignNavigatorToCenter();
        }

        #endregion Methods

        #region UserControl Event Handlers

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            AlignNavigatorToCenter();
        }

        private void DataGridViewPaging_Disposed(object sender, EventArgs e)
        {
            Disposed -= DataGridViewPaging_Disposed;
            _lastDataSource?.Dispose();
            _autoModeHelper?.Dispose();
        }

        #endregion UserControl Event Handlers

        #region BindingNavigator Event Handlers

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

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
        }

        private void bindingNavigatorPositionItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char) Keys.Enter) return;

            if (int.TryParse(bindingNavigatorPositionItem.Text, out var pageNumber))
            {
                if (pageNumber == CurrentPage) return;

                var totalPages = TotalPages;
                if (pageNumber < 1 || pageNumber > totalPages)
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

        #endregion BindingNavigator Event Handlers
    }
}