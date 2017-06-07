@echo off
@echo --------------------------- find serch

::---查看目标主机名及连接状态
	::ping -a 192.168.0.102
	:: -a 显示主机名
	
::---测试连接是否成功
	::net time \\192.168.0.102
	
::---查看局域网在线主机上的共享
	::net view \\192.168.0.102
	
::---查看本地共享
	::net share
	
::---查看端口
	::netstat -an (-v,正在活动的)
	
::---查看最近有哪些用户登陆
	::finger @192.168.0.102
	
::---查看网络地址、网关
	::route print
	
::---追踪主机
	::tracert 192.168.0.2

::---主机名、MAC、工作组
	::nbtstat -A 192.168.0.102

@echo -------------------------- to do samething
	
::---连接主机
	::net use \\192.168.0.102\ipc$ "sd4964403" /user:"batu"
	::net use \\192.168.0.102 /del 删除链接 
	
::---映射主机盘符到本地
	::net use Z: \\192.168.0.102\c$
	::net use Z: /del 
	
::---开启ipc$共享 
	::net share ipc$ 
	::net share QQ=c:\tencent
	::删除ipc$共享
	::net share ipc$ /del 
	
::---管理主机用户

	::建立一个名为tmeUser，密码为12345的用户
	::net user tmeUser 12345 /add 
	
	::删除名为tmeUser的用户
	::net user tmeUser /del 
	
	::禁用tmeUser
	::net user tmeUser /active:no（yes）
	
	::把hacker这个用户添加进管理员组，即提升hacker的权限，使之变为管理员
	::net localgroup administrators tmeUser /add 
	
	
@echo OK!!
pause