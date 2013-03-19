using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RadioButtonList.Tester;

namespace NiceControls.Tester
{
    public partial class FormTest : Form
    {
        public FormTest()
        {
            InitializeComponent();
        }

        private void Form1Load(object sender, EventArgs e)
        {
            FillRadioButtonList();
        }

        public TestModel CurrentTestModel
        {
            get { return rdlBindingSource.DataSource as TestModel; }
            set { rdlBindingSource.DataSource = value; }
        }

        private void FillRadioButtonList()
        {
            var source = new List<TestModel>
                             {
                                 new TestModel
                                     {
                                         Id = 666,
                                         TestText = "First Item Text",
                                         TestEnum = TestEnum.FirstItem
                                     },
                                 new TestModel
                                     {
                                         Id = 777,
                                         TestText = "Second Item Text",
                                         TestEnum = TestEnum.SecondItem
                                     }
                             };
            radioButtonList1.DataSource = source;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (CurrentTestModel == null)
            {
                return;
            }
            MessageBox.Show(CurrentTestModel.TestText);
        }
    }
}