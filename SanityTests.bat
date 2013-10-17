             
@echo off	
set mydate=%date%
set mydate=%mydate: =_%
set mydate=%mydate:/=_%
for /l %%v in (1,1,8) do (
	ECHO %date%, %Time% --- Run%%v, The run will be rescheduled in 30 minutes,Please wait!!....."
	"C:\Program Files\Gallio\bin\Gallio.Echo.exe" "C:\Users\Administrator\Desktop\AutoNitro_LadbrokesPhoenix\AutoNitro\AutoNitro_LadbrokesPhoenix\_output\SanitySuite.dll" "/rt:html" "/rnf:Sanity_Report_%mydate%_Run%%v"
	TIMEOUT 1800	
)
