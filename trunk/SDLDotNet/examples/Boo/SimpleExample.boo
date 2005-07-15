namespace SdlDotNet.Examples

import SdlDotNet from SdlDotNet
import System.Threading
import System.Drawing from System.Drawing

def KeyDown(sender, e as KeyboardEventArgs):
	if e.Key == Key.Escape:
		System.Environment.Exit(0)

def Quit(sender, e):
	System.Environment.Exit(0)
	
width = 640
height = 480
quit = false
rand = System.Random()
screen = Video.SetVideoModeWindow(width, height)
Events.KeyboardDown += KeyDown
Events.Quit += Quit
while not quit:
	while Events.Poll():
		pass
	screen.Fill(Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255)))
	Thread.Sleep(100)
	screen.Flip()
