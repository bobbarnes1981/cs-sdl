TARGET = SDLDotNet.TTF.dll
SRCS = SDLTTF.cs Font.cs SDLColor.cs SDLTTFException.cs
##
# To compile for windows, change this to "csc"
##
CSCOMPILER = mcs

all: $(TARGET)

$(TARGET): $(SRCS)
	$(CSCOMPILER) -target:library -out:$(TARGET) -r:System.Drawing.dll -r:SDL.NET.dll $(SRCS)

clean:
	rm -f $(TARGET)

install: $(TARGET)
	/usr/bin/install -m 644 $(TARGET) /usr/local/lib/
