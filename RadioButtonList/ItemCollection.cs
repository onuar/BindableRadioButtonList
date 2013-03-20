using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceControls
{
    public class ItemCollection : IList<Item>
    {
        public delegate void ItemsChangedDelegate(object sender, EventArgs e);
        public event ItemsChangedDelegate ItemsChanged;

        public delegate void ItemsChangingDelegate(object sender, EventArgs e);
        public event ItemsChangingDelegate ItemsChanging;

        private readonly List<Item> _items = new List<Item>();

        public IEnumerator<Item> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Item item)
        {
            if (ItemsChanging != null)
            {
                ItemsChanging(this, null);
            }
            _items.Add(item);
            if (ItemsChanged != null)
            {
                ItemsChanged(this, null);
            }
        }

        public void Clear()
        {
            _items.Clear();
            if (ItemsChanged != null)
            {
                ItemsChanged(this, null);
            }
        }

        public bool Contains(Item item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(Item[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
            if (ItemsChanged != null)
            {
                ItemsChanged(this, null);
            }
        }

        public bool Remove(Item item)
        {
            var result = _items.Remove(item);
            if (ItemsChanged != null)
            {
                ItemsChanged(this, null);
            }
            return result;
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int IndexOf(Item item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, Item item)
        {
            _items.Insert(index, item);
            if (ItemsChanged != null)
            {
                ItemsChanged(this, null);
            }
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
            if (ItemsChanged != null)
            {
                ItemsChanged(this, null);
            }
        }

        public Item this[int index]
        {
            get { return _items[index]; }
            set
            {
                _items[index] = value;
                if (ItemsChanged != null)
                {
                    ItemsChanged(this, null);
                }
            }
        }

        public void AddAll(ItemCollection newItems)
        {
            _items.AddRange(newItems);
            if (ItemsChanged != null)
            {
                ItemsChanged(this, null);
            }
        }
    }
}