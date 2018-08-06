# V2RayGCon  
V2Ray GUI for windows.  
V2Ray的windows图形界面。  
  
（需要.net 4.0框架）  
  
### 主要功能  
编辑 config.json  
生成/扫描vmess/v2ray二维码  
导入/导出vmess/v2ray链接  
（注：v2ray链接是本软件直接将整个config.json用base64编码而成，导入后要细心检查）  
  
### 用法  
  
详细用法请看 [wiki](https://github.com/nobody3u/V2RayGCon/wiki)  
  
下载解压[Release](https://github.com/nobody3u/V2RayGCon/releases)中的V2RayGcon.zip    
双击V2RayGCon.exe，注意系统托盘图标。  
首次运行要下载v2ray-core，可用托盘图标的下载菜单自动下载。  
网络不好的可以手动下载，然后将v2ray-core所有文件复制过来。  
左键点击托盘图标弹出主界面。  
  
可以通过以下几种方式添加服务器：  
 1. 点[操作]->[添加vmess服务器]  
 2. 点[窗口]->[配置编辑器]  
 3. 托盘菜单->扫描二维码  
 4. 托盘菜单->剪切板导入   
   
双击服务器启用相应服务器。  
首次使用建议打开日志窗口，查看输出信息。  
  
### 截图 screenshot  
系统托盘图标  
![systray.png](https://raw.githubusercontent.com/nobody3u/V2RayGCon/master/screenshot/systray.png)  

下载v2ray-core  
![downloader.png](https://raw.githubusercontent.com/nobody3u/V2RayGCon/master/screenshot/downloader.png)  
  
主界面  
![mainform.png](https://raw.githubusercontent.com/nobody3u/V2RayGCon/master/screenshot/mainform.png)  

简易添加vmess服务器窗口  
![addvmessclient.png](https://raw.githubusercontent.com/nobody3u/V2RayGCon/master/screenshot/addvmessclient.png)  
  
配置编辑器  
完整模式  
![configeditor.png](https://raw.githubusercontent.com/nobody3u/V2RayGCon/master/screenshot/configeditor.png)  
精简模式  
![configeditor_min.png](https://raw.githubusercontent.com/nobody3u/V2RayGCon/master/screenshot/configeditor_min.png)  
  
二维码生成器  
![qrcode.png](https://raw.githubusercontent.com/nobody3u/V2RayGCon/master/screenshot/qrcode.png)  
