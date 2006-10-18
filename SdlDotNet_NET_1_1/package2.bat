tools\Prebuild\Prebuild.exe /target nant /build NET_2_0 /file prebuild.xml
NAnt.exe clean
NAnt.exe -buildfile:package2.xml
