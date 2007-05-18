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
    /// 
    /// </summary>
    public abstract class Mpq
    {
        Dictionary<string, object> cachedResources;

        /// <summary>
        /// 
        /// </summary>
        protected Mpq()
        {
            cachedResources = new Dictionary<string, object>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract Stream GetStreamForResource(string path);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected Type GetTypeFromResourcePath(string path)
        {
            string ext = Path.GetExtension(path);
            if (ext.ToLower() == ".tbl")
            {
                return typeof(Tbl);
            }
            else if (ext.ToLower() == ".fnt")
            {
                return typeof(Fnt);
            }
            else if (ext.ToLower() == ".got")
            {
                return typeof(Got);
            }
            else if (ext.ToLower() == ".grp")
            {
                return typeof(Grp);
            }
            else if (ext.ToLower() == ".bin")
            {
                if (path.ToLower().EndsWith("aiscript.bin")) /* must come before iscript.bin */
                {
                    return null;
                }
                else if (path.ToLower().EndsWith("iscript.bin"))
                {
                    return typeof(ScriptBin);
                }
                else
                {
                    return typeof(Bin);
                }
            }
            else if (ext.ToLower() == ".chk")
            {
                return typeof(Chk);
            }
            else if (ext.ToLower() == ".dat")
            {
                if (path.ToLower().EndsWith("flingy.dat"))
                {
                    return typeof(FlingyDat);
                }
                else if (path.ToLower().EndsWith("images.dat"))
                {
                    return typeof(ImagesDat);
                }
                else if (path.ToLower().EndsWith("sfxdata.dat"))
                {
                    return typeof(SfxDataDat);
                }
                else if (path.ToLower().EndsWith("sprites.dat"))
                {
                    return typeof(SpritesDat);
                }
                else if (path.ToLower().EndsWith("units.dat"))
                {
                    return typeof(UnitsDat);
                }
                else if (path.ToLower().EndsWith("mapdata.dat"))
                {
                    return typeof(MapDataDat);
                }
            }
            else if (ext.ToLower() == ".spk")
            {
                return typeof(Spk);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public object GetResource(string path)
        {
            if (cachedResources.ContainsKey(path))
            {
                return cachedResources[path];
            }

            Stream stream = GetStreamForResource(path);
            if (stream == null)
            {
                return null;
            }

            Type t = GetTypeFromResourcePath(path);
            if (t == null)
            {
                return stream;
            }

            IMpqResource res = Activator.CreateInstance(t) as IMpqResource;

            if (res == null)
            {
                return null;
            }

            res.ReadFromStream(stream);

            /* don't cache .smk files */
            if (!path.ToLower().EndsWith(".smk"))
            {
                cachedResources[path] = res;
            }

            return res;
        }
    }
}
