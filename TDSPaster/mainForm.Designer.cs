namespace TDSPaster
{
    partial class MainForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.PasteDataButton = new System.Windows.Forms.Button();
            this.SaveFileButton = new System.Windows.Forms.Button();
            this.SelectFileButton = new System.Windows.Forms.Button();
            this.previewListBox = new System.Windows.Forms.ListBox();
            this.elevationTextBox = new System.Windows.Forms.TextBox();
            this.SectionTextBox = new System.Windows.Forms.TextBox();
            this.inspectionYearTextBox = new System.Windows.Forms.TextBox();
            this.millTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.rowLeftLabel = new System.Windows.Forms.Label();
            this.rowCenterLabel = new System.Windows.Forms.Label();
            this.rowRightLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.sublimeCheckBox = new System.Windows.Forms.CheckBox();
            this.singleRowRadioButton = new System.Windows.Forms.RadioButton();
            this.tripleRowRadioButton = new System.Windows.Forms.RadioButton();
            this.colRightLabel = new System.Windows.Forms.Label();
            this.colCenterLabel = new System.Windows.Forms.Label();
            this.colLeftLabel = new System.Windows.Forms.Label();
            this.clearAllButton = new System.Windows.Forms.Button();
            this.commentGroupBox = new System.Windows.Forms.GroupBox();
            this.transposeCheckBox = new System.Windows.Forms.CheckBox();
            this.beamRadioButton = new System.Windows.Forms.RadioButton();
            this.datumRadioButton = new System.Windows.Forms.RadioButton();
            this.dartRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.commentGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(64, 323);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(818, 263);
            this.dataGridView1.TabIndex = 0;
            // 
            // PasteDataButton
            // 
            this.PasteDataButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PasteDataButton.Location = new System.Drawing.Point(286, 26);
            this.PasteDataButton.Name = "PasteDataButton";
            this.PasteDataButton.Size = new System.Drawing.Size(214, 156);
            this.PasteDataButton.TabIndex = 1;
            this.PasteDataButton.Text = "Paste Data";
            this.PasteDataButton.UseVisualStyleBackColor = true;
            this.PasteDataButton.Click += new System.EventHandler(this.PasteDataButton_Click);
            // 
            // SaveFileButton
            // 
            this.SaveFileButton.Location = new System.Drawing.Point(755, 46);
            this.SaveFileButton.Name = "SaveFileButton";
            this.SaveFileButton.Size = new System.Drawing.Size(75, 23);
            this.SaveFileButton.TabIndex = 2;
            this.SaveFileButton.Text = "Save File";
            this.SaveFileButton.UseVisualStyleBackColor = true;
            this.SaveFileButton.Click += new System.EventHandler(this.SaveFileButton_Click);
            // 
            // SelectFileButton
            // 
            this.SelectFileButton.Location = new System.Drawing.Point(523, 46);
            this.SelectFileButton.Name = "SelectFileButton";
            this.SelectFileButton.Size = new System.Drawing.Size(75, 23);
            this.SelectFileButton.TabIndex = 3;
            this.SelectFileButton.Text = "Select File";
            this.SelectFileButton.UseVisualStyleBackColor = true;
            this.SelectFileButton.Click += new System.EventHandler(this.SelectFileButton_Click);
            // 
            // previewListBox
            // 
            this.previewListBox.FormattingEnabled = true;
            this.previewListBox.Location = new System.Drawing.Point(13, 26);
            this.previewListBox.Name = "previewListBox";
            this.previewListBox.Size = new System.Drawing.Size(267, 173);
            this.previewListBox.TabIndex = 4;
            // 
            // elevationTextBox
            // 
            this.elevationTextBox.Location = new System.Drawing.Point(572, 136);
            this.elevationTextBox.Name = "elevationTextBox";
            this.elevationTextBox.Size = new System.Drawing.Size(310, 20);
            this.elevationTextBox.TabIndex = 5;
            // 
            // SectionTextBox
            // 
            this.SectionTextBox.Location = new System.Drawing.Point(572, 162);
            this.SectionTextBox.Name = "SectionTextBox";
            this.SectionTextBox.Size = new System.Drawing.Size(310, 20);
            this.SectionTextBox.TabIndex = 6;
            // 
            // inspectionYearTextBox
            // 
            this.inspectionYearTextBox.Location = new System.Drawing.Point(572, 188);
            this.inspectionYearTextBox.Name = "inspectionYearTextBox";
            this.inspectionYearTextBox.Size = new System.Drawing.Size(310, 20);
            this.inspectionYearTextBox.TabIndex = 7;
            // 
            // millTextbox
            // 
            this.millTextbox.Location = new System.Drawing.Point(572, 110);
            this.millTextbox.Name = "millTextbox";
            this.millTextbox.Size = new System.Drawing.Size(310, 20);
            this.millTextbox.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(541, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Mill:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(512, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Elevation:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(520, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Section:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(534, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Year:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(617, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(229, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "This is the elevation you are going to overwrite!";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(105, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Selected file preview";
            // 
            // rowLeftLabel
            // 
            this.rowLeftLabel.AutoSize = true;
            this.rowLeftLabel.Location = new System.Drawing.Point(33, 329);
            this.rowLeftLabel.Name = "rowLeftLabel";
            this.rowLeftLabel.Size = new System.Drawing.Size(25, 13);
            this.rowLeftLabel.TabIndex = 15;
            this.rowLeftLabel.Text = "Left";
            // 
            // rowCenterLabel
            // 
            this.rowCenterLabel.AutoSize = true;
            this.rowCenterLabel.Location = new System.Drawing.Point(20, 351);
            this.rowCenterLabel.Name = "rowCenterLabel";
            this.rowCenterLabel.Size = new System.Drawing.Size(38, 13);
            this.rowCenterLabel.TabIndex = 16;
            this.rowCenterLabel.Text = "Center";
            // 
            // rowRightLabel
            // 
            this.rowRightLabel.AutoSize = true;
            this.rowRightLabel.Location = new System.Drawing.Point(26, 376);
            this.rowRightLabel.Name = "rowRightLabel";
            this.rowRightLabel.Size = new System.Drawing.Size(32, 13);
            this.rowRightLabel.TabIndex = 17;
            this.rowRightLabel.Text = "Right";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(61, 238);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(184, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Select the format of your pasted data.";
            // 
            // sublimeCheckBox
            // 
            this.sublimeCheckBox.AutoSize = true;
            this.sublimeCheckBox.Location = new System.Drawing.Point(709, 23);
            this.sublimeCheckBox.Name = "sublimeCheckBox";
            this.sublimeCheckBox.Size = new System.Drawing.Size(173, 17);
            this.sublimeCheckBox.TabIndex = 19;
            this.sublimeCheckBox.Text = "Open file in sublime after save?";
            this.sublimeCheckBox.UseVisualStyleBackColor = true;
            // 
            // singleRowRadioButton
            // 
            this.singleRowRadioButton.AutoSize = true;
            this.singleRowRadioButton.Checked = true;
            this.singleRowRadioButton.Location = new System.Drawing.Point(64, 258);
            this.singleRowRadioButton.Name = "singleRowRadioButton";
            this.singleRowRadioButton.Size = new System.Drawing.Size(79, 17);
            this.singleRowRadioButton.TabIndex = 20;
            this.singleRowRadioButton.TabStop = true;
            this.singleRowRadioButton.Text = "Single Row";
            this.singleRowRadioButton.UseVisualStyleBackColor = true;
            this.singleRowRadioButton.CheckedChanged += new System.EventHandler(this.singleRowRadioButton_CheckedChanged);
            // 
            // tripleRowRadioButton
            // 
            this.tripleRowRadioButton.AutoSize = true;
            this.tripleRowRadioButton.Location = new System.Drawing.Point(147, 258);
            this.tripleRowRadioButton.Name = "tripleRowRadioButton";
            this.tripleRowRadioButton.Size = new System.Drawing.Size(76, 17);
            this.tripleRowRadioButton.TabIndex = 21;
            this.tripleRowRadioButton.Text = "Triple Row";
            this.tripleRowRadioButton.UseVisualStyleBackColor = true;
            this.tripleRowRadioButton.CheckedChanged += new System.EventHandler(this.tripleRowRadioButton_CheckedChanged);
            // 
            // colRightLabel
            // 
            this.colRightLabel.AutoSize = true;
            this.colRightLabel.Location = new System.Drawing.Point(329, 307);
            this.colRightLabel.Name = "colRightLabel";
            this.colRightLabel.Size = new System.Drawing.Size(32, 13);
            this.colRightLabel.TabIndex = 24;
            this.colRightLabel.Text = "Right";
            // 
            // colCenterLabel
            // 
            this.colCenterLabel.AutoSize = true;
            this.colCenterLabel.Location = new System.Drawing.Point(232, 307);
            this.colCenterLabel.Name = "colCenterLabel";
            this.colCenterLabel.Size = new System.Drawing.Size(38, 13);
            this.colCenterLabel.TabIndex = 23;
            this.colCenterLabel.Text = "Center";
            // 
            // colLeftLabel
            // 
            this.colLeftLabel.AutoSize = true;
            this.colLeftLabel.Location = new System.Drawing.Point(139, 307);
            this.colLeftLabel.Name = "colLeftLabel";
            this.colLeftLabel.Size = new System.Drawing.Size(25, 13);
            this.colLeftLabel.TabIndex = 22;
            this.colLeftLabel.Text = "Left";
            // 
            // clearAllButton
            // 
            this.clearAllButton.Location = new System.Drawing.Point(807, 643);
            this.clearAllButton.Name = "clearAllButton";
            this.clearAllButton.Size = new System.Drawing.Size(75, 23);
            this.clearAllButton.TabIndex = 25;
            this.clearAllButton.Text = "Clear All";
            this.clearAllButton.UseVisualStyleBackColor = true;
            this.clearAllButton.Click += new System.EventHandler(this.clearAllButton_Click);
            // 
            // commentGroupBox
            // 
            this.commentGroupBox.Controls.Add(this.dartRadioButton);
            this.commentGroupBox.Controls.Add(this.datumRadioButton);
            this.commentGroupBox.Controls.Add(this.beamRadioButton);
            this.commentGroupBox.Location = new System.Drawing.Point(332, 238);
            this.commentGroupBox.Name = "commentGroupBox";
            this.commentGroupBox.Size = new System.Drawing.Size(199, 51);
            this.commentGroupBox.TabIndex = 26;
            this.commentGroupBox.TabStop = false;
            this.commentGroupBox.Text = "     Transpose Comments";
            // 
            // transposeCheckBox
            // 
            this.transposeCheckBox.AutoSize = true;
            this.transposeCheckBox.Location = new System.Drawing.Point(338, 239);
            this.transposeCheckBox.Name = "transposeCheckBox";
            this.transposeCheckBox.Size = new System.Drawing.Size(15, 14);
            this.transposeCheckBox.TabIndex = 0;
            this.transposeCheckBox.UseVisualStyleBackColor = true;
            this.transposeCheckBox.CheckedChanged += new System.EventHandler(this.transposeCheckBox_CheckedChanged);
            // 
            // beamRadioButton
            // 
            this.beamRadioButton.AutoSize = true;
            this.beamRadioButton.Location = new System.Drawing.Point(6, 20);
            this.beamRadioButton.Name = "beamRadioButton";
            this.beamRadioButton.Size = new System.Drawing.Size(55, 17);
            this.beamRadioButton.TabIndex = 1;
            this.beamRadioButton.TabStop = true;
            this.beamRadioButton.Text = "BEAM";
            this.beamRadioButton.UseVisualStyleBackColor = true;
            // 
            // datumRadioButton
            // 
            this.datumRadioButton.AutoSize = true;
            this.datumRadioButton.Location = new System.Drawing.Point(67, 20);
            this.datumRadioButton.Name = "datumRadioButton";
            this.datumRadioButton.Size = new System.Drawing.Size(64, 17);
            this.datumRadioButton.TabIndex = 2;
            this.datumRadioButton.TabStop = true;
            this.datumRadioButton.Text = "DATUM";
            this.datumRadioButton.UseVisualStyleBackColor = true;
            // 
            // dartRadioButton
            // 
            this.dartRadioButton.AutoSize = true;
            this.dartRadioButton.Location = new System.Drawing.Point(137, 20);
            this.dartRadioButton.Name = "dartRadioButton";
            this.dartRadioButton.Size = new System.Drawing.Size(55, 17);
            this.dartRadioButton.TabIndex = 3;
            this.dartRadioButton.TabStop = true;
            this.dartRadioButton.Text = "DART";
            this.dartRadioButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 678);
            this.Controls.Add(this.transposeCheckBox);
            this.Controls.Add(this.commentGroupBox);
            this.Controls.Add(this.clearAllButton);
            this.Controls.Add(this.colRightLabel);
            this.Controls.Add(this.colCenterLabel);
            this.Controls.Add(this.colLeftLabel);
            this.Controls.Add(this.tripleRowRadioButton);
            this.Controls.Add(this.singleRowRadioButton);
            this.Controls.Add(this.sublimeCheckBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.rowRightLabel);
            this.Controls.Add(this.rowCenterLabel);
            this.Controls.Add(this.rowLeftLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.millTextbox);
            this.Controls.Add(this.inspectionYearTextBox);
            this.Controls.Add(this.SectionTextBox);
            this.Controls.Add(this.elevationTextBox);
            this.Controls.Add(this.previewListBox);
            this.Controls.Add(this.SelectFileButton);
            this.Controls.Add(this.SaveFileButton);
            this.Controls.Add(this.PasteDataButton);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.commentGroupBox.ResumeLayout(false);
            this.commentGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button PasteDataButton;
        private System.Windows.Forms.Button SaveFileButton;
        private System.Windows.Forms.Button SelectFileButton;
        private System.Windows.Forms.ListBox previewListBox;
        private System.Windows.Forms.TextBox elevationTextBox;
        private System.Windows.Forms.TextBox SectionTextBox;
        private System.Windows.Forms.TextBox inspectionYearTextBox;
        private System.Windows.Forms.TextBox millTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label rowLeftLabel;
        private System.Windows.Forms.Label rowCenterLabel;
        private System.Windows.Forms.Label rowRightLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox sublimeCheckBox;
        private System.Windows.Forms.RadioButton singleRowRadioButton;
        private System.Windows.Forms.RadioButton tripleRowRadioButton;
        private System.Windows.Forms.Label colRightLabel;
        private System.Windows.Forms.Label colCenterLabel;
        private System.Windows.Forms.Label colLeftLabel;
        private System.Windows.Forms.Button clearAllButton;
        private System.Windows.Forms.GroupBox commentGroupBox;
        private System.Windows.Forms.CheckBox transposeCheckBox;
        private System.Windows.Forms.RadioButton dartRadioButton;
        private System.Windows.Forms.RadioButton datumRadioButton;
        private System.Windows.Forms.RadioButton beamRadioButton;
    }
}

