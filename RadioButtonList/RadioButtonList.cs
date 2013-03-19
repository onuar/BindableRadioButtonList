using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;

namespace NiceControls
{
    public class RadioButtonList : UserControl
    {
        private ItemCollection _items;
        private int _leftMargin = 2;
        private int _topMargin = 2;
        private object _dataSource;
        private const string SelectedIndexEventName = "SelectedIndex";

        public RadioButtonList()
        {
            _items = new ItemCollection();
            _items.ItemsChanged += ItemsChanged;
        }

        public delegate void ItemSelectedIndexChangedDelegate(object sender, EventArgs e);
        public event ItemSelectedIndexChangedDelegate SelectedIndexChanged;

        public string DisplayMember { get; set; }

        public string ValueMember { get; set; }

        [DefaultValue(2)]
        public int LeftMargin
        {
            get { return _leftMargin; }
            set { _leftMargin = value; }
        }

        [DefaultValue(2)]
        public int TopMargin
        {
            get { return _topMargin; }
            set { _topMargin = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ItemCollection Items
        {
            get { return _items; }
            set { _items = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object DataSource
        {
            get { return _dataSource; }
            set
            {
                if (value != null && !(value is IList) && !(value is IListSource))
                {
                    throw new ArgumentException("BadDataSourceForComplexBinding");
                }

                _dataSource = value;
                UpdateItems(_dataSource);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedValue { get { return SelectedItem.Value; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Item SelectedItem { get { return _items[SelectedIndex]; } }

        private void ItemsChanged(object sender, EventArgs e)
        {
            UpdateRadioButtons();
        }

        private void UpdateRadioButtons()
        {
            Controls.Clear();

            int y = 0;

            foreach (var item in _items)
            {
                var newRadioButton = new RadioButton
                                         {
                                             Text = item.Text,
                                             Tag = item,
                                             Top = y,
                                             Left = LeftMargin,
                                             ForeColor = Color.Black
                                         };

                newRadioButton.CheckedChanged += RadioButtonCheckedChanged;

                Controls.Add(newRadioButton);

                y = y + newRadioButton.Height + TopMargin;
            }
        }

        private void UpdateItems(object dataSource)
        {
            var list = dataSource as IList;
            var newItems = new ItemCollection();
            if (list == null)
            {
                return;
            }
            foreach (var item in list)
            {
                string itemText = string.Empty;
                object itemValue = null;

                if (!string.IsNullOrEmpty(DisplayMember))
                {
                    var value = ReflectionHelper.GetPropertyValue(item, DisplayMember);
                    if (value != null)
                    {
                        itemText = value.ToString();
                    }
                }
                else
                {
                    itemText = item.ToString();
                }

                if (!string.IsNullOrEmpty(ValueMember))
                {
                    var value = ReflectionHelper.GetPropertyValue(item, ValueMember);
                    if (value != null)
                    {
                        itemValue = value;
                    }
                }
                else
                {
                    itemValue = item.ToString();
                }

                newItems.Add(new Item(itemText, itemValue));
            }

            Items.AddAll(newItems);
        }

        private void UpdateSelectedIndexBindings()
        {
            if (DataBindings.Count == 0)
            {
                return;
            }

            var selectedIndexDataBinding = DataBindings.Cast<Binding>().FirstOrDefault(binding => binding.PropertyName.Equals(SelectedIndexEventName));
            if (selectedIndexDataBinding == null)
            {
                return;
            }
            var bindingSource = selectedIndexDataBinding.DataSource as BindingSource;
            if (bindingSource == null)
            {
                return;
            }

            var @object = bindingSource.Current;
            if (@object == null)
            {
                var objectType = bindingSource.DataSource as TypeInfo;
                if (objectType != null)
                {
                    @object = Activator.CreateInstance(objectType);
                }
            }
            var value = SelectedValue;

            ReflectionHelper.SetPropertyValue(@object, selectedIndexDataBinding.BindingMemberInfo.BindingMember, value);
            bindingSource.DataSource = @object;
        }

        private void RadioButtonCheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;
            if (radioButton == null)
            {
                return;
            }
            if (radioButton.Checked)
            {
                var index = _items.IndexOf(radioButton.Tag as Item);
                SelectedIndex = index;

                SelectedIndexChanged(sender, e);
                UpdateSelectedIndexBindings();
            }
        }
    }
}
