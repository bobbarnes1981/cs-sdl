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
using System.Globalization;

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
        protected static Type GetTypeFromResourcePath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            string pathLower = path.ToLower(CultureInfo.CurrentCulture);
            //string ext = Path.GetExtension(path).ToLower();
            string ext = Path.GetExtension(path);
            if (String.Compare(ext, ".tbl", true, CultureInfo.CurrentCulture) == 0)
            {
                return typeof(Tbl);
            }
            else if (String.Compare(ext, ".fnt", true, CultureInfo.CurrentCulture) == 0)
            {
                return typeof(SCFont);
            }
            else if (String.Compare(ext, ".got", true, CultureInfo.CurrentCulture) == 0)
            {
                return typeof(Got);
            }
            else if (String.Compare(ext, ".grp", true, CultureInfo.CurrentCulture) == 0)
            {
                return typeof(Grp);
            }
            else if (String.Compare(ext, ".bin", true, CultureInfo.CurrentCulture) == 0)
            {
                if (pathLower.EndsWith("aiscript.bin")) /* must come before iscript.bin */
                {
                    return null;
                }
                else if (pathLower.EndsWith("iscript.bin"))
                {
                    return typeof(ScriptBin);
                }
                else
                {
                    return typeof(Bin);
                }
            }
            else if (String.Compare(ext, ".chk", true, CultureInfo.CurrentCulture) == 0)
            {
                return typeof(Chk);
            }
            else if (String.Compare(ext, ".dat", true, CultureInfo.CurrentCulture) == 0)
            {
                if (pathLower.EndsWith("flingy.dat"))
                {
                    return typeof(FlingyDat);
                }
                else if (pathLower.EndsWith("images.dat"))
                {
                    return typeof(ImagesDat);
                }
                else if (pathLower.EndsWith("sfxdata.dat"))
                {
                    return typeof(SfxDataDat);
                }
                else if (pathLower.EndsWith("sprites.dat"))
                {
                    return typeof(SpritesDat);
                }
                else if (pathLower.EndsWith("units.dat"))
                {
                    return typeof(UnitsDat);
                }
                else if (pathLower.EndsWith("mapdata.dat"))
                {
                    return typeof(MapDataDat);
                }
            }
            else if (String.Compare(ext, ".spk", true, CultureInfo.CurrentCulture) == 0)
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
            if (!path.ToLower(CultureInfo.CurrentCulture).EndsWith(".smk"))
            {
                cachedResources[path] = res;
            }

            return res;
        }
    }
}
