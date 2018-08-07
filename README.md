V2RayGCon是windows平台下的v2ray-core图形配置程序  

### 主要用途  
* 编辑/测试/启用`config.json`  
* 导入/导出[vmess](https://github.com/2dust/v2rayN/wiki/%E5%88%86%E4%BA%AB%E9%93%BE%E6%8E%A5%E6%A0%BC%E5%BC%8F%E8%AF%B4%E6%98%8E(ver-2))/[v2ray](https://github.com/nobody3u/V2RayGCon/wiki/%E5%85%B3%E4%BA%8EV2RayGCon#v2ray%E9%93%BE%E6%8E%A5%E6%98%AF%E4%BB%80%E4%B9%88%E9%AC%BC)链接  
* 生成/扫描vmess/v2ray二维码  
  
### 使用方法  
  
（详细用法请看 [wiki](https://github.com/nobody3u/V2RayGCon/wiki)）  
  
下载解压[Release](https://github.com/nobody3u/V2RayGCon/releases)中的V2RayGcon.zip到任意目录  
首次运行需下载[v2ray-core](https://github.com/v2ray/v2ray-core)，可用托盘图标的下载菜单自动下载  
网络不好的可手动下载，然后将解压出来的文件复制进V2RayGCon目录内  
  
可以通过以下几种方式添加配置：  
1. 托盘菜单->`扫描二维码`  
2. 托盘菜单->`剪切板导入`  
3. 托盘菜单->`主窗口`->`操作`->`添加vmess服务器`  
4. 托盘菜单->`主窗口`->`窗口`->`配置编辑器`  
  
添加配置后，在主窗口双击启用相应配置  
首次启用建议打开日志窗口，查看输出信息  
  
### 设计目标
1. 尽可能保留用户对v2ray-core的操控能力  
2. 尽可能简化配置过程  
3. 尽可能使用v2ray-core的原生功能  
  
### 截图  
系统托盘图标  
![systray.png](https://raw.githubusercontent.com/nobody3u/V2RayGCon/master/screenshot/systray.png)  

下载v2ray-core窗口  
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
