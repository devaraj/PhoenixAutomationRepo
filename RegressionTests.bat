             
@echo off	
set mydate=%date%
set mydate=%mydate: =_%
set mydate=%mydate:/=_%
	"C:\Program Files\Gallio\bin\Gallio.Echo.exe" "C:\Users\Administrator\Desktop\AutoNitro_LadbrokesPhoenix\AutoNitro\AutoNitro_LadbrokesPhoenix\_output\RegressionSuite.dll" "/rt:html" "/rnf:Regression_Report_%mydate%"
	