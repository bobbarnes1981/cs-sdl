./prebuild /target nant /build NET_2_0 /file prebuild.xml
nant clean
nant -buildfile:package.xml
