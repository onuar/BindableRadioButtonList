﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Linq;

namespace NiceControls
{
    public class RadioButtonList : UserControl
    {
        private readonly ItemCollection _items;
        private int _leftMargin = 2;
        private int _topMargin = 2;
        private object _dataSource;
        private ListItemOrientation _orientation = ListItemOrientation.Horizontal;
        private bool _dataSourceBinded = false;
        private const string SelectedIndexEventName = "SelectedIndex";

        public RadioButtonList()
        {
            _items = new ItemCollection();
            _items.ItemsChanged += ItemsCollectionChanged;
            _items.ItemsChanging += ItemsCollectionChanging;
            DataBindings.CollectionChanged += DataBindingsCollectionChanged;
        }

        public delegate void ItemSelectedIndexChangedDelegate(object sender, EventArgs e);
        public event ItemSelectedIndexChangedDelegate SelectedIndexChanged;

        [Browsable(true)]
        public string DisplayMember { get; set; }

        [Browsable(true)]
        public string ValueMember { get; set; }

        [Browsable(true)]
        [DefaultValue(2)]
        public int LeftMargin
        {
            get { return _leftMargin; }
            set { _leftMargin = value; }
        }

        [Browsable(true)]
        [DefaultValue(2)]
        public int TopMargin
        {
            get { return _topMargin; }
            set { _topMargin = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ItemCollection Items
        {
            get { return _items; }
        }

        [Browsable(false)]
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
                UpdateItemsFromDataSource(_dataSource);
                _dataSourceBinded = _dataSource != null;
            }
        }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public object SelectedValue { get { return SelectedItem.Value; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Item SelectedItem { get { return _items[SelectedIndex]; } }

        [DefaultValue(ListItemOrientation.Horizontal)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public ListItemOrientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; }
        }

        public RadioButton this[object value]
        {
            get
            { return (from radioButton in Controls.OfType<RadioButton>() let item = radioButton.Tag as Item where item != null where item.Value.Equals(value) select radioButton).FirstOrDefault(); }
        }

        private void DataBindingsCollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            var binding = e.Element as Binding;
            if (binding == null)
            {
                return;
            }
            if (!binding.PropertyName.Equals(SelectedIndexEventName))
            {
                return;
            }
            var bindingSource = binding.DataSource as BindingSource;
            if (bindingSource == null)
            {
                return;
            }
            bindingSource.DataSourceChanged += BindingSourceDataSourceChanged;
        }

        private void BindingSourceDataSourceChanged(object sender, EventArgs e)
        {
            RegisterNotifyPropertyChanged();
        }

        private void RegisterNotifyPropertyChanged()
        {
            var bindingSource = GetSelectedIndexBindingSource();
            if (bindingSource == null)
            {
                return;
            }

            var @object = bindingSource.Current;
            if (@object == null)
            {
                var objectType = bindingSource.DataSource as Type;
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

        private void NotifyValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var binding = GetSelectedIndexBinding();
            if (binding == null)
            {
                return;
            }
            var changedProperty = binding.BindingMemberInfo.BindingField;
            if (!string.IsNullOrEmpty(ValueMember) && e.PropertyName.Equals(changedProperty))
            {
                var bindingSource = binding.DataSource as BindingSource;
                if (bindingSource == null)
                {
                    return;
                }
                object @object = bindingSource.Current;
                var newValue = ReflectionHelper.GetPropertyValue(@object, changedProperty);
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
                    throw new InvalidCastException("RadioButtonTagItem");
                }
                radioButton.Checked = selectedValue.Equals(item.Value);
            }
        }

        public void ItemsCollectionChanging(object sender, EventArgs e)
        {
            if (_dataSourceBinded)
            {
                throw new ArgumentException("Items collection cannot be modified when the DataSource property is set.");
            }
        }

        private void ItemsCollectionChanged(object sender, EventArgs e)
        {
            UpdateRadioButtons();
        }

        private void UpdateItemsFromDataSource(object dataSource)
        {
            var list = dataSource as IList;
            _items.Clear();
            if (list == null)
            {
                return;
            }
            var newItems = new ItemCollection();

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
                                             ForeColor = Color.Black,
                                             AutoSize = true
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

            if (Controls.Count > 0)
            {
                var firstRadioButton = (Controls[0] as RadioButton);
                if (firstRadioButton != null)
                {
                    firstRadioButton.Checked = true;
                }
            }
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
                UpdateSelectedIndexBindingDataSource();
            }
        }

        private void UpdateSelectedIndexBindingDataSource()
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
            var bindingSource = GetSelectedIndexBindingSource();
            if (bindingSource == null)
            {
                return;
            }

            var @object = bindingSource.Current;
            if (@object == null)
            {
                var objectType = bindingSource.DataSource as Type;
                if (objectType != null)
                {
                    @object = ReflectionHelper.SecureCreateInstance(objectType);
                }
            }
            var value = SelectedValue;

            ReflectionHelper.SetPropertyValue(@object, selectedIndexBinding.BindingMemberInfo.BindingMember, value);
            bindingSource.DataSource = @object;
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

        private BindingSource GetSelectedIndexBindingSource()
        {
            var binding = GetSelectedIndexBinding();
            if (binding == null)
            {
                return null;
            }
            var bindingSource = binding.DataSource as BindingSource;
            return bindingSource;
        }
    }
}