
# 安装mssql
```
docker volume create mssqldata

docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=yourStrong(!)Password' -it --name sql2017fh --restart=always -p 1433:1433 -v mssqldata:/var/opt/mysql -v D:\DB\mssql:/tmp/hostDB -d mcr.microsoft.com/mssql/server:2017-latest
```

# 安装 memcached
 ```
 docker run --name my-memcache -it -p 11211:11211 --restart=always -d memcached
 ```
