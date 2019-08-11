
# 安装mssql
```
docker volume create mssqldata

docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=yourStrong(!)Password' -it --name sql2017fh --restart=always -p 1433:1433 -v mssqldata:/var/opt/mysql -v D:\DB\mssql:/tmp/hostDB -d mcr.microsoft.com/mssql/server:2017-latest
```
---
# 安装Oracle

```
docker run -d -p 49161:1521 -p 8080:8080 -e ORACLE_ENABLE_XDB=true oracleinanutshell/oracle-xe-11g
```
## Login http://localhost:8080/apex/apex_admin with following credential:
* username: ADMIN
* password: admin

## Password for SYS & SYSTEM
* oracle

## Dockerfile
```
FROM oracleinanutshell/oracle-xe-11g
ADD init.sql /docker-entrypoint-initdb.d/
ADD script.sh /docker-entrypoint-initdb.d/
```
## Docker Compose
```
version: '3'

services: 
  oracle-db:
    image: oracleinanutshell/oracle-xe-11g:latest
    ports:
      - 1521:1521
      - 5500:5500
```
---

# 安装Redis
```
docker run -d --name myredis --restart=always -p 6379:6379 redis --requirepass "mypassword"
```
---
# 安装 memcached
 ```
 docker run --name my-memcache -it -p 11211:11211 --restart=always -d memcached
 ```
