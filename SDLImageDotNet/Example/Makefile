TARGET = ImageExample.exe
SRCS = AssemblyInfo.cs ImageSample.cs

$(TARGET): $(SRCS)
	mcs -define:__MONO__ -out:$(TARGET) $(SRCS) -r:..\..\SDLDotNet\SDL.NET.dll -r:..\SDLImage.NET.dll -r:System.Drawing.dll
	
clean:
	rm -f SDL.NET.dll
