==AudioExample==
Copyright notice for mason2.mid from www.worldforge.org

Copyright (c)  2002  Squif, a.k.a. Rob LeFebvre
You can redistribute this media and/or modify it under the terms of the GNU General Public License and/or under the terms of the GNU Free Documentation License (FDL) as published by the Free Software Foundation.   You can redistribute and/or modify this m
edia under the terms of version 2 of the GPL or any later version.  You may also redistribute this media and/or modify it under the terms of version 1.1 of the FDL or any later version.  You may also also redistribute and/or modify this media using both t
he terms of the GPL (version 2 or later versions) and the FDL (version 1.1 or later versions).

See the file COPYING.txt for details.  The newest versions of the GPL and GFDL can be found at the Free Software Foundation's web site http://www.fsf.org.


==Gears==
 Luciano Martorella - 2003 10 15
 NOINET - http://net.supereva.it/noinetcorp
 
  The GLUT example "gears 3D" ported in C#
  
  Based on SDLgears (included in this package), this porting use Tao library (for OpenGL) and SLD.NET.
  The program source is very simple.

  TODO: keyboard interaction.

==SnowDemo==
SneeuwDemo 0.1 Readme file
--------------------------

- Software requirements

Windows 98> or 2000> with the Microsoft .NET framework or mono
GNU/Linux with mono (not tested yet) or Portable .NET (also not tested)

- Tested on...

Pentium III at 900 Mhz
128 MB of SD-RAM
Intel i815 on-board graphics

About 50-60 frames per seconds at average.

- About the demo

This demo is my first public demo, so don't be too cruel. It simulates some bits of snow falling down, at variable speeds and wind effects. Als there is the text of In Dreams from the Lord of the Rings.

It is written in C# with SDL.NET and should be binary compatible on all platforms supported by a .NET runtime like Microsoft .NET or mono.

The demo opens a DOS box, to display some logs. Every 5 seconds it runs it will display the framerate over those 5 seconds.

- Things that I need to do

Give a command line option for the number of snow-thingies, so one can have fun with that :P

Also I've been to lazy to change some code things...

- About the author

I'm Sijmen Mulder, 16 years old. Currently doing some Computer Science study, and I hope to get to Full Sail once.

Version 0.2
 - Fade out of the text
 - Fade in is now faster
 - Code cleanup for the Tekst class
 - Shadow for the text
 - Alpha transparency for flocks further away. But becausse of this, the flock image is not static anymore so it takes a huge extra amount of memory. Sorry!
 - Framerate dropped by a few percent (about 2%) because of the increased use of alpha blending.
 - Changed filenames, SneeuwDemo now runs on Linux too (filename cases were wrong...)

Version 0.1
 - Initial release


==BombRun==
Bomb Run
Copyright (c) 2003 CL Game Stuidios (Sijmen Mulder)
This program (game) is published under the terms of the GNU General Public Licence version 2.

The goal of the game is to avoid as many bombs as possible, before being hit by one.

Features:
 - New original 2D graphics technology developped by CL Game Studios (thats me :P)
 - Varying bomb speeds
 - Challenging bomb speed algoritm
 - Jump ability

The bomb speed algoritm is as follows:

* new bomb speed is incremented by 3 pixels/sec each second
* once the bomb speed reaches MAXSPEED, the speed is halved and MAXSPEED multiplied by two

This technology creates 'waves like' speed bursts while the upward difficulty line is maintained.

The graphics technology is as follows. The illusion is given to the player that two images are behind each other, and that the objects are cutouts in the front image.

The truth is, that from a black/white image the black part is blitted on a temporary surface that already contains the back image part of that spot, and then that temporary image is blitted with a black colorkey.

Anyways, have fun!

--
Sijmen Mulder

This game is entirely (except from the current background images) made by me, Sijmen Mulder.

I am currently 16 years old and live in the Netherlands, you can find more information about me on my website (www.wpws.nl/~sijmen).

My email address is sjmulder at sourceforge dot net.
