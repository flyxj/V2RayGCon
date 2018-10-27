[![Build Status][1]][2] [![Release][7]][8] [![Total Downloads][3]][4] [![License][5]][6]

[1]: https://dev.azure.com/wgu6ymoma/V2RayGCon/_apis/build/status/nobody3u.V2RayGCon "Build Status Badge"
[2]: https://dev.azure.com/wgu6ymoma/V2RayGCon/_build/latest?definitionId=3 "Azure Build Status"
[3]: https://img.shields.io/github/downloads/nobody3u/V2RayGCon/total.svg "Total Downloads Badge"
[4]: http://www.somsubhra.com/github-release-stats/?username=nobody3u&repository=V2RayGCon "Downloads Detail"
[5]: https://img.shields.io/github/license/nobody3u/V2RayGCon.svg "Licence Badge"
[6]: https://github.com/nobody3u/V2RayGCon/blob/master/LICENSE "Licence"
[7]: https://img.shields.io/github/release/nobody3u/V2RayGCon.svg "Release Badge"
[8]: https://github.com/nobody3u/V2RayGCon/releases/latest "Releases"

V2RayGCon是windows平台下的v2ray-core图形配置程序  

### 主要用途  
* 编辑配置`config.json`  
* 启动多个`v2ray-core`  
* 导入/导出[vmess](https://github.com/2dust/v2rayN/wiki/%E5%88%86%E4%BA%AB%E9%93%BE%E6%8E%A5%E6%A0%BC%E5%BC%8F%E8%AF%B4%E6%98%8E(ver-2))/[v2ray](https://github.com/nobody3u/V2RayGCon/wiki/%E5%85%B3%E4%BA%8EV2RayGCon#v2ray%E9%93%BE%E6%8E%A5%E6%98%AF%E4%BB%80%E4%B9%88%E9%AC%BC)链接  
* 生成/扫描vmess/v2ray二维码  
  
### 简要使用说明  
  
（详细用法请看 [wiki](https://github.com/nobody3u/V2RayGCon/wiki)）  
  
下载解压[Release](https://github.com/nobody3u/V2RayGCon/releases)中的V2RayGCon.zip到任意目录  
然后通过本软件的下载窗口下载v2ray-core  
或手动下载再解压到本软件目录内    
  
按需选用下列方式添加配置：  
 1. 托盘菜单->`扫描二维码`  
 2. 托盘菜单->`剪切板导入`  
 3. 托盘菜单->`主窗口`->`文件`->`添加vmess客户端`  
 4. 托盘菜单->`主窗口`->`窗口`->`配置编辑器`  
    
添加配置后，可在主窗口中启用相应配置  
  
首次使用请打开日志窗口，查看输出信息排查错误  

### PAC说明
本软件使用的是txthinking的PAC,特此感谢  
Github地址：[https://github.com/txthinking/pac](https://github.com/txthinking/pac)  

### 设计目标
 1. 保留用户对v2ray-core的操控能力  
 2. 简化配置过程  
 3. 尽量使用v2ray-core的原生功能  
  
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

