﻿using System;
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
        private ListItemOrientation _orientation = ListItemOrientation.Horizontal;
        private const string SelectedIndexEventName = "SelectedIndex";

        public RadioButtonList()
        {
            _items = new ItemCollection();
            _items.ItemsChanged += ItemsChanged;
            DataBindings.CollectionChanged += DataBindingsCollectionChanged;
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

        [DefaultValue(ListItemOrientation.Horizontal)]
        public ListItemOrientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        private void DataBindingsCollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            var binding = GetSelectedIndexBinding();
            var bindingSource = binding.DataSource as BindingSource;
            if (bindingSource == null)
            {
                return;
            }
            bindingSource.DataSourceChanged += BindingSourceDataSourceChanged;
        }

        void BindingSourceDataSourceChanged(object sender, EventArgs e)
        {
            RegisterNotifyPropertyChanged();
        }

        private void RegisterNotifyPropertyChanged()
        {
            var selectedIndexBinding = GetSelectedIndexBinding();
            if (selectedIndexBinding == null)
            {
                return;
            }
            var bindingSource = selectedIndexBinding.DataSource as BindingSource;
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

            var notifyPropertyChanged = (@object as INotifyPropertyChanged);
            if (notifyPropertyChanged != null)
            {
                notifyPropertyChanged.PropertyChanged += NotifyValuePropertyChanged;
            }
        }

        void NotifyValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ValueMember) && e.PropertyName.Equals(ValueMember))
            {
                var binding = GetSelectedIndexBinding();
                var bindingSource = binding.DataSource as BindingSource;
                if (bindingSource == null)
                {
                    return;
                }
                object @object = bindingSource.Current;
                var newValue = ReflectionHelper.GetPropertyValue(@object, ValueMember);
                SelectRadioButtonByValue(newValue);
            }
        }

        private void SelectRadioButtonByValue(object selectedValue)
        {
            foreach (RadioButton radioButton in Controls)
            {
                var item = radioButton.Tag as Item;
                if (item == null)
                {
                    throw new InvalidCastException("RadioButton");
                }
                radioButton.Checked = selectedValue.Equals(item.Value);
            }
        }

        private void ItemsChanged(object sender, EventArgs e)
        {
            UpdateRadioButtons();
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

        private void UpdateRadioButtons()
        {
            Controls.Clear();
            int coordinate = 0;

            foreach (var item in _items)
            {
                var newRadioButton = new RadioButton
                                         {
                                             Text = item.Text,
                                             Tag = item,
                                             ForeColor = Color.Black
                                         };
                switch (Orientation)
                {
                    case ListItemOrientation.Horizontal:
                        newRadioButton.Top = coordinate;
                        newRadioButton.Left = LeftMargin;

                        coordinate = coordinate + newRadioButton.Height + TopMargin;
                        break;
                    case ListItemOrientation.Vertical:
                        newRadioButton.Top = TopMargin;
                        newRadioButton.Left = coordinate;
                        coordinate = coordinate + newRadioButton.Width + LeftMargin;
                        break;
                }
                newRadioButton.CheckedChanged += RadioButtonCheckedChanged;

                Controls.Add(newRadioButton);
            }
        }

        private void UpdateSelectedIndexBinding()
        {
            if (DataBindings.Count == 0)
            {
                return;
            }

            var selectedIndexBinding = GetSelectedIndexBinding();
            if (selectedIndexBinding == null)
            {
                return;
            }
            var bindingSource = selectedIndexBinding.DataSource as BindingSource;
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

            ReflectionHelper.SetPropertyValue(@object, selectedIndexBinding.BindingMemberInfo.BindingMember, value);
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

                if (SelectedIndexChanged != null)
                {
                    SelectedIndexChanged(sender, e);
                }
                UpdateSelectedIndexBinding();
            }
        }

        private Binding GetSelectedIndexBinding()
        {
            var selectedIndexBinding = DataBindings.Cast<Binding>().FirstOrDefault(binding => binding.PropertyName.Equals(SelectedIndexEventName));
            if (selectedIndexBinding == null)
            {
                return null;
            }
            return selectedIndexBinding;
        }
    }
}