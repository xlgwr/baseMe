# dotnet core 发布到IIS
* 安装 Install the .NET Core Hosting Bundle
 * [下载地址.NET Core Hosting Bundle](https://www.microsoft.com/net/permalink/dotnetcore-current-windows-runtime-bundle-installer)
 * [参考URL](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-2.2#install-the-net-core-hosting-bundle)
# 记得重启IIS cmd 管理员
```
net stop was /y
net start w3svc
```

