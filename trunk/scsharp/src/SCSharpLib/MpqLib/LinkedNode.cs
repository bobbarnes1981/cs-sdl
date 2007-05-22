#region LICENSE
//
// MpqHuffman.cs
//
// Authors:
//		Foole (fooleau@gmail.com)
//
// (C) 2006 Foole (fooleau@gmail.com)
// Based on code from StormLib by Ladislav Zezula and ShadowFlare
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion LICENSE

using System;
using System.IO;
using System.Collections;

namespace SCSharp.MpqLib
{
    // A node which is both hierachcical (parent/child) and doubly linked (next/prev)
    /// <summary>
    /// 
    /// </summary>
    public class LinkedNode
    {
        private int decompressedValue;

        /// <summary>
        /// 
        /// </summary>
        public int DecompressedValue
        {
            get { return decompressedValue; }
            set { decompressedValue = value; }
        }
        private int weight;

        /// <summary>
        /// 
        /// </summary>
        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        private LinkedNode parent;

        /// <summary>
        /// 
        /// </summary>
        public LinkedNode Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        private LinkedNode child0;

        /// <summary>
        /// 
        /// </summary>
        public LinkedNode Child0
        {
            get { return child0; }
            set { child0 = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public LinkedNode Child1
        { get { return child0.previous; } }

        private LinkedNode next;

        /// <summary>
        /// 
        /// </summary>
        public LinkedNode Next
        {
            get { return next; }
            set { next = value; }
        }

        private LinkedNode previous;

        /// <summary>
        /// 
        /// </summary>
        public LinkedNode Previous
        {
            get { return previous; }
            set { previous = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decompressedValue"></param>
        /// <param name="weight"></param>
        public LinkedNode(int decompressedValue, int weight)
        {
            this.decompressedValue = decompressedValue;
            this.weight = weight;
        }

        // TODO: This would be more efficient as a member of the other class
        // ie avoid the recursion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public LinkedNode Insert(LinkedNode other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            // 'Next' should have a lower weight
            // we should return the lower weight
            if (other.weight <= weight)
            {
                // insert before
                if (next != null)
                {
                    next.previous = other;
                    other.next = next;
                }
                next = other;
                other.previous = this;
                return other;
            }
            else
            {
                if (previous == null)
                {
                    // Insert after
                    other.previous = null;
                    previous = other;
                    other.next = this;
                }
                else
                {
                    previous.Insert(other);
                }
            }
            return this;
        }
    }
}
