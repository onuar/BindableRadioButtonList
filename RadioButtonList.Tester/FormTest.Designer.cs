using RadioButtonList.Tester;

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
            this.radioButtonList1 = new NiceControls.RadioButtonList();
            this.button1 = new System.Windows.Forms.Button();
            this.rdlBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.rdlBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonList1
            // 
            this.radioButtonList1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.radioButtonList1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedIndex", this.rdlBindingSource, "Id", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioButtonList1.DisplayMember = "TestText";
            this.radioButtonList1.Location = new System.Drawing.Point(37, 25);
            this.radioButtonList1.Name = "radioButtonList1";
            this.radioButtonList1.Size = new System.Drawing.Size(212, 201);
            this.radioButtonList1.TabIndex = 2;
            this.radioButtonList1.ValueMember = "TestEnum";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(131, 233);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(122, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "GetSelectedItemValue";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // rdlBindingSource
            // 
            this.rdlBindingSource.DataSource = typeof(TestModel);
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 271);
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




    }
}

