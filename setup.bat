@echo off

cd %~dp0
powershell -Command "Invoke-WebRequest https://fox-gieg.com/patches/github/n1ckfg/TailingsPrototype3/Assets/Plugins.zip -OutFile Assets\Plugins.zip"
cd Assets
powershell Expand-Archive Plugins.zip -DestinationPath Plugins
del Plugins.zip

@pause

