#region LICENSE
//
// Authors:
//	David Hudson (jendave@yahoo.com)
//
// (C) 2007 David Hudson
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
using System.Runtime.Serialization;

namespace SCSharp.UI
{
    /// <summary>
    /// Represents a run-time error from the SCSharp library.
    /// </summary>
    [Serializable()]
    public class SCException : Exception
    {
        #region Constructors

        /// <summary>
        /// Returns basic exception
        /// </summary>
        public SCException() : base()
        {
        }
        /// <summary>
        /// Initializes an SdlException instance
        /// </summary>
        /// <param name="message">
        /// The string representing the error message
        /// </param>
        public SCException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Returns exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="exception">Exception type</param>
        public SCException(string message, Exception exception)
            : base(message, exception)
        {
        }

        /// <summary>
        /// Returns SerializationInfo
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected SCException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
