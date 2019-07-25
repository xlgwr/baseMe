# dotnet core 发布到IIS
* 安装 Install the .NET Core Hosting Bundle
 * [下载地址.NET Core Hosting Bundle](https://www.microsoft.com/net/permalink/dotnetcore-current-windows-runtime-bundle-installer)
 * [参考URL](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-2.2#install-the-net-core-hosting-bundle)
# 记得重启IIS cmd 管理员
```
net stop was /y
net start w3svc
```
# 跨域头
```
Response.AddHeader("Access-Control-Allow-Origin", "*");
Response.AddHeader("Access-Control-Allow-Headers", "*");
```
# 使用getAllResponseHeaders()看到的所有response header与实际在控制台 Network 中看到的 response header 不一样

# 使用getResponseHeader()获取某个 header 的值时，浏览器抛错Refused to get unsafe header "XXX"

* "simple response header"包括的 header 字段有：Cache-Control,Content-Language,Content-Type,Expires,Last-Modified,Pragma;
* "Access-Control-Expose-Headers"：
* 首先得注意是"Access-Control-Expose-Headers"进行跨域请求时响应头部中的一个字段，
* 对于同域请求，响应头部是没有这个* 字段的。这个字段中列举的 header 字段就是服务器允许暴露给客户端访问的字段。


