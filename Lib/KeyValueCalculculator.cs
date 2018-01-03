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

namespace Visyn.Collection
{
    public abstract class KeyValueCalculculator<TInput,TKey,TValue>
    {
        /// <summary>
        /// Gets the key associated with last TInput set using UpdateInput
        /// </summary>
        /// <value>The key.</value>
        public abstract TKey Key { get; }
        /// <summary>
        /// Gets the Value associated with last TInput set using UpdateInput
        /// For performance reasons, value should only be calculated on demand and cached (and cleared when UpdateInput is called)
        /// </summary>
        /// <value>The value.</value>
        public abstract TValue Value { get; }

        /// <summary>
        /// Gets the last input value set by UpdateInput
        /// </summary>
        /// <value>The input.</value>
        public TInput Input { get; protected set; }

        /// <summary>
        /// Updates the input for both the Key and Value calculations
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>TKey.</returns>
        public abstract TKey UpdateInput(TInput input);

        public abstract TInput Calculate(TKey key);
    }
}