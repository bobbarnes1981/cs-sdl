./prebuild /target nant /file prebuild.xml
nant clean
nant -buildfile:package.xml
