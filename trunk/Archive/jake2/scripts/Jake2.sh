#!/bin/sh

cd `dirname $0`

if [ `uname -s` = "Darwin" ] ; then
	export UNAME="osx"
	export DYLD_LIBRARY_PATH=lib/$UNAME/lwjgl
elif [ `uname -s` = "Linux" ] ; then
	export UNAME="linux"
fi

export LD_LIBRARY_PATH=lib/$UNAME/lwjgl
CP=lib/jake2.jar:lib/lwjgl.jar:lib/lwjgl_util.jar

exec java -Xmx100M -Djava.library.path=lib/$UNAME/lwjgl -cp $CP jake2.Jake2
