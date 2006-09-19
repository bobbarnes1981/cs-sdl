./prebuild /target nant /file prebuild2.xml
nant clean
nant -buildfile:package2.xml
