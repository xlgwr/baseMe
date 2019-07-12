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
