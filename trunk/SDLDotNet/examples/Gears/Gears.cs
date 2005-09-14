/*
* 3-D gear wheels. This program is in the public domain.
*
* Brian Paul
*
* Conversion to GLUT by Mark J. Kilgard 
* Conversion to C# by Lux, Using Tao & SDL.NET - 2003/10/15 
* Revised to use SdlDotNet 2.x by David Hudson 2004-11-04
* Revised to use SdlDotNet 3.x by David Hudson 2005-02-04
*
*/

using System;

using SdlDotNet;
using Tao.OpenGl;

namespace SdlDotNet.Examples
{
	class Gears
	{
//		Draw a gear wheel. You'll probably want to call this function when
//		building a display list since we do a lot of trig here.
//
//		Input: inner_radius - radius of hole at center
//		outer_radius - radius at center of teeth
//		width - width of gear
//		teeth - number of teeth
//		tooth_depth - depth of tooth

		static private void gear (double inner_radius,
			double outer_radius,
			double width,
			int teeth,
			double tooth_depth)
		{
			int i;
			double angle;
			double u; 
			double v; 
			double len;

			double r0 = inner_radius;
			double r1 = outer_radius - tooth_depth / 2.0;
			double r2 = outer_radius + tooth_depth / 2.0;

			double da = 2.0 * Math.PI / teeth / 4.0;

			Gl.glShadeModel(Gl.GL_FLAT);

			Gl.glNormal3d(0.0, 0.0, 1.0);

			/* draw front face */
			Gl.glBegin(Gl.GL_QUAD_STRIP);
			for (i = 0; i <= teeth; i++)
			{
				angle = i * 2.0 * Math.PI / teeth;
				Gl.glVertex3d(r0 * Math.Cos(angle), 
					r0 * Math.Sin(angle), width * 0.5);
				Gl.glVertex3d(r1 * Math.Cos(angle), 
					r1 * Math.Sin(angle), width * 0.5);
				if (i < teeth)
				{
					Gl.glVertex3d(r0 * Math.Cos(angle), 
						r0 * Math.Sin(angle), width * 0.5);
					Gl.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
						r1 * Math.Sin(angle + 3 * da), width * 0.5);
				}
			}
			Gl.glEnd();

			/* draw front sides of teeth */
			Gl.glBegin(Gl.GL_QUADS);
			da = 2.0 * Math.PI / teeth / 4.0;
			for (i = 0; i < teeth; i++)
			{
				angle = i * 2.0 * Math.PI / teeth;

				Gl.glVertex3d(r1 * Math.Cos(angle), 
					r1 * Math.Sin(angle), width * 0.5);
				Gl.glVertex3d(r2 * Math.Cos(angle + da), 
					r2 * Math.Sin(angle + da), width * 0.5);
				Gl.glVertex3d(r2 * Math.Cos(angle + 2 * da), 
					r2 * Math.Sin(angle + 2 * da), width * 0.5);
				Gl.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
					r1 * Math.Sin(angle + 3 * da), width * 0.5);
			}
			Gl.glEnd();

			Gl.glNormal3d (0.0, 0.0, -1.0);

			/* draw back face */
			Gl.glBegin(Gl.GL_QUAD_STRIP);

			for (i = 0; i <= teeth; i++)
			{
				angle = i * 2.0 * Math.PI / teeth;
				Gl.glVertex3d(r1 * Math.Cos(angle), 
					r1 * Math.Sin(angle), -width * 0.5);
				Gl.glVertex3d(r0 * Math.Cos(angle), 
					r0 * Math.Sin(angle), -width * 0.5);
				if (i < teeth)
				{
					Gl.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
						r1 * Math.Sin(angle + 3 * da), -width * 0.5);
					Gl.glVertex3d(r0 * Math.Cos(angle), 
						r0 * Math.Sin(angle), -width * 0.5);
				}
			}
			Gl.glEnd();

			/* draw back sides of teeth */
			Gl.glBegin(Gl.GL_QUADS);
			da = 2.0 * Math.PI / teeth / 4.0;
			for (i = 0; i < teeth; i++)
			{
				angle = i * 2.0 * Math.PI / teeth;

				Gl.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
					r1 * Math.Sin(angle + 3 * da), -width * 0.5);
				Gl.glVertex3d(r2 * Math.Cos(angle + 2 * da), 
					r2 * Math.Sin(angle + 2 * da), -width * 0.5);
				Gl.glVertex3d(r2 * Math.Cos(angle + da), 
					r2 * Math.Sin(angle + da), -width * 0.5);
				Gl.glVertex3d(r1 * Math.Cos(angle), 
					r1 * Math.Sin(angle), -width * 0.5);
			}
			Gl.glEnd();

			/* draw outward faces of teeth */
			Gl.glBegin(Gl.GL_QUAD_STRIP);
			for (i = 0; i < teeth; i++)
			{
				angle = i * 2.0 * Math.PI / teeth;

				Gl.glVertex3d(r1 * Math.Cos(angle), 
					r1 * Math.Sin(angle), width * 0.5);
				Gl.glVertex3d(r1 * Math.Cos(angle), 
					r1 * Math.Sin(angle), -width * 0.5);
				u = r2 * Math.Cos(angle + da) - r1 * Math.Cos(angle);
				v = r2 * Math.Sin(angle + da) - r1 * Math.Sin(angle);
				len = Math.Sqrt(u * u + v * v);
				u /= len;
				v /= len;
				Gl.glNormal3d(v, -u, 0.0);
				Gl.glVertex3d(r2 * Math.Cos(angle + da), 
					r2 * Math.Sin(angle + da), width * 0.5);
				Gl.glVertex3d(r2 * Math.Cos(angle + da), 
					r2 * Math.Sin(angle + da), -width * 0.5);
				Gl.glNormal3d(Math.Cos(angle), Math.Sin(angle), 0.0);
				Gl.glVertex3d(r2 * Math.Cos(angle + 2 * da), 
					r2 * Math.Sin(angle + 2 * da), width * 0.5);
				Gl.glVertex3d(r2 * Math.Cos(angle + 2 * da), 
					r2 * Math.Sin(angle + 2 * da), -width * 0.5);
				u = r1 * Math.Cos(angle + 3 * da) - r2 * Math.Cos(angle + 2 * da);
				v = r1 * Math.Sin(angle + 3 * da) - r2 * Math.Sin(angle + 2 * da);
				Gl.glNormal3d(v, -u, 0.0);
				Gl.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
					r1 * Math.Sin(angle + 3 * da), width * 0.5);
				Gl.glVertex3d(r1 * Math.Cos(angle + 3 * da), 
					r1 * Math.Sin(angle + 3 * da), -width * 0.5);
				Gl.glNormal3d(Math.Cos(angle), Math.Sin(angle), 0.0);
			}

			Gl.glVertex3d(r1 * Math.Cos(0), r1 * Math.Sin(0), width * 0.5);
			Gl.glVertex3d(r1 * Math.Cos(0), r1 * Math.Sin(0), -width * 0.5);

			Gl.glEnd();

			Gl.glShadeModel(Gl.GL_SMOOTH);

			/* draw inside radius cylinder */
			Gl.glBegin(Gl.GL_QUAD_STRIP);
			for (i = 0; i <= teeth; i++)
			{
				angle = i * 2.0 * Math.PI / teeth;
				Gl.glNormal3d(-Math.Cos(angle), -Math.Sin(angle), 0.0);
				Gl.glVertex3d(r0 * Math.Cos(angle), 
					r0 * Math.Sin(angle), -width * 0.5);
				Gl.glVertex3d(r0 * Math.Cos(angle), 
					r0 * Math.Sin(angle), width * 0.5);
			}
			Gl.glEnd();
		}

		static double view_rotx = 20.0f; 
		static double view_roty = 30.0f; 
		static double view_rotz = 0.0f;
		static int gear1;
		static int gear2; 
		static int gear3;
		static double angle = 0.0f;

		static int T0 = 0;
		static int Frames = 0;

		static void draw ()
		{
			Gl.glClear (Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);

			Gl.glPushMatrix();
			Gl.glRotated(view_rotx, 1.0, 0.0, 0.0);
			Gl.glRotated(view_roty, 0.0, 1.0, 0.0);
			Gl.glRotated(view_rotz, 0.0, 0.0, 1.0);

			Gl.glPushMatrix();
			Gl.glTranslated(-3.0, -2.0, 0.0);
			Gl.glRotated(angle, 0.0, 0.0, 1.0);
			Gl.glCallList(gear1);
			Gl.glPopMatrix();

			Gl.glPushMatrix();
			Gl.glTranslated(3.1, -2.0, 0.0);
			Gl.glRotated(-2.0 * angle - 9.0, 0.0, 0.0, 1.0);
			Gl.glCallList(gear2);
			Gl.glPopMatrix();

			Gl.glPushMatrix();
			Gl.glTranslated(-3.1, 4.2, 0.0);
			Gl.glRotated(-2.0 * angle - 25.0, 0.0, 0.0, 1.0);
			Gl.glCallList(gear3);
			Gl.glPopMatrix();

			Gl.glPopMatrix();

			Video.GLSwapBuffers();

			Frames++;
			int t = Timer.TicksElapsed;
			if (t - T0 >= 5000)
			{
				double seconds = (t - T0) / 1000.0;
				double fps = Frames / seconds;
				System.Console.WriteLine("c#: {0} frames in {1} seconds = {2} FPS", Frames, seconds, fps);
				T0 = t;
				Frames = 0;
			}
		}


		static void idle ()
		{
			angle += 2.0;
		}

		static int m_newW;
		static int m_newH;

		/* new window size or exposure */
		static void reshape ()
		{
			m_newW = m_screen.Width;
			m_newH = m_screen.Height;
			double h = (double)m_newH / (double)m_newW;

			Gl.glViewport (0, 0, m_newW, m_newH);
			Gl.glMatrixMode (Gl.GL_PROJECTION);
			Gl.glLoadIdentity ();
			Gl.glFrustum (-1.0, 1.0, -h, h, 5.0, 60.0);
			Gl.glMatrixMode (Gl.GL_MODELVIEW);
			Gl.glLoadIdentity ();
			Gl.glTranslated (0.0, 0.0, -40.0);
		}

		static void init ()
		{
			float[] pos = {5.0f, 5.0f, 10.0f, 0.0f};
			float[] red = {0.8f, 0.1f, 0.0f, 1.0f};
			float[] green = {0.0f, 0.8f, 0.2f, 1.0f};
			float[] blue = {0.2f, 0.2f, 1.0f, 1.0f};

			Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, pos);
			Gl.glEnable(Gl.GL_CULL_FACE);
			Gl.glEnable(Gl.GL_LIGHTING);
			Gl.glEnable(Gl.GL_LIGHT0);
			Gl.glEnable(Gl.GL_DEPTH_TEST);

			/* make the gears */
			gear1 = Gl.glGenLists(1);
			Gl.glNewList(gear1, Gl.GL_COMPILE);
			Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE, red);
			gear(1.0, 4.0, 1.0, 20, 0.7);
			Gl.glEndList();

			gear2 = Gl.glGenLists(1);
			Gl.glNewList(gear2, Gl.GL_COMPILE);
			Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE, green);
			gear(0.5, 2.0, 2.0, 10, 0.7);
			Gl.glEndList();

			gear3 = Gl.glGenLists(1);
			Gl.glNewList(gear3, Gl.GL_COMPILE);
			Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT_AND_DIFFUSE, blue);
			gear(1.3, 2.0, 0.5, 10, 0.7);
			Gl.glEndList();

			Gl.glEnable(Gl.GL_NORMALIZE);

				Console.WriteLine ("GL_RENDERER = {0}", 
					Gl.glGetString(Gl.GL_RENDERER));
				Console.WriteLine ("GL_VERSION = {0}", 
					Gl.glGetString(Gl.GL_VERSION));
				Console.WriteLine ("GL_VENDOR = {0}", 
					Gl.glGetString(Gl.GL_VENDOR));
				Console.WriteLine ("GL_EXTENSIONS = {0}", 
					Gl.glGetString(Gl.GL_EXTENSIONS));
		}

		static bool quitFlag = false;
		static Surface m_screen;

		/// <summary>
		/// 
		/// </summary>
		public void Run()
		{
			m_screen = Video.SetVideoModeWindowOpenGL(500, 500, true);
			Events.Quit += new QuitEventHandler (this.Quit);
			Events.VideoResize += new VideoResizeEventHandler (this.Resize);
			Events.KeyboardDown +=
				new KeyboardEventHandler(this.KeyboardDown);

			Video.WindowCaption = "SdlDotNet - Gears";

			init ();
			reshape ();
			while ( ! quitFlag )
			{
				idle();
				while (Events.Poll()) ;
				if (m_screen.Width != m_newW || m_screen.Height != m_newH)
				{
					reshape ();
				}
				draw ();
			}
		}

		[STAThread]
		static void Main() 
		{
			Gears gears = new Gears();
			gears.Run();
		}

		private void Quit (object sender, QuitEventArgs e)
		{
			quitFlag = true;
		}

		private void Resize (object sender, VideoResizeEventArgs e)
		{
			m_screen = Video.SetVideoModeWindowOpenGL(e.W, e.H, true);
		}
		private void KeyboardDown(
			object sender,
			KeyboardEventArgs e)
		{
			if (e.Key == Key.Escape ||
				e.Key == Key.Q)
			{
				quitFlag = true;
			}
		}
	}
}
