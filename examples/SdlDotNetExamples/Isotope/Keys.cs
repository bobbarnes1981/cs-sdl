#region LICENSE
/* 
 * (c) 2005 Simon Gillespie
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

// Isotope: Isometric Object Oriented Pygame Engine
// Isometric game engine module for pygame
// Author: Simon Gillespie
// License: GPL Copyright Oct 2005
/*
*                       
       I S O T O P E <>
             <> <>
        <> <>
       
       The Isometric Engine for Python

 Isotope is an isometric game engine based on Pygame, and written in Python. It provides
 the framework for constructing an isometric graphics game with actors who can pick up objects,
 jump onto platforms. Automated actors who can interact with the player or their environment.

 Features:   
    <> Actors: used for player and monster game  Capable of facing, gravity,
         collision response, jumping, automation and inventory.
    <> Multiframe animation and images
    <> Simple physics simulation of velocity, collision and touch detection.
    <> All objects can be customised and extended using Python.
    <> Free commented open source code.

 Author: Simon Gillespie
 License: GPL Oct 2005

 ISOTOPE Modules:

    <> Control interfaces: Interfaces to control the Isotope system

    isotope:          A complete game engine with information panel, keyboard control and examine object surface mode.
    isotope_elements: A lower level interface to isotope which allows direct control of the isometric view and
                      the object simulator.

    <> Atomic classes: Class definitions for defining objects and how they appear in an isometric view

    actors:           Definitions of high level objects which can face, jump and carry 
    special_objects:  Complex Object definitions.
    objects:          Object definitions for basic objects and objects that can be carried or affected by gravity.
    scene:            Definitions to support scenes, groups of objects and scenetypes.
    skins:            Translators from object information into sprite images.

    <> Function Libraries: Low level routines to support the Isotope system

    isometric:        Function Library to draw isometric views.
    physics:          Function Library for 3d simulation of object physics.
    sprites:          Function Library to draw the sprites on the surface.
    vector:           Function Library for 3d vector mathematics.
*/

using System;
using System.IO;
using System.Collections;
using System.Drawing;

using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Input;

namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// 
    /// </summary>
    public class Keys
    {
        /*keys class

           left,right,up,down,jump,pick_up,drop,examine,using: key codes for the player keys: key
        */
        private Key left;

        public Key Left
        {
            get { return left; }
            set { left = value; }
        }
        private Key right;

        public Key Right
        {
            get { return right; }
            set { right = value; }
        }
        private Key up;

        public Key Up
        {
            get { return up; }
            set { up = value; }
        }
        private Key down;

        public Key Down
        {
            get { return down; }
            set { down = value; }
        }
        private Key jump;

        public Key Jump
        {
            get { return jump; }
            set { jump = value; }
        }
        private Key pickup;

        public Key Pickup
        {
            get { return pickup; }
            set { pickup = value; }
        }
        private Key drop;

        public Key Drop
        {
            get { return drop; }
            set { drop = value; }
        }
        private Key examine;

        public Key Examine
        {
            get { return examine; }
            set { examine = value; }
        }
        private Key usingKey;

        public Key UsingKey
        {
            get { return usingKey; }
            set { usingKey = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <param name="jump"></param>
        /// <param name="pick_up"></param>
        /// <param name="drop"></param>
        /// <param name="examine"></param>
        /// <param name="usingk"></param>
        public Keys(Key left, Key right, Key up, Key down,
            Key jump, Key pickup, Key drop, Key examine, Key usingKey)
        {
            this.left = left;
            this.right = right;
            this.up = up;
            this.down = down;
            this.jump = jump;
            this.pickup = pickup;
            this.drop = drop;
            this.examine = examine;
            this.usingKey = usingKey;
        }
    }
}