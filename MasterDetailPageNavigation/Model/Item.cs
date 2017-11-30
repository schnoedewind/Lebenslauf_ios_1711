using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lebenslauf
{
    public class Item
    {
        public string ItemName { get; set; }
        public string ItemPhoto { get; set; }
    }

    public class ItemList : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Item> _items;

        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set { _items = value; OnPropertyChanged("Items"); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public ItemList(List<Item> itemList)
        {
            Items = new ObservableCollection<Item>();
            foreach (Item itm in itemList)
            {
                Items.Add(itm);
            }
        }
    }
}
