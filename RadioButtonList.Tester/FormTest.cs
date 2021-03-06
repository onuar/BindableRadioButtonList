﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

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
            radioButtonList1.DataSource = GetDataSource();

            radioButtonList2.Items.Add(new Item("NewItem1", 1));
            radioButtonList2.Items.Add(new Item("NewItem2", 2));
            radioButtonList2.Items.Add(new Item("NewItem3", 3));
            radioButtonList2.Items.Add(new Item("NewItem4", 4));

            radioButtonList3.DataSource = Enum.GetValues(typeof(TestEnum));
        }

        public TestModel CurrentTestModel
        {
            get { return rdlBindingSource.DataSource as TestModel; }
            set
            { rdlBindingSource.DataSource = value; }
        }

        private object GetDataSource()
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
            return source;
        }

        private void Button1Click(object sender, EventArgs e)
        {
            if (CurrentTestModel == null)
            {
                return;
            }
            MessageBox.Show(CurrentTestModel.TestEnum.ToString());
        }

        private void BtnFirstItemClick(object sender, EventArgs e)
        {
            CurrentTestModel = new TestModel();
            CurrentTestModel.TestEnum = TestEnum.FirstItem;
        }

        private void BtnSecondItemClick(object sender, EventArgs e)
        {
            CurrentTestModel = new TestModel();
            CurrentTestModel.TestEnum = TestEnum.SecondItem;
        }

        private void Button2Click(object sender, EventArgs e)
        {
            if (CurrentTestModel == null)
            {
                return;
            }
            MessageBox.Show(CurrentTestModel.Id.ToString());
        }

        private void Button3Click(object sender, EventArgs e)
        {
            var radioButton = radioButtonList1[CurrentTestModel.TestEnum];
            if (radioButton == null)
            {
                throw new Exception("Radiobutton not found");
            }
            radioButton.Enabled = !radioButton.Enabled;
        }
    }
}