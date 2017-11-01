SET directory=c:\backups\databases\
FOR /F "TOKENS=1,2 DELIMS=. " %%A IN ('DATE /T') DO SET mm=%%B 
FOR /F "TOKENS=2,3 DELIMS=. " %%A IN ('DATE /T') DO SET dd=%%B 
FOR /F "TOKENS=3* DELIMS=. " %%A IN ('DATE /T') DO SET yyyy=%%B 

set /A yyyy=%yyyy% 
set /A dd=%dd% 
set /A mm=%mm% 

set fileName=eurona_%yyyy%%dd%%mm%.bak
SET filePath=%directory%\%fileName%  


sqlcmd -S LWA-00135-73\SQLEXPRESS2008 -Q "BACKUP DATABASE eurona TO DISK = N'%filePath%' WITH INIT, NAME = N'Automatic back up of database', STATS = 1"

pause