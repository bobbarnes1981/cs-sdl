TARGET = SDL.NET.dll
SRCS = AssemblyInfo.cs CDAudio.cs EventEnums.cs Events.cs Joysticks.cs Mixer.cs \
	Natives.cs SDL.cs SDLException.cs Surface.cs Video.cs WindowManager.cs

$(TARGET): $(SRCS)
	mcs -define:__MONO__ -target:library --unsafe -out:$(TARGET) $(SRCS) -r:System.Drawing.dll

clean:
	rm -f SDL.NET.dll
