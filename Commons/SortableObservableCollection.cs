﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Quality_Control_EF.Commons
{
    public class SortableObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public SortableObservableCollection(ICollection<T> collection) : base(collection)
        {
            CollectionChanged += SortableObservableCollection_CollectionChanged;
        }

        public SortableObservableCollection()
        {
            CollectionChanged += SortableObservableCollection_CollectionChanged;
        }

        void SortableObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (object item in e.NewItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged += Item_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (object item in e.OldItems)
                {
                    (item as INotifyPropertyChanged).PropertyChanged -= Item_PropertyChanged;
                }
            }
        }

        void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var a = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(a);
        }

        public void Sort<TKey>(Func<T, TKey> keySelector, ListSortDirection direction)
        {
            switch (direction)
            {
                case ListSortDirection.Ascending:
                    {
                        ApplySort(Items.OrderBy(keySelector));
                        break;
                    }
                case ListSortDirection.Descending:
                    {
                        ApplySort(Items.OrderByDescending(keySelector));
                        break;
                    }

                default:
                    break;
            }
        }

        public void Sort<TKey>(Func<T, TKey> keySelector, IComparer<TKey> comparer)
        {
            ApplySort(Items.OrderBy(keySelector, comparer));
        }

        private void ApplySort(IEnumerable<T> sortedItems)
        {
            List<T> sortedItemsList = sortedItems.ToList();

            foreach (T item in sortedItemsList)
            {
                Move(IndexOf(item), sortedItemsList.IndexOf(item));
            }
        }
    }
}
