# MySQL

## docker
```
docker run --name mariadb -p 3306:3306 -e MYSQL_ROOT_PASSWORD=123456 -idt mariadb --character-set-server=utf8mb4 --collation-server=utf8mb4_unicode_ci
```
## 查看
show variables like 'character%';

## 新增数据库
```
create database MYDB character set utf8;
```

## 修改数据库的字符集
```
SET NAMES 'utf8';
SET character_set_client = utf8;
alter database name character set utf8;#修改数据库成utf8的.
alter table type character set utf8;#修改表用utf8.
alter table type modify type_name varchar(50) CHARACTER SET utf8;#修改字段用utf8
```
