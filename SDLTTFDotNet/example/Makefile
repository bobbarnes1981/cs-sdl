TARGET = FontTest.exe
SRCS = FontTest.cs
CSCOMPILER = mcs
TTFLIB = SDLDotNet.TTF.dll

all: example

example: $(SRCS)
	ln -sf ../$(TTFLIB) .
	$(CSCOMPILER) -out:$(TARGET) -r:$(TTFLIB) -r:SDL.NET.dll -r:System.Drawing.dll $(SRCS)

clean:
	rm -f $(TARGET)
	rm -f $(TTFLIB)
