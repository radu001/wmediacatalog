; Please compile using following preprocessor parameters:
; ISCC.exe /d"OutDir=<path for output installer>" /d"SourceDir=<release source path>" "wmediacatalog.iss"

[Setup]
SourceDir={#SourceDir}
OutputDir={#OutDir}
AppName=WMediaCatalog
AppId=14776ff5-d54e-4624-ab41-a045a6843374
AppVersion=1.0
AppPublisher=Ilya N. Ternovich
AppPublisherURL=http://code.google.com/p/wmediacatalog/
AppContact=ternovich@gmail.com
DefaultDirName={pf}\WMediaCatalog
DefaultGroupName=WMediaCatalog
UninstallDisplayIcon={app}\MediaCatalog.exe
LicenseFile=WMediaCatalog.license.txt

[Icons]
Name: "{group}\WMediaCatalog"; Filename: "{app}\MediaCatalog.exe"
Name: "{group}\Uninstall WMediaCatalog"; Filename: "{uninstallexe}"

[Registry]
Root: HKLM; Subkey: "Software\WMediacatalog"; Flags: uninsdeletekey

[Files]
Source: "MediaCatalog.exe"; DestDir: "{app}"
Source: "DbTool.exe"; DestDir: "{app}"
Source: "*.config"; DestDir: "{app}"
Source: "hibernate.cfg.xml"; DestDir: "{app}"
Source: "*.license.txt"; DestDir: "{app}"
Source: "*.dll"; DestDir: "{app}"

[Dirs]
Name: "{app}\"; Permissions: users-full

[UninstallDelete]
Type: files; Name: "{app}\*.*"
Type: dirifempty; Name: "{app}"

[Code]
function InitializeSetup(): Boolean;
begin
  if RegKeyExists(HKEY_LOCAL_MACHINE, 'Software\WMediacatalog') then
  begin
    MsgBox('Previous version detected. Please uninstall previous version of WMediacatalog before continue', mbInformation, MB_OK);
    result := false;
  end
  else
    result := true;
end;

