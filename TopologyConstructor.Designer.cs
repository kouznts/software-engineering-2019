namespace GasStationMs.App
{
    partial class TopologyConstructor
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.fillingStationField = new System.Windows.Forms.DataGridView();
            this.cellsHorizontally = new System.Windows.Forms.NumericUpDown();
            this.cellsVertically = new System.Windows.Forms.NumericUpDown();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonTest = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fillingStationField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellsHorizontally)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellsVertically)).BeginInit();
            this.SuspendLayout();
            // 
            // fillingStationField
            // 
            this.fillingStationField.AllowUserToResizeColumns = false;
            this.fillingStationField.AllowUserToResizeRows = false;
            this.fillingStationField.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.fillingStationField.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.fillingStationField.ColumnHeadersHeight = 29;
            this.fillingStationField.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.fillingStationField.ColumnHeadersVisible = false;
            this.fillingStationField.Location = new System.Drawing.Point(28, 66);
            this.fillingStationField.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fillingStationField.Name = "fillingStationField";
            this.fillingStationField.RowHeadersWidth = 51;
            this.fillingStationField.Size = new System.Drawing.Size(719, 437);
            this.fillingStationField.TabIndex = 0;
            // 
            // cellsHorizontally
            // 
            this.cellsHorizontally.Location = new System.Drawing.Point(273, 612);
            this.cellsHorizontally.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cellsHorizontally.Name = "cellsHorizontally";
            this.cellsHorizontally.Size = new System.Drawing.Size(160, 22);
            this.cellsHorizontally.TabIndex = 1;
            this.cellsHorizontally.ValueChanged += new System.EventHandler(this.cellsHorizontally_ValueChanged);
            // 
            // cellsVertically
            // 
            this.cellsVertically.Location = new System.Drawing.Point(461, 612);
            this.cellsVertically.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cellsVertically.Name = "cellsVertically";
            this.cellsVertically.Size = new System.Drawing.Size(160, 22);
            this.cellsVertically.TabIndex = 2;
            this.cellsVertically.ValueChanged += new System.EventHandler(this.cellsVertically_ValueChanged);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(813, 108);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(159, 116);
            this.listBox1.TabIndex = 3;
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(813, 307);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 4;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.ButtonTest_ClickAsync);
            // 
            // TopologyConstructor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 690);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.cellsVertically);
            this.Controls.Add(this.cellsHorizontally);
            this.Controls.Add(this.fillingStationField);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(1061, 728);
            this.Name = "TopologyConstructor";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.fillingStationField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellsHorizontally)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cellsVertically)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView fillingStationField;
        private System.Windows.Forms.NumericUpDown cellsHorizontally;
        private System.Windows.Forms.NumericUpDown cellsVertically;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonTest;
    }
}

