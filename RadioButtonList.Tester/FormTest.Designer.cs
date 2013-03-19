namespace NiceControls.Tester
{
    partial class FormTest
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButtonList1 = new NiceControls.RadioButtonList();
            this.rdlBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnSecondItem = new System.Windows.Forms.Button();
            this.btnFirstItem = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.rdlBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(37, 233);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(216, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "GetSelectedItemValue";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // radioButtonList1
            // 
            this.radioButtonList1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.radioButtonList1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedIndex", this.rdlBindingSource, "TestEnum", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioButtonList1.DisplayMember = "TestText";
            this.radioButtonList1.Location = new System.Drawing.Point(37, 25);
            this.radioButtonList1.Name = "radioButtonList1";
            this.radioButtonList1.Orientation = NiceControls.ListItemOrientation.Vertical;
            this.radioButtonList1.Size = new System.Drawing.Size(212, 201);
            this.radioButtonList1.TabIndex = 2;
            this.radioButtonList1.ValueMember = "TestEnum";
            // 
            // rdlBindingSource
            // 
            this.rdlBindingSource.DataSource = typeof(NiceControls.Tester.TestModel);
            // 
            // btnSecondItem
            // 
            this.btnSecondItem.Location = new System.Drawing.Point(37, 291);
            this.btnSecondItem.Name = "btnSecondItem";
            this.btnSecondItem.Size = new System.Drawing.Size(212, 23);
            this.btnSecondItem.TabIndex = 4;
            this.btnSecondItem.Text = "Select Second Item";
            this.btnSecondItem.UseVisualStyleBackColor = true;
            this.btnSecondItem.Click += new System.EventHandler(this.BtnSecondItemClick);
            // 
            // btnFirstItem
            // 
            this.btnFirstItem.Location = new System.Drawing.Point(37, 262);
            this.btnFirstItem.Name = "btnFirstItem";
            this.btnFirstItem.Size = new System.Drawing.Size(212, 23);
            this.btnFirstItem.TabIndex = 4;
            this.btnFirstItem.Text = "Select First Item";
            this.btnFirstItem.UseVisualStyleBackColor = true;
            this.btnFirstItem.Click += new System.EventHandler(this.BtnFirstItemClick);
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 327);
            this.Controls.Add(this.btnFirstItem);
            this.Controls.Add(this.btnSecondItem);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radioButtonList1);
            this.Name = "FormTest";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1Load);
            ((System.ComponentModel.ISupportInitialize)(this.rdlBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource rdlBindingSource;
        private RadioButtonList radioButtonList1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnSecondItem;
        private System.Windows.Forms.Button btnFirstItem;




    }
}

