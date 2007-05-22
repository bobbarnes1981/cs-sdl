#region LICENSE
//
// Authors:
// Chris Toshok (toshok@hungry.com)
//
// (C) 2006 The Hungry Programmers (http://www.hungry.com/)
//

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
using System.Threading;
using System.Collections.Generic;

using SdlDotNet;
using System.Drawing;
using SCSharp;
using SCSharp.MpqLib;

namespace SCSharp.UI
{
    /// <summary>
    ///
    /// </summary>
    public class Unit
    {
        int unitId;
        UnitsDat units;

        uint hitpoints;
        uint shields;

        int x;
        int y;

        Sprite sprite;

        /// <summary>
        ///
        /// </summary>
        /// <param name="unitId"></param>
        public Unit(int unitId)
        {
            this.unitId = unitId;
            units = GlobalResources.Instance.UnitsDat;

            hitpoints = units.GetHitPoints(unitId);
            shields = units.GetShields(unitId);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="info"></param>
        public Unit(UnitInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            this.unitId = info.UnitId;
            units = GlobalResources.Instance.UnitsDat;

            hitpoints = units.GetHitPoints(info.UnitId);
            shields = units.GetShields(info.UnitId);
            x = info.PositionX;
            y = info.PositionY;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="palette"></param>
        /// <returns></returns>
        public Sprite CreateSprite(Mpq mpq, byte[] palette)
        {
            if (sprite != null)
            {
                throw new Exception();
            }

            sprite = SpriteManager.CreateSprite(mpq, SpriteId, palette, x, y);

            sprite.RunScript(AnimationType.Init);

            return sprite;
        }

        /// <summary>
        ///
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public Sprite Sprite
        {
            get { return sprite; }
        }

        /// <summary>
        ///
        /// </summary>
        public int FlingyId
        {
            get { return units.GetFlingyId(unitId); }
        }

        /// <summary>
        ///
        /// </summary>
        public int SpriteId
        {
            get { return GlobalResources.Instance.FlingyDat.GetSpriteId(FlingyId); }
        }

        /// <summary>
        ///
        /// </summary>
        [CLSCompliant(false)]
        public uint ConstructSpriteId
        {
            get { return units.GetConstructSpriteId(unitId); }
        }

        /// <summary>
        ///
        /// </summary>
        public int AnimationLevel
        {
            get { return units.GetAnimationLevel(unitId); }
        }

        /// <summary>
        ///
        /// </summary>
        [CLSCompliant(false)]
        public uint HitPoints
        {
            get { return hitpoints; }
            set { hitpoints = value; }
        }

        /// <summary>
        ///
        /// </summary>
        [CLSCompliant(false)]
        public uint Shields
        {
            get { return shields; }
            set { shields = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public int CreateScore
        {
            get { return units.GetCreateScore(unitId); }
        }

        /// <summary>
        ///
        /// </summary>
        public int DestroyScore
        {
            get { return units.GetDestroyScore(unitId); }
        }

        /// <summary>
        ///
        /// </summary>
        public int SelectionCircle
        {
            get { return GlobalResources.Instance.SpritesDat.GetSelectionCircle(SpriteId); }
        }

        /// <summary>
        ///
        /// </summary>
        public int SelectionCircleOffset
        {
            get { return GlobalResources.Instance.SpritesDat.GetSelectionCircleOffset(SpriteId); }
        }
    }
}
