@echo off
rem 安装包路径
set install=CityMaker_Automesh_v1.1.170822_trunk.exe

rem 测试工程名
set testProject=test01.amp

rem 测试数据
set testDIR=\testDATA\

rem AM安装目录
set master="C:\Program Files\CityMaker 8\CityMaker Automesh\CityMakerAutomesh.exe"
set client="C:\Program Files\CityMaker 8\CityMaker Automesh\DCMonitor.exe"

rem 文件名
set setupiss=setup.iss
set unsetupiss=upinstall.iss
set txt=5.log

rem 文件路径
rem %cd%代表的是当前工作目录（current working directory，variable）
rem %~dp0代表的是当前批处理文件所在完整目录（the batch file's directory，fixed）
:: 			   %~dp0 %setupiss%
set installiss=%~dp0%setupiss%
set uninstalliss=%~dp0%unsetupiss%
set log=%~dp0%txt%
::		%cd%  %testDIR%  %testProject%
set proPath=%cd%%testDIR%%testProject%

cls
@echo 正在卸载...
start /wait %install% /s -f1%uninstalliss% ::-f2%log%

@echo 卸载完成，开始安装...
start /wait %install% /s -f1%installiss% ::-f2%log%

@echo 安装完成，启动程序...
start /min "" %client%
start /max "" %master% test %proPath%