Name "Mezzanine"

OutFile "mezzanine_2006_xx_xx_setup.exe"

InstallDir $PROGRAMFILES\Mezzanine

InstallDirRegKey HKLM "Software\Mezzanine" "Install_Dir"

SetCompressor /SOLID lzma
XPStyle on

Page components
Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles

Section "Mezzanine (required)"

  SectionIn RO
  
  SetOutPath $INSTDIR
  
  File /r "..\..\*.*"
  
  WriteRegStr HKLM SOFTWARE\Sauerbraten "Install_Dir" "$INSTDIR"
  
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Mezzanine" "DisplayName" "Mezzanine"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Mezzanine" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Mezzanine" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Mezzanine" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd

Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\Mezzanine"
  CreateShortCut "$SMPROGRAMS\Mezzanine\Mezzanine.lnk" "$INSTDIR\mezzanine.bat" "" "$INSTDIR\mezzanine.bat" 0
  CreateShortCut "$SMPROGRAMS\Mezzanine\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  
SectionEnd

Section "Uninstall"
  
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Mezzanine"
  DeleteRegKey HKLM SOFTWARE\Sauerbraten

  RMDir /r "$SMPROGRAMS\Mezzanine"
  RMDir /r "$INSTDIR"

SectionEnd
