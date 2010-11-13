TARGET = FontTest.exe
SRCS = FontTest.cs
CSCOMPILER = mcs
TTFLIB = SDL-TTF.NET.dll

all: example

example: $(SRCS)
	ln -sf ../$(TTFLIB) .
	$(CSCOMPILER) -g -out:$(TARGET) -r $(TTFLIB) -r SDL.NET.dll -r System.Drawing.dll $(SRCS)

clean:
	rm -f $(TARGET)
	rm -f $(TTFLIB)
