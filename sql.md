# MS SQL 分页
````sql
with oa as (
 select ROW_NUMBER() over (order by Id) as num, * 
 from [table] {0}
),
ob as (
 select * from oa
 where num between  {1} and {2}
)
SELECT * FROM ob;
````

## 使用语句查看一个存储过程的定义
```
EXEC sp_helptext  'Auth_BankCardAuthorize'
```
 

 

## 查询所有存储过程的名称以及定义
```

SELECT name, definition

FROM sys.sql_modules AS m

INNER JOIN sys.all_objects AS o ON m.object_id = o.object_id

WHERE o.[type] = 'P'
```
