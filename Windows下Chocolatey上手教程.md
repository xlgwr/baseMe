# Windows下Chocolatey上手教程

## 安装
* 安装 Chocolatey，只需要在 Windows 系统的命令行工具下面去执行一行命令（cmd），只
* 需要在其中的一个上面安装 Chocolatey 就可以了。你要用管理员的身份去运行命令行工具，
* 不然会遇到权限问题。终端下执行
```
Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
```
* 安装完成后，在命令行工具的下面，输入：
```
choco help
```
* 如果出现一些有用的帮助信息，比如 Chocolatey 的版本号，安装到的目录，相关的命令，还有示例等等，这就说明我们已经可以在系统上使用 Chocolatey 了。

## 更新
```
choco upgrade chocolatey
```

## 使用
* Chocolatey的使用也很简单，使用指令如下：
```
choco search <keyword>    搜索软件
choco list <keyword>  跟 search 命令功能类似
choco install <package1 package2 package3...>  安装软件
choco install <package>  -version *** 安装指定版本
choco  uninstall name 卸载软件
choco version <package>  查看安装包的版本情况
choco  upgrade <package>   更新某个软件 
choco list -localonly        查看一下所有安装在本地的包的列表
choco list -lo       功能同上
```
* 包的类型
  * 例：nodejs，git）
  * install （例：nodejs.install，git.install）
  * commandline（例：nodejs.commandline，未来会被抛弃）
  * portable（例：putty.portable）
* Chocolatey 的包有不同的类型，有些包的名字里面会包含特殊的后缀，
* 比如 .install ，.commandline，.portable ，有些包的名字不带这些后缀。
* 安装带 .install 后缀的包，这个包会出现在系统控制面板里的 卸载或更改程序 里面，
* 你可以把 .install 的包想成是通过安装程序（msi）安装的包。
---
* .commandline（未来会被抛弃） 与 .portable 后缀的包是压缩包（zip），
* 安装这种后缀的包，你不能在 卸载或更改程序 里找到它们。
---
* 你也可以选择不带后缀的包，这样如果系统中已经安装了这个包，
* 就会跳过去，如果没安装，Chocolatey 就会为你安装一个，默认安装的这个包的类型应该就是 .install 后缀的包。

