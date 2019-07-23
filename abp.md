# ABP 相关记录
## 参考url:https://docs.abp.io/zh-Hans/abp/dev/

## 安装tools
```
dotnet tool install -g Volo.Abp.Cli

dotnet tool update -g Volo.Abp.Cli
```
## 新增项目
### abp new <解决方案名称> [options]

#### Options

* `--template` 或 `-t`: 指定模板. **默认的模板是 `mvc`**.可用的模板有:
  * `mvc` (默认): ASP.NET Core [MVC应用程序模板](Startup-Templates/Mvc.md). 其他选项:
    * `--database-provider` 或 `-d`: 指定数据库提供程序. 默认提供程序是 `ef`. 可用的提供程序有:
      * `ef`: Entity Framework Core.
      * `mongodb`: MongoDB.
    * `--tiered`: 创建分层解决方案,Web和Http Api层在物理上是分开的. 如果未指定会创建一个分层的解决方案, 此解决方案没有那么复杂,适合大多数场景.
  *  **`mvc-module`**: ASP.NET Core [MVC模块模板](Startup-Templates/Mvc-Module.md). 其他选项:
    * `--no-ui`: 不包含UI. 仅创建服务模块 (也称为微服务 - 没有UI).
* `--output-folder` 或 **`-o`**: 指定输出文件夹,默认是当前目录.

```
abp new abpDemo -t mvc -o apbDemo
abp new abpDemo -t mvc-module -o apbDemo
```

## 本地缓存模板文件
* `.abp\templates`
   * `mvc-0.18.1.zip`
   * `mvc-module-0.18.1.zip`
* **可在官方源码中把**`abp\templates`**中对应模块，压缩为zip文件**，**重命名为对应版本缓存**

