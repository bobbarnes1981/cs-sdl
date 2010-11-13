HEADERS    = 
SOURCES    = cssdl.c
CS_SOURCES = $(shell find . -name "*.cs")
LIB_DIR    = ../../lib
# ----------------------------
# standart rules and variables
# ----------------------------
VPATH = $(LIB_DIR) .
#-----------------------------

all: csgl-sdl-native.dll csgl-sdl.dll
doc: csgl-sdl.xml
csgl-sdl.xml: $(CS_SOURCES)
	-rm $(LIB_DIR)/csgl-sdl.dll
	make DOCFLAG="/doc:$(LIB_DIR)/csgl-sdl.xml" all

$(LIB_DIR):
	cd ../.. && mkdir -p lib

clean:
	-rm $(LIB_DIR)/csgl-sdl*.dll

csgl-sdl.dll: $(CS_SOURCES)
	csc /nologo /unsafe /t:library $(DOCFLAG) \
		/out:$(LIB_DIR)/$@ /recurse:*.cs

csgl-sdl-native.dll: $(LIB_DIR) $(HEADERS) $(SOURCES)
	gcc -shared -Wall -o $(LIB_DIR)/$@ $(SOURCES) -lSDL
	strip $(LIB_DIR)/$@
