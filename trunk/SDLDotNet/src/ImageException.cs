/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
 * Copyright (C) 2003 Klavs Martens
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Runtime.Serialization;

using SdlDotNet;

namespace SdlDotNet
{
	/// <summary>
	/// Exception class for SdlDotNet.Images
	/// </summary>
	[Serializable()]
	public class ImageException : SdlException 
	{
		/// <summary>
		/// 
		/// </summary>
		public ImageException() 
		{
		}
		/// <summary>
		/// Initializes an ImageException instance
		/// </summary>
		/// <param name="message">
		/// The string representing the error message
		/// </param>
		public ImageException(string message): base(message)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public ImageException(string message, Exception exception) 
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ImageException(SerializationInfo info, StreamingContext context) 
		{
		}
	}
}
