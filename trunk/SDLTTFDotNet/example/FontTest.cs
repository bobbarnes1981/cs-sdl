using System;
using System.Drawing;
using SDLDotNet;
using SDLDotNet.TTF;

namespace SDLDotNet.TTF.Example
{
	class FontTest
	{
		private bool mDone = false;
		private Surface screen;
		private Surface text;

		static public void Main( string[] args )
		{
			string FontName = "";
			Style Style = Style.Normal;
			int Size;
			string Text;
			int i;

			string Usage = "Usage: FontTest.exe [-b] [-i] [-u] <font>.ttf [ptsize] [text]";

			for(i = 0; i < args.Length; ++i)
			{
				string Arg = args[i];
				if (Arg.StartsWith("-") == false)
				{
					FontName = Arg;
					break;
				} else if (Arg == "-b")
				{
					Style |= Style.Bold;
				} else if (Arg == "-i")
				{
					Style |= Style.Italic;
				} else if (Arg == "-u")
				{
					Style |= Style.Underline;
				} else
				{
					Console.WriteLine(Usage);
					return;
				}
			}

			if (FontName=="")
			{
				Console.WriteLine(Usage);
				return;
			}

			if (++i < args.Length)
			{
				Size = Convert.ToInt32(args[i]);
			} else
			{
				Size = 12;
			}

			if (++i < args.Length)
			{
				Text = args[i];
			} else
			{
				Text = FontName;
			}

			new FontTest().Run(FontName, Style, Size, Text);
		}

		public void Run(string FontName, Style Style, int Size, string Text)
		{
			Font font;

			Console.WriteLine("Font\t{0}\nStyle\t{1}\nSize\t{2}\nText\t{3}\n", FontName, Style, Size, Text);

			SDL sdl = SDL.Instance;
			sdl.Events.Quit += new QuitEventHandler(SDL_Quit);
			sdl.Events.MouseButtonDown += new MouseButtonEventHandler(SDL_MouseButtonEvent);
			sdl.Events.KeyboardDown += new KeyboardEventHandler(SDL_KeyboardDown);

			font = new Font(FontName, Size);
			font.Style = Style;

			try
			{
				screen = sdl.Video.SetVideoModeWindow(640, 480, true); // Frame = true
				sdl.WindowManager.Caption = "SDL.TTF.NET test ";
				//screen.FillRect(new System.Drawing.Rectangle(0,0,100,100), System.Drawing.Color.Green);
				screen.Flip();
				text = font.RenderTextSolid(Text, new SDLColor(147, 112, 219));
				while (!mDone)
				{
					sdl.Events.WaitAndDelegate();
				}
			} catch {
				sdl.Dispose();
				throw;
			}
		}

		private void SDL_Quit() {
			mDone = true;
		}

		private void SDL_MouseButtonEvent(MouseButton button, bool down, int x, int y)
		{
			System.Drawing.Rectangle DestRect;

			DestRect = new System.Drawing.Rectangle(new System.Drawing.Point(x, y), text.Size);
			text.Blit( screen, DestRect );
			screen.Flip();
		}

		private void SDL_KeyboardDown(int device, bool down, int scancode, Key key, Mod mod)
		{
			mDone = true;
		}
	}
}
