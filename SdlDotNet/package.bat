tools\Prebuild\Prebuild.exe /target nant /file prebuild.xml
NAnt.exe clean
NAnt.exe -buildfile:package.xml
