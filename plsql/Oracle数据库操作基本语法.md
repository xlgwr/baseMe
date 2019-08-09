创建表

SQL>create table classes(

       classId number(2),

       cname varchar2(40)，

       birthday date

       );

添加一个字段

SQL>alter table student     add (classId number(2));

 

修改字段长度

SQL>alter table student modify(xm varchar2(30));

 

修改字段的类型/或是名字（不能有数据）

SQL＞alter table student modify(xm char(30));

 

删除一个字段

SQL>alter table student drop column sal;

 

修改表的名字

SQL>rename student to stu;

 

删除表

SQL>drop table student;

 

插入所有字段数据

SQL>insert into student values (‘001’,’salina’,’女’,’01-5月-05’，10)；

 

修改日期输入格式

SQL>alter session set nls_date_format = ‘yyyy-mm-dd’;       //临时生效，重启后不起错用

SQL>insert into student values (‘001’,’salina’,’女’,to_date(’01-5 -05’,’yyyy-mm-dd’)，10)；

SQL>insert into student values (‘001’,’salina’,’女’,to_date(’01/5 -05’,’yyyy/mm/dd’)，10)；

 

 

 

插入部分字段

SQL>insert into student (xh,xm,sex) values(‘001’,’lison’,’女’);

 

插入空值

SQL>insert into student (xh,xm,sex,birthday) values(‘021’,’BLYK’,’男’,null);

 

一条插入语句可以插入多行数据

SQL> insert into kkk (Myid,myname,mydept) select empno ,ename,deptno from emp where deptno=10;

 

 

查询空值/(非空)的数据

SQL>select * from student where brithday is null（/not null）;

 

修改（更新）数据

SQL＞update student set sal=sql/2 where sex =’男’；

 

更改多项数据

SQL> update emp set (job,sal,comm)=(select job,sal,comm from emp where ename='SMITH') where ename='SCOTT';

 

删除数据

1.       保存还原点
SQL>savepoint aa;

2.       删除数据
【1】      SQL>delete from student; //删除表的数据

【2】      SQL>drop table student;  //删除表的结构和数据

【3】      SQL>delete from student where xh=’001’; //删除一条记录

【4】      SQL>truncate table student;  //删除表中的所有记录，表结构还在，不写日志，无法扎找回的记录，速度快

 

查看表结构

SQL>desc student;

 

查询指定列

SQL>select sex,xh,xm from student;

 

如何取消重复

SQL>select distinct deptno,job from student;

 

打开显示操作时间的开关

SQL>set timing on;

 

 

为表格添加大的数据行（用于测试反应时间）

SQL>insert into users (userid,username,userpss) select * from users;

 

统计表内有多少条记录

SQL>select count(*) from users;

 

屏蔽列内相同数据

SQL>select distinct deptno,job from emp;

 

查询指定列的某个数据相关的数据

SQL＞select deptno,job,sal from emp where ename=’smith’;

 

使用算数表达式

SQL>select sal*12 from emp;

 

使用类的别名

SQL>select ename “姓名”，sal*12 as “年收入” from emp；

 

处理null(空)值

SQL>select sal*13+nvl(comm,0)*13 “年工资”，ename,comm from emp;

 

连接字符串（||）

SQL＞select ename || ‘is a’ || job from emp;

 

Where子句的使用

【1】SQL>select ename,sal from emp where sal>3000;   //number的范围确定

【2】SQL>select ename,hiredate from emp where hiredate>’1-1月-1982’; //日期格式的范围确定

【3】SQL>select ename,sal from emp where sal>=2000 and sal<=2500;      //组合条件

 

Like操作符：’%’、’_’

SQL>select ename,sal from emp where ename like ‘S%’;              //第一个字符【名字第一个字符为S的员工的信息（工资）】

SQL>select ename,sal from emp where ename like ‘__O%’;          //其它字符【名字第三个字符为O的员工的信息（工资）】

 

批量查询

SQL>select * from emp where in(123,456,789);       //查询一个条件的多个情况的批量处理

 

查询某个数据行的某列为空的数据的相关数据

SQL >select * from emp where mgr is null;

 

条件组合查询（与、或）

SQL>select * from emp where (sal>500 or job=’MANAGER’) and ename like ‘J%’;

 

Order by 排序

【1】SQL>select * from emp order by sal （asc）;       //从低到高[默认]

【2】SQL>select * from emp order by sal desc;      //从高到低

【3】SQL>select * from emp order by deptno (asc),sal  desc;     //组合排序

【4】SQL>select ename,sal*12 “年薪” from emp order by “年薪” (asc);

SQL> select ename,(sal+nvl(comm,0))*12 as "年薪" from emp order by "年薪";

 

资料分组（max、min、avg、sum、count）

SQL>select max(sal),min(sal) from emp;

SQL>select ename,sal from emp where sal=(select max(sal)  from emp); //子查询，组合查询

SQL> select * from emp where sal>(select avg(sal) from emp); //子查询，组合查询

SQL> update emp set sal=sal*1.1 where sal<(select avg(sal) from emp) and hiredate<'1-1月-1982';      //将工资小于平均工资并且入职年限早于1982-1-1的人工资增加10%

 

Group by 和 having 子句

//group by用于对查询出的数据进行分组统计

//having 用于限制分组显示结果

SQL>select avg(sal),max(sal),deptno from emp group by deptno;        //显示每个部门的平均工资和最低工资

SQL>select avg(sal),max(sal),deptno from emp group by deptno;        //显示每个部门的平均工资和最低工资

SQL> select avg (sal),max(sal),deptno from emp group by deptno having avg(sal)>2000;

SQL> select avg (sal),max(sal),deptno from emp group by deptno having avg(sal)>2000 order by  avg(sal);

 

多表查询

笛卡尔集:规定多表查询的条件是至少不能少于：表的个数-1

SQL> select a1.ename,a1.sal,a2.dname from emp a1,dept a2 where a1.deptno=a2.deptno;

SQL> select a1.dname,a2.ename,a2.sal from dept a1,emp a2 where a1.deptno=a2.deptno and a1.deptno=10;     //显示部门编号为10的部门名、员工名和工资

SQL> select a1.ename,a1.sal,a2.grade from emp a1,salgrade a2 where a1.sal between a2.losal and a2.hisal;

SQL> select a1.ename,a1.sal,a2.dname from emp a1,dept a2 where a1.deptno=a2.deptno order by a1.deptno;                   //多表排序

SQL> select worker.ename,boss.ename from emp worker,emp boss where worker.mgr=boss.empno;       //     自连接（多表查询的特殊情况）

SQL> select worker.ename,boss.ename from emp worker,emp boss where worker.mgr=boss.empno and worker.ename='FORD';

 

子查询

SQL> select * from emp where deptno=(select deptno from emp where ename='SMITH');

SQL> select distinct job from emp where deptno=10;

 

SQL> select * from emp where job in (select distinct job from emp where deptno=10);   

// 如何查询和部门10的工作相同的雇员的名字、岗位、工资、部门号。

SQL> select ename ,sal,deptno from emp where sal>all (select sal from emp where deptno=30);//如何查询工资比部门30的所有员工的工资高的员工的姓名、工资和部门号

SQL> select ename ,sal,deptno from emp where sal>(select max(sal) from emp where deptno=30);

SQL> select * from emp where (deptno,job)=(select deptno,job from emp where ename='SMITH');

 

内嵌视图

//当在from子句中使用子查询的时候，必须给子查询指定别名

SQL>select a2.ename,a2.sal,a2.deptno,a1.mysal from emp a2,(select deptno,avg(sal) (as ) mysal from emp group by deptno) a1 where a2.deptno=a1.deptno and a2.sal>a1.mysal;

 

 

分页

ＳＱＬ　＞select a1.*,rownum rn from (select * from emp) a1;//orcle为表分配的行号

SQL> select * from (select a1.*,rownum rn from (select * from emp) a1 where rownum<=10) where rn>=6;

//查询内容的变化

所有的改动（指定查询列）只需更改最里面的子查询
（排序）只需更改最里面的子查询
 

 

子查询（用查询结果创建新表）

SQL> create table mytable (id,name,sal,job,deptno) as select empno,ename,sal,job,deptno from emp;

 

合并查询

union(求并集), union  all  , intersect（取交集）,   minus （差集）

SQL> select ename,sal,job from emp where sal>2500;

SQL> select ename,sal,job from emp where job='MANAGER';

 

SQL> select ename,sal,job from emp where sal>2500 union select ename,sal,job from emp where job='MANAGER';    // union(求并集)

 

Java连接数据库

 

事务

SQL>commit;        //事务            （第一次创建，第二次提交）当退出数据库时，系统自动提交事务

SQL>savepoint a1;              //创建保存点                     （保存点的个数没有限制）

SQL>rollback to aa;    //使用保存点回滚到aa

SQL>rollback；           //回滚到事务创建开始

只读事务

SQL>set transaction read only

 

Java中的事务

Ct.setAutoCommit(false);    //设置事务自动提交为否

Ct.commit();          //提交事务

 

 

字符函数

lower(char)将字符串转换为小写的格式

upper(char)将字符串装换为大写的格式

length(char)返回字符串的长度

substr(char,m,n)取字符串的子串

 

SQL>select lower(ename) from emp;

SQL>select ename from emp where length(ename)=5；

                                                                                        
