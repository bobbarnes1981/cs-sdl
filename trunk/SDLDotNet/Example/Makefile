TARGET = Example.exe
SRCS = AssemblyInfo.cs Example.cs

$(TARGET): $(SRCS)
	ln -sf ../SDL.NET.dll .
	mcs -define:__MONO__ -out:$(TARGET) $(SRCS) -r:SDL.NET.dll,System.Drawing.dll

clean:
	rm -f Example.exe SDL.NET.dll
