using System; 
using System.Drawing; 
using SdlDotNet; 

namespace SdlDotNet.Examples 
{ 
	class SimpleExample 
	{ 
		#region Variables 
       
		private const int width = 640; 
		private const int height = 480; 
		private Random rand = new Random(); 
		private bool quit = false; 
       
		private Surface screen; 
		//private Font font; 
       
		#endregion 

       
		public SimpleExample() 
		{ 
			screen = Video.SetVideoModeWindow(width, height); 
			Video.WindowCaption = "SdlDotNet - Simple Example"; 
          
			Events.KeyboardDown += new KeyboardEventHandler(this.KeyDown); 
			Events.Quit += new QuitEventHandler(this.Quit); 
			Events.Tick += new TickEventHandler(this.FrameTick); 
          
			Events.FPS = 40; 
			Events.Run(); 
		} 

		private void KeyDown(object sender, KeyboardEventArgs e) 
		{ 
			if (e.Key == Key.Escape) 
				quit = true; 
		} 

		private void Quit(object sender, QuitEventArgs e) 
		{ 
			quit = true; 
		} 

		public void FrameTick(object sender, TickEventArgs e) 
		{ 
			screen.Fill(Color.FromArgb(rand.Next(255),rand.Next(255),rand.Next(255))); 
			screen.Flip(); 
		} 
       
		public void Run() 
		{ 
			while (quit == false) 
			{ 
				while(Events.Poll()) 
				{ 
				} 
			} 
		} 

		public static void Main() 
		{ 
			SimpleExample t = new SimpleExample(); 
			t.Run(); 
		} 
	} 
} 
