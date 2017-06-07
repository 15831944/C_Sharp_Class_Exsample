@echo off 
@echo -------------------------------------  CALL 

::子程序与父程序在同一线程，输出内容在同一窗体

goto note1
::Call的标准用法

	echo startCall
	::call标签时必须带:，后面可以带参，传给标签以后的命令
	call:step1 hello

	::调用外部程序,只有当外部程序关闭后才会继续执行下一步
	call test02.bat sunProgram
	echo nextCall
	
	call textdata1.txt
	echo endCall

	exit

	:step1
	echo %1
	pause

:note1

goto note2
::CALL的扩展功能，变量延迟

	set n=3
	for /l %%i in (1 1 3) do (
		set /a n+=%%i
		::echo %n%
		call echo %%n%%
		)
:note2
::call用在for中
	for /l %%i in (0 1 9) do call :step2 %%i
	
	pause
	exit
	:step2
	set /a n=%1*%1
	echo %n%
