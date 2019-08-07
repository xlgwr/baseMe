# Plsql:
* （procedural language/sql）是oracle在标准的sql语言上的扩展。
*  Plsql不仅允许嵌入sql语言，还可以定义变量和常量，允许使用条件语句和循环语句，
*  允许使用例外处理各种错误，这样是的使得它的功能边的强大。
* 使用plsql语句块网络上只要发送一次plsql块就可以完成所有的sql语句的数据处理操作 大大减少了网络开销

# 基础
* 开发人员使用pl/sql编写应用模块时，不仅需要掌握sql语句的编写方法，
* 还要掌握pl/sql语句及语法规则，pl/sql编程使用变量和控制语句，从而可以编写非常有用的功能模块。
* 例如：分页存储过程模块，订单处理存储过程模块，转账过程模块，而且如果使用pl/sql编程，
* 可以轻松完成复杂的查询要求。

## 简单分类
* 块（过程【存储过程】，函数，触发器，包）

## 编写规范
* 注释

* 单行注释 –
* Select * from  emp  where empno=7788;--取得员工信息

* 多行注释
* /*…..*/

* 标识符命名规范
* 定义变量： v_为前缀   v_user

* 定义常量： c_为前缀   c_rate

* 定义游标：  _cursor后缀 emp_cursor

* 定义例外：  e_前缀   e_error

# 使用%TYPE
* 定义一个变量，其数据类型与已经定义的某个数据变量的类型相同，
* 或者与数据库表的某个列的数据类型相同，这时可以使用%TYPE。

* 使用%TYPE特性的优点在于：
 * 所引用的数据库列的数据类型可以不必知道；
 * 所引用的数据库列的数据类型可以实时改变。

```
DECLARE
   -- 用 %TYPE 类型定义与表相配的字段
   TYPE t_Record IS RECORD(

          T_no emp.empno%TYPE,

          T_name emp.ename%TYPE,

          T_sal emp.sal%TYPE );

   -- 声明接收数据的变量

   v_emp t_Record;

BEGIN

   SELECT empno, ename, sal INTO v_emp FROM emp WHERE empno=7788;

   DBMS_OUTPUT.PUT_LINE

(TO_CHAR(v_emp.t_no)||v_emp.t_name||TO_CHAR(v_emp.t_sal));

END;
```
## 使用%ROWTYPE
* PL/SQL 提供%ROWTYPE操作符, 返回一个记录类型, 其数据类型和数据库表的数据结构相一致。

* 使用%ROWTYPE特性的优点在于：
 * 所引用的数据库中列的个数和数据类型可以不必知道；
 * 所引用的数据库中列的个数和数据类型可以实时改变。
 ```
 DECLARE

    v_empno emp.empno%TYPE :=&no;
    rec emp%ROWTYPE;

BEGIN

    SELECT * INTO rec FROM emp WHERE empno=v_empno;
    DBMS_OUTPUT.PUT_LINE('姓名:'||rec.ename||'工资:'||rec.sal||'工作时间:'||rec.hiredate);

END;
 ```


## 块的介绍
* 块（block）是pl/sql的基本程序单元，编写pl/sql程序实际上是编写pl/sql块，要完成相对简单的应用功能，
* 可能只需要编写一个pl/sql块，但是要实现复杂的功能的时候就需要在一个pl/sql块中嵌套其他的pl/sql块
 
-- 解锁用户： 
alter user scott account unlock; 
---切换用户：
connect scott/tiger 
--连接远程的服务器： 
connect scott/tiger@orcl
--查看错误信息： 
show error；
```
## 调用过程：

* Exec 过程名（参数1，参数2…..）

* Call  过程名（参数1，参数2….）

* 过程成功的话，就是插入成功。

## 编写一个存储过程，该过程可以向表中添加记录
* 块的示意图
* Pl/sql块有三个部分构成：定义部分，执行部分，例外处理部分。
```
```
Declear

/*定义部分(定义常量，变量，游标，例外，复杂数据类型) 这个部分可选

Begin

/*执行部分(要执行的pl/sql语句和sql语句)这个部分是必须要的

Exception

/*例外处理部分（处理运行的各种错误）这个部分是可选

End；
```
* 存储过程样例
```
create or replace procedure hfc_a1 is //这里的or replace的作用就是存在的话就覆盖，没有的话就创建

 begin

--执行

insert into hfc valuses (123,’hfc’,’男’); //这个存储过程的作用就是向hfc这个表中添加数据

end;

/     (代表数据库去进行去执行) 这个斜杠不能省略

--结束
```
