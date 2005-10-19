/*
 * $RCSfile$
 * Copyright (C) 2003 Lucas Maloney
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

/*
 * $Revision$
 * $Date$
 *
 *	REVISION HISTORY
 *
 *	Mon 31 Mar 2003 23:28:02 EST LM
 *	Changed namespace from SdlTtfDotNet
 *
 *	Tue 25 Mar 2003 18:19:03 EST LM
 *	Basically removed all code due to Natives.Sdl_GetError being inaccessible.
 */

using System;
using System.Runtime.Serialization;

using SdlDotNet;

namespace SdlDotNet {
	/// <summary>
	/// Sdl Ttf Exception Class
	/// </summary>
	[Serializable()]
	public class TtfException : SdlException 
	{
		/// <summary>
		/// 
		/// </summary>
		public TtfException() 
		{
		}
		/// <summary>
		/// Initializes an TtfException instance
		/// </summary>
		/// <param name="message">
		/// The string representing the error message
		/// </param>
		public TtfException(string message): base(message)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public TtfException(string message, Exception exception) 
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected TtfException(SerializationInfo info, StreamingContext context) 
		{
		}
	}
}