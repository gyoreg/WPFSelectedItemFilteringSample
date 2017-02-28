using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;

using WpfApplication1.Annotations;

namespace WpfApplication1
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            MyList = new ObservableCollection<Item>();
            MyList.CollectionChanged += MyList_CollectionChanged;

            // add test data
            MyList.Add("Item1");
            MyList.Add("Item2");
            MyList.Add("Item3");
            MyList.Add("Item4");

            SelectedItems = new CollectionViewSource { Source = MyList }.View;
            SelectedItems.Filter = o => ((Item)o).IsSelected;
        }

        private void MyList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null) foreach (var item in e.OldItems) ((Item)item).PropertyChanged -= ItemChanged;
            if (e.NewItems != null) foreach (var item in e.NewItems) ((Item)item).PropertyChanged += ItemChanged;
        }

        private void ItemChanged(object sender, PropertyChangedEventArgs e)
        {
            SelectedItems.Refresh();
        }

        private bool FilterItem(object o)
        {
            var item = (Item)o;
            return item.IsSelected;
        }

        public ObservableCollection<Item> MyList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICollectionView AllItems { get; }
        public ICollectionView SelectedItems { get; }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}