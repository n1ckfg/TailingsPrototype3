@echo off

set BASE_URL="https://fox-gieg.com/patches/github/n1ckfg/TailingsPrototype3/Assets"

cd %~dp0
powershell -Command "Invoke-WebRequest %BASE_URL%/Plugins1.zip -OutFile Assets\Plugins1.zip"
cd Assets
powershell Expand-Archive Plugins1.zip -DestinationPath Plugins
del Plugins1.zip

cd ..
powershell -Command "Invoke-WebRequest  %BASE_URL%/Plugins2.zip -OutFile Plugins2.zip"
powershell Expand-Archive Plugins2.zip -DestinationPath Assets
del Plugins2.zip

@pause

