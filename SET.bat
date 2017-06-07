@echo off 

@echo ----------------------------------- 1
goto note1

	拆分字符串，%变量：~启始位置,字符个数%
	set str=abcdefghigklmnopqrstuvwxyz
	
	从第0个开始，取2个
	set var=%str:~,2% 
	
	从第4个开始取后面所有
	set var=%str:~4%
	
	从结尾向前5位开始，取后面所有
	set var=%str:~-5%
	
	从第3位开始，取后面所有，再去掉后5位
	set var=%str:~3,-5%
	
	echo %var%
	
:note1

@echo ----------------------------------- 2
goto note2
	替换字符串，%变量：原字符串=新字符串% 
	新字符串为空时，效果为删除原字符串
	支持通配符 * ?
	set str=Go Back Home
	set var=%str:o=O%
	echo %str%
	echo %var%
	
	set str=I have a Pen,I have an Apple
	set var=%str:have=had%
	echo %str%
	echo %var%
:note2 
	
@echo ----------------------------------- 3
goto note3
	::设置整数变量，并计算,set /a 后面所引用的变量不需要加%
	set /a var1=-55 
	set /a var3=4
	set /a var5=var1+var3
	set /a var6=9*22
	echo %var5% %var6%
:note3

@echo ----------------------------------- 4
goto note4
	::接收用户输数
	set /p uu=setValue:
	set /a vv=%uu%
	pause>nul
	for /l %%i in (1 1 %vv%) do echo haha %%i

:note4
@echo\
pause>nul
