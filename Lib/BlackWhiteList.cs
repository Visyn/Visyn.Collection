#region Copyright (c) 2015-2018 Visyn
// The MIT License(MIT)
// 
// Copyright (c) 2015-2018 Visyn
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

using System.Collections.Generic;
using Visyn.Criteria;
using Visyn.JetBrains;

namespace Visyn.Collection
{
    public class BlackWhiteList<T> : ICriteria<T>
    {
        private BlackList<T> _blackList = new BlackList<T>();

        [NotNull]
        public BlackList<T> BlackList
        {
            get { return _blackList; }
            set
            {
                if(value == null) _blackList.Clear();
                else _blackList = value;
            }
        }

        [NotNull]
        private WhiteList<T> _whiteList = new WhiteList<T>();

        [NotNull]
        public WhiteList<T> WhiteList
        {
            get { return _whiteList; }
            set
            {
                if (value == null) _whiteList.Clear();
                else _whiteList = value;
            }
        }

        public bool MeetCriteria(T entity) 
            => _whiteList.And(_blackList).MeetCriteria(entity);

        public List<T> MeetCriteria(IEnumerable<T> collection) 
            => _whiteList.And(_blackList).MeetCriteria(collection);
    }
}