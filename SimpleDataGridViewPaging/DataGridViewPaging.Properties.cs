using System.ComponentModel;
using System.Windows.Forms;
using Code4Bugs.SimpleDataGridViewPaging.Exceptions;

namespace Code4Bugs.SimpleDataGridViewPaging
{
    public partial class DataGridViewPaging
    {
        private const string CategoryControl = "Control";
        private const string CategoryBehavior = "Behavior";
        private NavigatorHorizontal _navigatorHorizontal;

        [Browsable(true)]
        [Description("The original DataGridView control.")]
        [Category(CategoryControl)]
        public DataGridView GridView { get; private set; }

        [Browsable(true)]
        [Description("The original BindingNavigator control.")]
        [Category(CategoryControl)]
        public BindingNavigator Navigator => _navigator;

        [Browsable(true)]
        [DefaultValue(typeof(ReadOnlyMode), "Default")]
        [Description("Gets or sets a value indicating whether DataGridView in the DataGridViewPaging is read-only.")]
        [Category(CategoryBehavior)]
        public ReadOnlyMode ReadOnly
        {
            get => _readonly;
            set
            {
                _readonly = value;

                if (_readonly != ReadOnlyMode.Default)
                {
                    ComputedReadOnly = _readonly == ReadOnlyMode.True;
                    GridView.ReadOnly = ComputedReadOnly;
                }
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
        [Description("Gets or whether the navigator is hidden if necessary.")]
        [Category(CategoryBehavior)]
        public bool AutoHideNavigator
        {
            get => _autoHideNavigator;
            set
            {
                _autoHideNavigator = value;
                _navigator.Visible = !(_autoHideNavigator && IsNotPaging);
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
                if (InvokeRequired)
                {
                    BeginInvoke((MethodInvoker) delegate
                    {
                        bindingSource.DataSource = value;
                        ComputeReadOnlyIfNecessary();
                    });
                }
                else
                {
                    bindingSource.DataSource = value;
                    ComputeReadOnlyIfNecessary();
                }
            }
        }

        [Browsable(true)]
        [Description("Gets or sets horizontally of navigator.")]
        [Category(CategoryBehavior)]
        public NavigatorHorizontal NavigatorHorizontal
        {
            get => _navigatorHorizontal;
            set
            {
                _navigatorHorizontal = value;
                UpdateNavigatorHorizontal();
            }
        }
    }
}