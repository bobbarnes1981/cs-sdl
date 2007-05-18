#region LICENSE
//
// Authors:
//	Chris Toshok (toshok@hungry.com)
//
// (C) 2006 The Hungry Programmers (http://www.hungry.com/)
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
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

namespace SCSharp.MpqLib
{
    /// <summary>
    /// 
    /// </summary>
    public class MpqDirectory : Mpq
    {
        Dictionary<string, string> fileHash;
        string mpqDirPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public MpqDirectory(string path)
        {
            mpqDirPath = path;
            fileHash = new Dictionary<string, string>();

            RecurseDirectoryTree(mpqDirPath);
        }

        string ConvertBackSlashes(string path)
        {
            while (path.IndexOf('\\') != -1)
                path = path.Replace('\\', Path.DirectorySeparatorChar);

            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override Stream GetStreamForResource(string path)
        {
            string rebased_path = ConvertBackSlashes(Path.Combine(mpqDirPath, path));

            if (fileHash.ContainsKey(rebased_path.ToLower()))
            {
                string real_path = fileHash[rebased_path.ToLower()];
                if (real_path != null)
                {
                    Console.WriteLine("using {0}", real_path);
                    return File.OpenRead(real_path);
                }
            }
            return null;
        }

        void RecurseDirectoryTree(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string f in files)
            {
                string platform_path = ConvertBackSlashes(f);
                fileHash.Add(f.ToLower(), platform_path);
            }

            string[] directories = Directory.GetDirectories(path);
            foreach (string d in directories)
            {
                RecurseDirectoryTree(d);
            }
        }
    }
}
