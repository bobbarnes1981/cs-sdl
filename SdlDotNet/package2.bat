tools\Prebuild\Prebuild.exe /target nant /file prebuild2.xml
NAnt.exe clean
NAnt.exe -buildfile:package2.xml
