tools\Prebuild\Prebuild.exe /target nant /build NET_1_1 /file prebuild.xml
NAnt.exe clean
NAnt.exe -buildfile:package.xml
