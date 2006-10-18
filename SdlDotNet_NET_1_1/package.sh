./prebuild /target nant /build NET_1_1 /file prebuild.xml
nant clean
nant -buildfile:package.xml
