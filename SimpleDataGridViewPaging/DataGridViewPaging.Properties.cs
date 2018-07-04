using Code4Bugs.SimpleDataGridViewPaging.Exceptions;
using System.ComponentModel;
using System.Windows.Forms;

namespace Code4Bugs.SimpleDataGridViewPaging
{
    public partial class DataGridViewPaging
    {
        private const string CategoryControl = "Control";
        private const string CategoryBehavior = "Behavior";

        [Browsable(true)]
        [Description("The original DataGridView control.")]
        [Category(CategoryControl)]
        public DataGridView GridView => _gridView;

        [Browsable(true)]
        [Description("The original BindingNavigator control.")]
        [Category(CategoryControl)]
        public BindingNavigator Navigator => _navigator;

        [Browsable(true)]
        [Description("Gets or sets a value indicating whether DataGridView in the DataGridViewPaging is read-only.")]
        [Category(CategoryBehavior)]
        public bool ReadOnly
        {
            get => _readonly;
            set
            {
                _readonly = value;

                GridView.ReadOnly = _readonly;

                if (ReadOnly)
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
                    BeginInvoke((MethodInvoker)delegate { bindingSource.DataSource = value; });
                else
                    bindingSource.DataSource = value;
            }
        }
    }
}