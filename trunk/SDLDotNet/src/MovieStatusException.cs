/*
 * $RCSfile$
 * Copyright (C) 2004 David Hudson (jendave@yahoo.com)
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

namespace SdlDotNet 
{
	/// <summary>
	/// Represents an error resulting from a surface being lost, 
	/// usually as a result of the user changing the input focus 
	/// away from a full-screen application.
	/// </summary>
	[Serializable()]
	public class MovieStatusException : SdlException 
	{
		/// <summary>
		/// 
		/// </summary>
		public MovieStatusException() 
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public MovieStatusException(string message): base(message)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public MovieStatusException(string message, Exception exception) 
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected MovieStatusException(SerializationInfo info, StreamingContext context) 
		{
		}
	}
}
