namespace Code4Bugs.SimpleDataGridViewPaging.Examples
{
    partial class AutoQueryWithEditableForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewPaging1 = new Code4Bugs.SimpleDataGridViewPaging.DataGridViewPaging();
            this.SuspendLayout();
            // 
            // dataGridViewPaging1
            // 
            this.dataGridViewPaging1.AutoHideNavigator = false;
            this.dataGridViewPaging1.DataSource = null;
            this.dataGridViewPaging1.DbRequestHandler = null;
            this.dataGridViewPaging1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPaging1.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewPaging1.Margin = new System.Windows.Forms.Padding(5);
            this.dataGridViewPaging1.MaxRecords = 100;
            this.dataGridViewPaging1.Name = "dataGridViewPaging1";
            this.dataGridViewPaging1.ReadOnly = false;
            this.dataGridViewPaging1.Size = new System.Drawing.Size(616, 460);
            this.dataGridViewPaging1.TabIndex = 0;
            // 
            // AutoQueryWithEditableForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 460);
            this.Controls.Add(this.dataGridViewPaging1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AutoQueryWithEditableForm";
            this.Text = "Auto Query With Editable";
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridViewPaging dataGridViewPaging1;
    }
}

