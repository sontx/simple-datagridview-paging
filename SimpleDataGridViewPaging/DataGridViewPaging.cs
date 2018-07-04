using Code4Bugs.SimpleDataGridViewPaging.Exceptions;
using System;
using System.Data;
using System.Windows.Forms;

namespace Code4Bugs.SimpleDataGridViewPaging
{
    /// <inheritdoc/>
    /// <summary>
    /// The <see cref="T:System.Windows.Forms.UserControl" /> contains a <see cref="T:System.Windows.Forms.DataGridView" />
    /// and a <see cref="T:System.Windows.Forms.BindingNavigator" /> and it's capable of paging automatically.
    /// </summary>
    public partial class DataGridViewPaging : UserControl
    {
        private const int MagicNumber = 10;

        private ReadOnlyMode _readonly;
        private bool _computedReadOnly;
        private int _maxRecords = 100;
        private bool _autoHideNavigator;

        private int _numberOfRecords;
        private int _currentPageOffset;
        private IDbRequestHandler _dbRequestHandler;

        private bool IsNotPaging => TotalPages == 1 || !HasRows;

        private bool HasRows => _numberOfRecords > 0;

        private bool ComputedReadOnly
        {
            get => _computedReadOnly;
            set
            {
                _computedReadOnly = value;

                if (_computedReadOnly)
                {
                    bindingNavigatorAddNewItem.Visible = false;
                    bindingNavigatorDeleteItem.Visible = false;
                    bindingNavigatorSeparator2.Visible = false;
                }
                else
                {
                    bindingNavigatorAddNewItem.Enabled = true;
                    bindingNavigatorDeleteItem.Enabled = true;
                    bindingNavigatorSeparator2.Enabled = true;
                }

                PerformLayout();
            }
        }

        public IDbRequestHandler DbRequestHandler
        {
            get => _dbRequestHandler;
            set
            {
                _dbRequestHandler = value;
                if (_dbRequestHandler != null)
                {
                    var numberOfRecords = _dbRequestHandler.NumberOfRecords;
                    Initialize(numberOfRecords);
                }
            }
        }

        private void Initialize(int numberOfRecords)
        {
            if (numberOfRecords < 0)
                throw new QuantityRangeException("Number of records must >= 0.");
            _numberOfRecords = numberOfRecords;
            _currentPageOffset = 0;
            DataSource = null;
            QueryData();
        }

        public DataGridViewPaging()
        {
            InitializeComponent();
            GridView.AutoGenerateColumns = true;
            Disposed += DataGridViewPaging_Disposed;
        }

        private void QueryData()
        {
            if (DbRequestHandler != null)
                DataSource = DbRequestHandler.DataSource(MaxRecords, _currentPageOffset);
        }

        private void UpdateNavigatorButtons()
        {
            if (IsNotPaging)
            {
                if (_autoHideNavigator && _computedReadOnly)
                    _navigator.Visible = false;
            }
            else
            {
                var totalPages = TotalPages;
                var currentPage = CurrentPage;

                bindingNavigatorCountItem.Text = string.Format(_navigator.CountItemFormat, totalPages);
                bindingNavigatorPositionItem.Text = currentPage.ToString();
                bindingNavigatorPositionItem.Enabled = totalPages > 1;

                if (currentPage == 1)
                {
                    EnableBack(false);
                    EnableNext(true);
                }
                else if (currentPage == totalPages)
                {
                    EnableBack(true);
                    EnableNext(false);
                }
                else
                {
                    EnableBack(true);
                    EnableNext(true);
                }

                _navigator.Visible = true;
            }
        }

        private void EnableNext(bool enabled)
        {
            bindingNavigatorMoveLastItem.Enabled = enabled;
            bindingNavigatorMoveNextItem.Enabled = enabled;
        }

        private void EnableBack(bool enabled)
        {
            bindingNavigatorMoveFirstItem.Enabled = enabled;
            bindingNavigatorMovePreviousItem.Enabled = enabled;
        }

        private void ComputeReadOnlyIfNecessary()
        {
            if (DataSource != null && ReadOnly == ReadOnlyMode.Default)
            {
                ComputedReadOnly = !(DataSource is DataTable);
            }
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);
            UpdateNavigatorHorizontal();
        }

        private void UpdateNavigatorHorizontal()
        {
            switch (_navigatorHorizontal)
            {
                case NavigatorHorizontal.Default:
                    AlignNavigatorToCenter();
                    break;

                case NavigatorHorizontal.Left:
                    AlignNavigatorToLeft();
                    break;

                case NavigatorHorizontal.Center:
                    AlignNavigatorToCenter();
                    break;

                case NavigatorHorizontal.Right:
                    AlignNavigatorToRight();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AlignNavigatorToCenter()
        {
            var totalItemsWidth = 0;
            var items = _navigator.Items;
            for (var i = 0; i < items.Count; i++)
                totalItemsWidth += items[i].Visible ? items[i].Width : 0;

            var containerWidth = _navigator.Width;
            var marginLeft = (containerWidth - totalItemsWidth) / 2;

            var firstItem = items[0];
            var firstItemMargin = firstItem.Margin;
            firstItemMargin.Left = marginLeft;
            firstItem.Margin = firstItemMargin;
        }

        private void AlignNavigatorToLeft()
        {
            var items = _navigator.Items;
            var firstItem = items[0];
            var firstItemMargin = firstItem.Margin;
            firstItemMargin.Left = 0;
            firstItem.Margin = firstItemMargin;
        }

        private void AlignNavigatorToRight()
        {
            var totalItemsWidth = 0;
            var items = _navigator.Items;
            for (var i = 0; i < items.Count; i++)
                totalItemsWidth += items[i].Visible ? items[i].Width : 0;

            var containerWidth = _navigator.Width;
            var marginLeft = containerWidth - totalItemsWidth - MagicNumber;

            var firstItem = items[0];
            var firstItemMargin = firstItem.Margin;
            firstItemMargin.Left = marginLeft;
            firstItem.Margin = firstItemMargin;
        }
    }
}