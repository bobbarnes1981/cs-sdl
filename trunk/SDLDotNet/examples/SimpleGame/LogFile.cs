// Copyright 2005 David Hudson (jendave@yahoo.com)
// This file is part of Iron Crown.
//
// Iron Crown is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// Iron Crown is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with Iron Crown; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.IO;

namespace SdlDotNet.Examples
{
	/// <summary>
	/// Summary description for LogFile.
	/// </summary>
	public sealed class LogFile
	{
		static FileStream fs;
		
		static readonly LogFile instance = new LogFile();

		LogFile()
		{
			RotateLogs();
		}

		public static void WriteLine(string format)
		{
			
			StreamWriter streamWriter = new StreamWriter(fs);
			streamWriter.BaseStream.Seek(0, SeekOrigin.End); 
			streamWriter.WriteLine(DateTime.Now.ToString() + " " + format);
			streamWriter.Flush(); 
		}

		public static void WriteLine(string format, Object arg0)
		{
			
			StreamWriter streamWriter = new StreamWriter(fs);
			streamWriter.BaseStream.Seek(0, SeekOrigin.End); 
			streamWriter.WriteLine(DateTime.Now.ToString() + " " + format, arg0);
			streamWriter.Flush(); 
		}

		public static void WriteLine(string format, Object arg0, Object arg1)
		{
			
			StreamWriter streamWriter = new StreamWriter(fs);
			streamWriter.BaseStream.Seek(0, SeekOrigin.End); 
			streamWriter.WriteLine(DateTime.Now.ToString() + " " + format, arg0, arg1);
			streamWriter.Flush(); 
		}

		public static void WriteLine(string format, Object arg0, Object arg1, Object arg2)
		{
			
			StreamWriter streamWriter = new StreamWriter(fs);
			streamWriter.BaseStream.Seek(0, SeekOrigin.End); 
			streamWriter.WriteLine(DateTime.Now.ToString() + " " + format, arg0, arg1, arg2);
			streamWriter.Flush(); 
		}

		public static void Write(string format)
		{
			
			StreamWriter streamWriter = new StreamWriter(fs);
			// Write to the file using StreamWriter class 
			streamWriter.BaseStream.Seek(0, SeekOrigin.End); 
			streamWriter.Write(DateTime.Now.ToString() + " " + format);
			streamWriter.Flush(); 
		}

		public static void Write(string format, Object arg0)
		{
			
			StreamWriter streamWriter = new StreamWriter(fs);
			streamWriter.BaseStream.Seek(0, SeekOrigin.End); 
			streamWriter.Write(DateTime.Now.ToString() + " " + format, arg0);
			streamWriter.Flush(); 
		}

		public static void Write(string format, Object arg0, Object arg1)
		{
			
			StreamWriter streamWriter = new StreamWriter(fs);
			streamWriter.BaseStream.Seek(0, SeekOrigin.End); 
			streamWriter.Write(DateTime.Now.ToString() + " " + format, arg0, arg1);
			streamWriter.Flush(); 
		}

		public static void Write(string format, Object arg0, Object arg1, Object arg2)
		{
			
			StreamWriter streamWriter = new StreamWriter(fs);
			streamWriter.BaseStream.Seek(0, SeekOrigin.End); 
			streamWriter.Write(DateTime.Now.ToString() + " " + format, arg0, arg1, arg2);
			streamWriter.Flush(); 
		}

		public static void RotateLogs()
		{
			LogFile.fs = null;
			if (File.Exists(Names.LogFile3))
			{
				File.Delete(Names.LogFile3);
			}
			if (File.Exists(Names.LogFile2) & !File.Exists(Names.LogFile3))
			{
				File.Move(Names.LogFile2, Names.LogFile3);
			}
			if (File.Exists(Names.LogFile) & !File.Exists(Names.LogFile2))
			{
				File.Move(Names.LogFile, Names.LogFile2);
			}
			LogFile.fs = new FileStream(Names.LogFile, FileMode.OpenOrCreate, FileAccess.Write); 
		}
	}
}
