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

using SdlDotNet.Graphics;

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using SCSharp;
using SCSharp.MpqLib;

namespace SCSharp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public static class SpriteManager
    {
        /// <summary>
        /// 
        /// </summary>
        private static Collection<Sprite> sprites = new Collection<Sprite>();

        /// <summary>
        /// 
        /// </summary>
        public static Collection<Sprite> Sprites
        {
            get { return SpriteManager.sprites; }
        }
        static Painter painter;

        static Mpq ourMpq;

        /// <summary>
        /// 
        /// </summary>
        static int positionX;

        /// <summary>
        /// 
        /// </summary>
        public static int PositionX
        {
            get { return SpriteManager.positionX; }
            set { SpriteManager.positionX = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        static int positionY;

        /// <summary>
        /// 
        /// </summary>
        public static int PositionY
        {
            get { return SpriteManager.positionY; }
            set { SpriteManager.positionY = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mpq"></param>
        /// <param name="spriteNumber"></param>
        /// <param name="palette"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <returns></returns>
        public static Sprite CreateSprite(Mpq mpq, int spriteNumber, byte[] palette, int positionX, int positionY)
        {
            ourMpq = mpq;
            return CreateSprite(spriteNumber, palette, positionX, positionY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentSprite"></param>
        /// <param name="imagesNumber"></param>
        /// <param name="palette"></param>
        /// <returns></returns>
        [CLSCompliant(false)]
        public static Sprite CreateSprite(Sprite parentSprite, ushort imagesNumber, byte[] palette)
        {
            Sprite sprite = new Sprite(parentSprite, imagesNumber, palette);
            sprites.Add(sprite);

            if (painter != null)
            {
                sprite.AddToPainter(painter);
            }

            return sprite;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteNumber"></param>
        /// <param name="palette"></param>
        /// <param name="positionY"></param>
        /// <param name="positionX"></param>
        /// <returns></returns>
        public static Sprite CreateSprite(int spriteNumber, byte[] palette, int positionX, int positionY)
        {
            Sprite sprite = new Sprite(ourMpq, spriteNumber, palette, positionX, positionY);
            sprites.Add(sprite);
            if (painter != null)
            {
                sprite.AddToPainter(painter);
            }

            return sprite;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sprite"></param>
        public static void RemoveSprite(Sprite sprite)
        {
            if (painter != null)
            {
                sprite.RemoveFromPainter(painter);
            }
            sprites.Remove(sprite);
        }

        static void SpriteManagerPainterTick(Surface surf, DateTime now)
        {
            //foreach (Sprite e in sprites)
            //{
            //    if (e.Tick(surf, now) == false)
            //    {
            //        Console.WriteLine("removing sprite!!!!");
            //        RemoveSprite(e);
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="painterToAdd"></param>
        public static void AddToPainter(Painter painterToAdd)
        {
            painterToAdd.Add(Layer.Background, SpriteManagerPainterTick);

            painter = painterToAdd;
            foreach (Sprite s in sprites)
            {
                s.AddToPainter(painter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="painterToRemove"></param>
        public static void RemoveFromPainter(Painter painterToRemove)
        {
            painterToRemove.Remove(Layer.Background, SpriteManagerPainterTick);

            foreach (Sprite s in sprites)
            {
                s.RemoveFromPainter(painterToRemove);
            }
            painter = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        public static void SetUpperLeft(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }
    }
}
