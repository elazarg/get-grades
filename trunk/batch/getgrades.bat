@echo off
set url="http://techmvs.technion.ac.il/cics/wmn/wmngrad"
set userid=%1
set password=%2
set data="function=signon&userid=%userid%&password=%password%"

\curl.exe --location "%url%?ORD=1" | for /F "skip=100 delims=?& tokens=2" %%i IN ('more') DO @\curl.exe --location -d%data% "%url%?%%i"