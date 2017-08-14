#region Copyright (c) 2015-2017 Visyn
// The MIT License(MIT)
// 
// Copyright(c) 2015-2017 Visyn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Visyn.Collection
{

    public enum ECollectionChangeNotificationMode
    {
        /// <summary>
        /// Notifies that only a portion of data was changed and supplies the changed items (not supported by some elements,
        /// like CollectionView class).
        /// </summary>
        Add,

        /// <summary>
        /// Notifies that the entire collection was changed, does not supply the changed items (may be inneficient with large
        /// collections as requires the full update even if a small portion of items was added).
        /// </summary>
        Reset
    }

    public sealed class ObservableCollectionExtended<T> : ObservableCollection<T>
    {
        #region Constructors

        public ObservableCollectionExtended()
        {
        }   

        public ObservableCollectionExtended(List<T> list) : base(list)
        {
        }

        public ObservableCollectionExtended(IEnumerable<T> collection) : base(collection)
        {
        }

        #endregion

        public void ClearWithoutNotify()
        {
            Items.Clear();
        }

        /// <summary> 
        /// Adds the elements of the specified collection to the end of the ObservableCollection(Of T). 
        /// </summary> 
        /// <exception cref="ArgumentNullException"><paramref name="itemsToAdd"/> is <see langword="null" />.</exception>
        public void AddRange( IEnumerable<T> itemsToAdd, ECollectionChangeNotificationMode notificationMode = ECollectionChangeNotificationMode.Add)
        {
            if (itemsToAdd == null)
            {
                throw new ArgumentNullException(nameof(itemsToAdd));
            }
            CheckReentrancy();

            if (notificationMode == ECollectionChangeNotificationMode.Reset)
            {
                foreach (var i in itemsToAdd)
                {
                    Items.Add(i);
                }

                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
                OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

                return;
            }

            var startIndex = Count;
            var changedItems = itemsToAdd is List<T> ? (List<T>)itemsToAdd : new List<T>(itemsToAdd);
            foreach (var i in changedItems)
            {
                Items.Add(i);
            }

            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startIndex));
        }

    }
}