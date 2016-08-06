using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SimpleDataGridViewPaging
{
    public partial class DataGridViewPaging : UserControl
    {
        private const string CATEGORY_CONTROL = "Control";
        private const string CATEGORY_BEHAVIOR = "Behavior";

        #region Variants

        private bool _readonly = false;

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
        
        #endregion

        public DataGridViewPaging()
        {
            InitializeComponent();
            SetCenterHorizontalAlignment();
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

        #endregion

        #region UserControl Event Handlers

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            SetCenterHorizontalAlignment();
        }

        #endregion

        #region DataGridView Event Handlers

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        #endregion

        #region BindingNavigator Event Handlers

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorPositionItem_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        #endregion
    }
}
