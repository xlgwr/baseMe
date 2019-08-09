--创建用户 
--语句结构
CREATE USER 用户名 IDENTIFIED BY 口令 [ACCOUNT LOCK|UNLOCK]

Create User xx IDentified by Lock|UNLOCK --默认锁定用户 不可登陆

Create User xx IDENTIFIED BY tom Account UNLOCK;

---------------------------------------------
--创建好的用户 授权
--语句结构

GRANT 权限 TO 用户

GRANT CONNECT TO xx; --

--权限说明
--CONNECT 临时用户 不可创建表 只可普通连接会话
--RESOURCE 可以创建 表 存储过程 触发器 索引。。。
--DBA 所以权限

--其他用户操作
--收回权限

REVOKE 权限 FROM 用户

--修改密码

ALTER USER 用户 IDENTIFIED BY 新的密码

--修改 锁定OR非锁定

ALTER USER ACCOUNT LOCK|UNLOCK

---------------------------------------------------------------

--数据库类型说明：

CHAR(length) --固定长度 最大2000字节

VARCHR2(Length)	--可变长度 最大4000字符

NUMBER(P,S)	--浮点数,p最大位数 默认36 ，s 小数位

DATE	--“年月日时分秒”从公元前 从公元前 4712年 1月 1日到 公元后 4712年 12 月 31 日

TIMESTAMP --长时间类型 包含 毫秒

CLOB --大数据文本类型

BLOB	--二进制


-------------------------------------------------------------------------
--建表语句

CREATE TABLE 表名
(
ID NUMBER(4) NOT NULL PRIMARY KEY, --在 Oracle 想让主键自增 不能直接写
--非常麻烦艹蛋的
STUID VARCHAR2(7) NOT NULL,	--在sql Server中 可以使用 IDENTIFY 设置默认值

..............	
)

--序列（创建自动增长或减少的整数）

--先创建一个 名为 customer_id_seq的序列 Increment by 序列类型 增量2 从1开始

create sequence customer_id_seq increment by 2 start with 1

--increment by 增长数 如果是正数为升序 ，如果为负数为降序

--START WITH 从某整数开始，升序默认1，降序默认-1


--查询语句
--结构
SELECT *|列名|表达式 FROM 表名 WHERE 条件 ORDER BY 列名

--克隆表结构并复制数据

CRATE TABLE newTable SELECT * from 表名 --如果 只需要结构不希望导入数据 输入不成立的条件即可

--插入语句
INSERT INTO 表名(列名1,列名2.....)VALUES(值1,值2........)

--插入结构集
INSERT INTO 表名 SELECT * FROM 表2;

--UPDATE 语句

UPDATE 表名 SET 列名1=值,列名2=值.... WHERE 条件

--DELETE 语句

DELETE FROM 表名 WHERE 条件

--TRUNCATE 语句 用于清空表数据（特性是比DELETE 快）

TRUNCATE TABLE 表名
------------------------------------------------
操作符

+、-、*、/四个，其中除号 /结果为浮点数 ，MOD(x,y)余数返回 x/y的余数

--****************** 字符串连接符 ||

SELECT ( 列名1 || 'is a ' || 列名2) as 新列名 form 表名

-------------取消重复行

SELECT * FROM 表 DEPTNO

-----集合运算

INTERSECT 交集

UNION ALL 并集

UNION 并集 --不包括重复行

MINUS 补集

---------------------------------------

-------------------连接查询

--在SQL Server 连接查询 inner join (内联查询)，outer join (外链) ，

--外链分左右 left outer join （左）, right outer join (右)

--在Oracle 中 表与表的外链连接用 “(+)” 表示

--内联查询

--1两表直接查询 变懒了不想写了

--2 * INNER JOIN * ON *

--INNER 可以不写

SELECT A.COL1,A.COL2,B.COL1 FROM table1 A INNER JOIN table2 B ON A.COL1=B.COL1 WHERE A.* > *

--外链

SELECT A.COL1,A.COL2,B.COL1 FROM table1 A ,table2 B WHERE A.COL3(+) = B.COL3

-- 说明（+） 在左 为 右外联节 ，在右为 左外链节

--学习过SQL Server 知道标准SQL语句 也可以使用标准写法

SELECT A.COL1,A.COL2,B.COL1 FROM table1 A RIGHT OUTER JOIN table2 B ON A.COL3 = B.COL3

--可以不写 OUTER

----------------------------------------------------
--子查询

--子查询分 单行返回 与多行返回

-- =,>,<,>=,<=,<> 比较符 用于单行返回

----------------------------------
--ORacle中的伪列 ROWID ROWNUM

-- SELECT ROWNUM, ROWID FROM xx

--在sql Server 通过函数 ROW_NUMBER() OVER() ?

-----------------------------------------------------
------------函数
--字符函数
ASCII(X)
CONCAT(X,Y) --连接2个字符串
INSTR(X,STR[,start][,n)	--查找字符串
Length(X)
LOWER(X)
UPPER(X)
LTRIM(X[,trim_str])	--截取x左边的字符串，缺省截取空格
RTRIM(x[,trim_str])
TRIM([trim_str FROM]X)	--艹 谁写的函数 不妨套路出牌
REPLACE(X,old,new)	--替换
SUBSTR(X,start[,length])-- 返回 staart-length 的字符串
----------数字函数
ABS(X)
ACOS(X)
COS(X)
CEIL(X) --大于或等于X最小值
FLOOR(X)	--小于或等于X最大值
LOG（X,Y）	--x为底Y的对数
MOD(X,y)	--
POWER(X,Y)	--幂
ROUND(x[,y])	--x的第y位四舍五入
SQRT(X)	--X平方根
TRUNC(x[,y]) --断裂

----------------日期函数
SYSDATE

ADD_MONTHS(D,N) 
LAST_DAY(D)	
EXTRACT(fmt from d) -- 获取日期
TO_DATE(x[,fmt])

-----转换函数
TO_CHAR(d|n[,fmt]) --日期或数字转为指定格式的字符串转

-----单行函数
NVL(x,value) --x为空返回value --SQL Server 不知道有木有 用标准语法还是可以的嘛
NVL2(x,value1,value2) x非空 返回value1 否则 value2

--聚合函数

AVG --平均值
SUM --求和
MIN,MAX
Count

---------------------------数据库对象

-------创建别名
CREATE [OR REPLACE] [PUBLIC] SYSNONYM [schema.]synonym_name
FOR [schema.]object_name

----------------视图
--语法
CREATE [OR REPLACE] [{FORCE|NOFORCE}] VIEW view_name
AS
SELECT查询
[WITH READ ONLY CONSTRAINT]

--解释：or replace 视图存在替换旧的

--force 表不存在也可以建立视图 但不能使用 只有基表创建完成后方可

--With read 可以通过视图对基本的添加修改操作

---------------------索引

Create [UNIQUE] Index INdex_name ON 
table_name(column_name[,column_name.....])

--解释：unique 指定索引列的值必须是唯一的
--column_name 可多列

---------------PL/SQL 
[DECLARE
--声明部分]
BEGIN 
--开始

[exception
--异常处理]
End;

------------

--------说几个特殊符号
:= 赋值
|| 以前见到过 连接符
..	集合(范围)
**	求冥 如：3**2=9

------------
DEALARE 
sname VARCHAR2(20):='jerry';
BEGIN
sname:=sname||' and tom';
dbms_output.put_line(sname);
end;
--dbms_output.put_line 输出


--属性 %ROWTYPE

DECLARE 
myemp EMP%ROWTYPE;
BEGIN
SELECT * INTO myemp FROM emp WHERE empno = '';
dbms_ootput.put_line(myem.ename);
END;


--if

if 条件 Then 
--执行
end if ;

----------------
if 条件 then

-->
else
-->
end if; -- if then elsif else end if;

--case

case when then end case;

--循环
无条件 循环 loop-end loop
whle 循环 whle 条件 loop --循环体 end loop;
for 循环 for 循环变量 in [循环下限] loop --循环体 end loop;
exit 强制跳出循环

---------------------------------------------------

--动态语句 好久都没用过了

EXECUTE IMMEDIATE 动态语句字符串
[INTO 变量列表 ]
[USING 参数列表 ]

----直接上代码

DECLARE

sql_stmt VARCHAR2(200);
emp_id NUMBER(4) := 7566;
salary NUMBER(7,2);
dept_id NUMBER(2) := 90;
dept_name VARCHAR2(14) := 'PERSONNEL';
location VARCHAR2(13) := 'DALLAS';
emp_rec emp%ROWTYPE;

BEGIN

--无子句的execute immediate
EXECUTE IMMEDIATE 'CREATE TABLE bonus1 (id NUMBER, amt NUMBER)';

----using子句的execute immediate
sql_stmt := 'INSERT INTO dept VALUES (:1, :2, :3)';
EXECUTE IMMEDIATE sql_stmt USING dept_id, dept_name, location;

----into子句的execute immediate
sql_stmt := 'SELECT * FROM emp WHERE empno = :id';
EXECUTE IMMEDIATE sql_stmt INTO emp_rec USING emp_id;

----returning into子句的execute immediate
sql_stmt := 'UPDATE emp SET sal = 2000 WHERE empno = :1
RETURNING sal INTO :2';
EXECUTE IMMEDIATE sql_stmt USING emp_id RETURNING INTO salary; 
EXECUTE IMMEDIATE 'DELETE FROM dept WHERE deptno = :num'
USING dept_id; ⑤
END;

创建表：
CREATE TABLE 表 (
ID Number primary KEY NOT NULL , --在主键这里不能指定 默认值
GUID char(32) not null, --其他列可以
GLGUID char(32) not null,
SORT Number(10) NOT null,
BZ CLOB 
);
CREATE SEQUENCE 名称 ; --创建表时 创建一个 序列 用着 ID

--修改列名称
ALTER TABLE 表 RENAME COLUMN 列名称 to 新名称

--删除列
ALTER TABLE 表 DROP COLUMN 列名称

 

插入语句：
INSERT INTO 表（ID,其他列） VALUES( 序列名称.NEXTVAL,其他值） --说明：ID并非自动增长 需要使用序列

--在插入数据时有date类型需要使用 to_date函数 to_date('值','yyyy-MM-dd HH24:mi:ss')

--我们插入了一条数据需要 返回ID
SELECT 序列名称.CURRVAL FROM DUAL --注: 执行多条语句 看 最后一段

--拷贝一个表的数据
INSERT INTO 表(列) SELECT 列 FROM 表2


查询语句：
--普通查询
SELECT COUNT(1) AS "COUNT" FROM 表 --在使用关键字的时候 为了 不让Oracle 识别错误我们可以使用" " 包裹

SELECT T2.GUID FROM 表2 T2 ,表1 T1 WHERE t1.GUID = T2.GUID--注意： 不要使用 表1 as T1 在oracle 不能这样

SELECT t1.ZDYW, t1.ZDZW ,T1.ZDLXZ FROM 表1 t1 JOIN 表2 T2 ON t1.A=T2.A

--ROW_NUMBER() OVER 与sqlserver 一致

SELECT 列 FROM ( SELECT ROW_NUMBER() OVER ( ID ) AS ROW_ID,其他列 FROM 表 WHERE 条件 ) T1 
WHERE T1.ROW_ID>= （页-1）*条 and T1.ROW_ID < 页*条

 

更新语句：
UPDATE 表名称 SET 列=值 WHERE 条件

--我们经常修改或者替换数据库数据
--这里 cast 是 转换类型 
--replace 替换值 参数1 原字符串 参数2 查找的值 参数3 修改的
update 表 set 列 = replace(cast(列 as varchar2(4000)),'查找的值','值') where ----;

--在实际应用中可能转换 全半角
to_single_byte(c) 转换成半角
to_multi_byte(c) 转换成全角

--需要给某列重新排序 用法与sqlserver 一致如下：
--WITH 在内存中创建临时表 T1
WITH T1 AS
(SELECT ROW_NUMBER() OVER (ORDER BY ID ASC ) as C1 , 表.* FROM 表 where 条件 ) 
UPDATE T1 SET 列 ='ABC' || TO_CHAR(C1) ||'ABC'; --ABC 前后坠

 

--在开发中我们为了减少访问次数 需要一次性提交多条数据


--begin　end 包裹 并且不能包含 select 语句
--看 oracle 存储过程详解

 

 

oracle 存储过程的基本语法

1.基本结构 
CREATE OR REPLACE PROCEDURE 存储过程名字
(
参数1 IN NUMBER,
参数2 IN NUMBER
) IS
变量1 INTEGER :=0;
变量2 DATE;
BEGIN

END 存储过程名字

2.SELECT INTO STATEMENT
将select查询的结果存入到变量中，可以同时将多个列存储多个变量中，必须有一条
记录，否则抛出异常(如果没有记录抛出NO_DATA_FOUND)
例子： 
BEGIN
SELECT col1,col2 into 变量1,变量2 FROM typestruct where xxx;
EXCEPTION
WHEN NO_DATA_FOUND THEN
xxxx;
END;
...

3.IF 判断
IF V_TEST=1 THEN
BEGIN 
do something
END;
END IF;

4.while 循环
WHILE V_TEST=1 LOOP
BEGIN
XXXX
END;
END LOOP;

5.变量赋值
V_TEST :=123;

6.用for in 使用cursor
...
IS
CURSOR cur IS SELECT * FROM xxx;
BEGIN
FOR cur_result in cur LOOP
BEGIN
V_SUM :=cur_result.列名1+cur_result.列名2
END;
END LOOP;
END;

7.带参数的cursor
CURSOR C_USER(C_ID NUMBER) IS SELECT NAME FROM USER WHERE TYPEID=C_ID;
OPEN C_USER(变量值);
LOOP
FETCH C_USER INTO V_NAME;
EXIT FETCH C_USER%NOTFOUND;
do something
END LOOP;
CLOSE C_USER;

8.用pl/sql developerdebug
连接数据库后建立一个Test WINDOW
在窗口输入调用SP的代码,F9开始debug,CTRL+N单步调试


Oracle存储过程总结
1、创建存储过程
create or replace procedure test(var_name_1 in type,var_name_2 out type) as
--声明变量(变量名 变量类型)
begin
--存储过程的执行体
end test;
打印出输入的时间信息
E.g:
create or replace procedure test(workDatein Date) is
begin
dbms_output.putline(The input date is:||to_date(workDate,yyyy-mm-dd));
end test;
2、变量赋值
变量名 := 值;
E.g：
create or replace procedure test(workDatein Date) is
x number(4,2);
begin
x := 1;
end test;
3、判断语句:
if 比较式 then begin end; end if;
E.g
create or replace procedure test(x innumber) is
begin
ifx >0 then
begin
x:= 0 - x;
end;
end if;
if x = 0 then
begin
x:= 1;
end;
end if;
end test;
4、For 循环
For ... in ... LOOP
--执行语句
end LOOP;
(1)循环遍历游标
create or replace procedure test() as
Cursor cursor is select name from student;
name varchar(20);
begin
for name in cursor LOOP
begin
dbms_output.putline(name); 
end;
end LOOP;
end test;
(2)循环遍历数组
create or replace proceduretest(varArray in myPackage.TestArray) as
--(输入参数varArray 是自定义的数组类型，定义方式见标题6)
i number;
begin
i := 1; --存储过程数组是起始位置是从1开始的，与java、C、C++等语言不同。因为在Oracle中本是没有数组的概念的，数组其实就是一张
--表(Table),每个数组元素就是表中的一个记录，所以遍历数组时就相当于从表中的第一条记录开始遍历
for i in 1..varArray.count LOOP 
dbms_output.putline(The No. || i ||recordin varArray is: ||varArray(i)); 
end LOOP;
end test;
5、While 循环
while 条件语句 LOOP
begin
end;
end LOOP;
E.g
create or replace procedure test(i innumber) as
begin
while i < 10 LOOP
begin 
i:= i + 1;
end;
end LOOP;
end test;
6、数组
首先明确一个概念：Oracle中本是没有数组的概念的，数组其实就是一张表(Table),每个数组元素就是表中的一个记录。
使用数组时，用户可以使用Oracle已经定义好的数组类型，或可根据自己的需要定义数组类型。
(1)使用Oracle自带的数组类型
x array; --使用时需要需要进行初始化
e.g:
create or replace procedure test(y outarray) is
x array; 
begin
x := new array();
y := x;
end test;
(2)自定义的数组类型 (自定义数据类型时，建议通过创建Package的方式实现，以便于管理)
E.g (自定义使用参见标题4.2) create or replace package myPackage is
-- Public type declarations type info is record( name varchar(20), ynumber);
type TestArray is table of infoindex by binary_integer; --此处声明了一个TestArray的类型数据，其实其为一张存储Info数据类型的Table而已，及TestArray 就是一张表，有两个字段，一个是
name，一个是y。需要注意的是此处使用了Index by binary_integer 编制该Table的索引项，也可以不写，直接写成：type TestArray is
table of info，如果不写的话使用数组时就需要进行初始化：varArray myPackage.TestArray; varArray:= new myPackage.TestArray();
end TestArray;
7.游标的使用 Oracle中Cursor是非常有用的，用于遍历临时表中的查询结果。其相关方法和属性也很多，现仅就常用的用法做一二介绍：
(1)Cursor型游标(不能用于参数传递)
create or replace procedure test()is 
cusor_1 Cursor is select std_name fromstudent where ...; --Cursor的使用方式1 cursor_2 Cursor;
begin
select class_name into cursor_2 from classwhere ...; --Cursor的使用方式2
可使用For x in cursor LOOP .... end LOOP; 来实现对Cursor的遍历
end test;
(2)SYS_REFCURSOR型游标，该游标是Oracle以预先定义的游标，可作出参数进行传递
create or replace procedure test(rsCursorout SYS_REFCURSOR) is
cursor SYS_REFCURSOR; name varhcar(20);
begin
OPEN cursor FOR select name from studentwhere ... --SYS_REFCURSOR只能通过OPEN方法来打开和赋值
LOOP

fetch cursor intoname --SYS_REFCURSOR只能通过fetch into来打开和遍历 exit whencursor%NOTFOUND; --SYS_REFCURSOR中可使用三个状态属性： ---%NOTFOUND(未找到记录信息) %FOUND(找到记录信息) ---%ROWCOUNT(然后当前游标所指向的行位置)
dbms_output.putline(name);
end LOOP;
rsCursor := cursor;
end test;
下面写一个简单的例子来对以上所说的存储过程的用法做一个应用：
现假设存在两张表，一张是学生成绩表(studnet)，字段为：stdId,math,article,language,music,sport,total,average,step 一张是学生课外成绩表(out_school),字段为:stdId,parctice,comment
通过存储过程自动计算出每位学生的总成绩和平均成绩，同时，如果学生在课外课程中获得的评价为A，就在总成绩上加20分。
create or replace procedureautocomputer(step in number) is
rsCursor SYS_REFCURSOR;
commentArray myPackage.myArray;
math number;
article number;
language number;
music number;
sport number;
total number;
average number;
stdId varchar(30);
record myPackage.stdInfo;
i number;
begin
i := 1;
get_comment(commentArray); --调用名为get_comment()的存储过程获取学生课外评分信息
OPEN rsCursor for selectstdId,math,article,language,music,sport from student t where t.step = step;
LOOP

fetch rsCursor intostdId,math,article,language,music,sport; exit when rsCursor%NOTFOUND;
total := math + article + language + music+ sport;
for i in 1..commentArray.count LOOP 
record :=commentArray(i); 
if stdId = record.stdId then 
begin 
if record.comment =&apos;A&apos; then 
begin 
total := total +20; 
go to next; --使用go to跳出for循环 
end; 
end if; 
end; 
end if;
end LOOP;
<<continue>> average :=total / 5;
update student t set t.total=totaland t.average = average where t.stdId = stdId;
end LOOP;
end;
end autocomputer;
--取得学生评论信息的存储过程
create or replace procedureget_comment(commentArray out myPackage.myArray) is
rs SYS_REFCURSOR；
record myPackage.stdInfo;
stdId varchar(30);
comment varchar(1);
i number;
begin
open rs for select stdId,comment fromout_school
i := 1;
LOOP

fetch rs into stdId,comment; exit whenrs%NOTFOUND;
record.stdId := stdId;
record.comment := comment;
recommentArray(i) := record;
i:=i + 1;
end LOOP;
end get_comment;
--定义数组类型myArray
create or replace package myPackage isbegin
type stdInfo is record(stdIdvarchar(30),comment varchar(1));
type myArray is table of stdInfo index bybinary_integer;
end myPackage;
。

 

 

关于oracle存储过程的若干问题备忘
1.在oracle中，数据表别名不能加as，如：

select a.appname from appinfo a;-- 正确
select a.appname from appinfo as a;-- 错误

也许，是怕和oracle中的存储过程中的关键字as冲突的问题吧

2.在存储过程中，select某一字段时，后面必须紧跟into，如果select整个记录，利用游标的话就另当别论了。

select af.keynode into kn from APPFOUNDATION af where af.appid=aid and af.foundationid=fid;-- 有into，正确编译
select af.keynode from APPFOUNDATION af where af.appid=aid and af.foundationid=fid;-- 没有into，编译报错，提示：Compilation 
Error: PLS-00428: an INTO clause is expected in this SELECT statement


3.在利用select...into...语法时，必须先确保数据库中有该条记录，否则会报出"no data found"异常。

可以在该语法之前，先利用select count(*) from 查看数据库中是否存在该记录，如果存在，再利用select...into...

4.在存储过程中，别名不能和字段名称相同，否则虽然编译可以通过，但在运行阶段会报错

select keynode into kn from APPFOUNDATION where appid=aid and foundationid=fid;-- 正确运行
select af.keynode into kn from APPFOUNDATION af where af.appid=appid and af.foundationid=foundationid;-- 运行阶段报错，提示
ORA-01422:exact fetch returns more than requested number of rows

5.在存储过程中，关于出现null的问题

假设有一个表A，定义如下：

create table A(
id varchar2(50) primary key not null,
vcount number(8) not null,
bid varchar2(50) not null -- 外键 
);

如果在存储过程中，使用如下语句：

select sum(vcount) into fcount from A where bid='xxxxxx';

如果A表中不存在bid="xxxxxx"的记录，则fcount=null(即使fcount定义时设置了默认值，如：fcountnumber(8):=0依然无效，fcount还是会变成null)，这样以后使用fcount时就可能有问题，所以在这里最好先判断一下：

if fcount is null then
fcount:=0;
end if;

这样就一切ok了。

6.Hibernate调用oracle存储过程

this.pnumberManager.getHibernateTemplate().execute(
new HibernateCallback() {
public Object doInHibernate(Session session)
throws HibernateException, SQLException {
CallableStatement cs = session
.connection()
.prepareCall("{call modifyapppnumber_remain(?)}");
cs.setString(1, foundationid);
cs.execute();
return null;
}
});

 

oracle 存储过程语法总结及练习
---------------------------------------------
--1.存储过程之if
clear;
create or replace procedure mydel(
in_a in integer)
as
begin
if in_a<100 then
dbms_output.put_line('小于100.');
elsif in_a<200 then
dbms_output.put_line('大于100小于200.');
else
dbms_output.put_line('大于200.');
end if;
end;
/

set serveroutput on;
begin
mydel(1102);
end;
/
---------------------------------------------

--2.存储过程之case1
clear;
create or replace procedure mydel(
in_a in integer)
as
begin
case in_a
when 1 then
dbms_output.put_line('小于100.');
when 2 then
dbms_output.put_line('大于100小于200.');
else
dbms_output.put_line('大于200.');
end case;
end;
/

set serveroutput on;
begin
mydel(2);
end;
/
------------------------------------------------

--1.存储过程之loop1
clear;
create or replace procedure mydel(
in_a in integer)
as
a integer;
begin
a:=0;
loop
dbms_output.put_line(a);
a:=a+1;
exit when
a>301;
end loop;
end;
/


set serveroutput on;
begin
mydel(2);
end;
/
--------------------------------------------------
--1.存储过程之loop2
clear;
create or replace procedure mydel(
in_a in integer)
as
a integer;
begin
a:=0;
while a<300 loop
dbms_output.put_line(a);
a:=a+1;
end loop;
end;
/


set serveroutput on;
begin
mydel(2);
end;
--------------------------------------------------
--1.存储过程之loop3
clear;
create or replace procedure mydel(
in_a in integer)
as
a integer;
begin
for a in 0..300
loop
dbms_output.put_line(a);
end loop;
end;
/


set serveroutput on;
begin
mydel(2);
end;
/
clear;
select ename,cc:=(case
when comm=null then sal*12;
else (sal+comm)*12;
end case from emp order by salpersal;

----------------------------------------------------
clear;
create or replace procedure getstudentcomments(
i_studentid in int,o_comments out varchar)
as
exams_sat int;
avg_mark int;
tmp_comments varchar(100);
begin
select count(examid) into exams_sat from studentexam
where studentid=i_studentid;
if exams_sat=0 then
tmp_comments:='n/a-this student did not attend the exam!';
else
select avg(mark) into avg_mark from studentexam
where studentid=i_studentid;
case
when avg_mark<50 then tmp_comments:='very bad';
when avg_mark<60 then tmp_comments:='bad';
when avg_mark<70 then tmp_comments:='good';
end case;
end if;
o_comments:=tmp_comments;
end;
/


set serveroutput on;
declare
pp studentexam.comments%type;
begin
getstudentcomments(8,pp);
dbms_output.put_line(pp);
end;
/
--------------------------------------------------------

delete from emp where empno<6000;
clear;
create or replace procedure insertdata(
in_num in integer)
as
myNum int default 0;
emp_no emp.empno%type:=1000;
begin
while myNum<in_num loop
insert into empvalues(emp_no,'hui'||myNum,'coder',7555,current_date,8000,6258,30);
emp_no:=emp_no+1;
myNum:=myNum+1;
end loop;
end;
/

set serveroutput on;
begin
insertdata(10);
end;
/
select * from emp;
