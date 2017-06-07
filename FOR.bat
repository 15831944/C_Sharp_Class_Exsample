@echo off 

	::Setlocal Enabledelayedexpansion

@echo ----------------------------------------- 1  (array)
	::遍历集合
	::for %%i in (a b v) do @echo %%i
	
	::set ff=aa bb cc ee
	::for %%i in (%ff%) do echo %%i

@echo ----------------------------------------- 2 for /l (List)

	::从0开始，每次加1，加到不超过5
	::for /l %%i in (0 1 5) do @echo %%i	
	
	::从88开始，每次加减8，减到不超过20
	::for /l %%i in (88 -8 20) do @echo %%i
	
@echo ----------------------------------------- 3 for /f (files)

goto note1
	for /f 的几种形式,
	1、for /f %%i in (a.txt) do (……)
　　2、for /f %%i in ('dir d:*.txt') do (……)
　　3、for /f %%i in ("aa bb cc") do (……)
　　4、for /f "usebackq" %%i in ("文件名") do (……)
　　5、for /f "usebackq" %%i in (`命令语句`) do (……)
　　6、for /f "usebackq" %%i in ('字符串') do (……)

	delims=,.; 定义分隔符,默认是空格和分号
	
	tokens=1,3 获取被分开的第1，3两段
	tokens=1,2-5 获取被分开的第1，2到5五段,*为后面所有且不会分段
	
	skip=1 跳过第1行，从第2行开始
	
	eol=[	不读取以[开头的行，eol只能指定一个字符，默认为;
	
	usebackq 可以理解为上档键开关(shift),当开启时输入参数的写法会有变化，如4，5，6
	叫作增强型参数 是为了应对文件名中有空格时产生的冲突，其它是“文件名”符号改变引起的蝴蝶效应。
	for /f "usebackq" %%i in ("test 1.txt") do echo %%i
	
:note1

	::for /f "delims=, tokens=1-4 skip=1 eol=[" %%i in (a.txt) do (
	::echo %%i %%j %%k %%l %%m 
	)
@echo ----------------------------------------- 4 for /r (files)
goto note2

	根据条件搜索目录，如果不指定 “目录” 则在当前文件内执行
	for /r 目录 %%i in (元素集合) do 命令语句集合
	查不到隐藏文件

:note2

	for /r D:\work\China_Gis %%i in (*.shp *.prj) do echo %%i

	
@echo\
pause>nul