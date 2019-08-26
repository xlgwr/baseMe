# 资源uri的完整写法

```
// 0:项目名称
// 1:资源文件在项目中的路径
pack://application:,,,/{0};component/{1}
```

# 从后台获取编译方式为Resource的资源文件
```
System.Windows.Application.GetResourceStream(new Uri("pack://application:,,,/{0};component/{1}", UriKind.RelativeOrAbsolute)).Stream
```
