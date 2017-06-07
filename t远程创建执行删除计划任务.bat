@echo off
::创建
schtasks /create /s 192.168.0.102 /u batu /p sd4964403 /sc once /st 00:00 /tn MyTest /tr D:\kankan\TCP-Server.exe
::执行
::schtasks /run /s 192.168.0.102 /u batu /p sd4964403 /tn MyTest
::删除 /f强删
::schtasks /delete /s 192.168.0.102 /u batu /p bt /tn MyTest /f