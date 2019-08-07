# PLSQL的简介
* PL/SQL是Oracle数据库对SQL语句的扩展
# PLSQL的语法结构
* https://blog.csdn.net/qq_33404395/article/details/79912538
```
Declare 
      /*
       *声明部分--声明变量、常量、复杂数据类型、游标等
      */
BEGIN
      /*
       * 执行部分--PL/SQL语句和SQL语句
      */
EXCEPTION
      /*
      * 异常处理部分--处理运行错误
      */
END; --块结束标记
```
* 例子
```
SET SERVEROUTPUT ON
BEGIN
	--打印输出
    DBMS_OUTPUT.PUT_LINE('hello everyone!');
END;
 
```
** /*定义变量，查询工号7369的姓名、工资和入职时间。计算其所得税，最后打印出相关信息*/
```
DECLARE
    v_name  VARCHAR2(10);
    v_sal   NUMBER(7,2);
    v_hiredate  DATE;
    c_tax_rate  CONSTANT  NUMBER(3,2) := 0.02;
    v_tax_sal   NUMBER(7,2);
    v_valid     BOOLEAN  DEFAULT TRUE;
BEGIN
    SELECT ename,sal,hiredate
    INTO   v_name,v_sal,v_hiredate
    FROM   emp
    WHERE  empno = 7369;
    --计算所得税
    v_tax_sal:= v_sal * c_tax_rate;
    --打印输出
    DBMS_OUTPUT.PUT_LINE(v_name|| '的工资是：' || v_sal || ' 雇员日期是：' || v_hiredate || ' 所得税是：' || v_tax_sal);
    IF v_valid THEN
       DBMS_OUTPUT.PUT_LINE('已核实');
    END IF;
END;
```
*  PLSQL的引用型变量和记录型变量
```
--引用型变量
DECLARE
    v_name  emp.ename%TYPE;
    v_sal   emp.sal%TYPE;
BEGIN
	--给变量赋值
    SELECT ename,sal
    INTO   v_name,v_sal
    FROM   emp
    WHERE  empno = 7788;
    --打印输出
    DBMS_OUTPUT.PUT_LINE(v_name||'的工资是：'||v_sal);
END;
```
* 例子（记录型变量相当于一条完整的表记录，通过“变量名.列名”获取其值）：
```
--记录型变量
DECLARE
    emp_record   emp%ROWTYPE;
BEGIN
	--给变量赋值
    SELECT *  INTO emp_record FROM emp WHERE empno = 7788;
    --打印输出
    DBMS_OUTPUT.PUT_LINE(emp_record.ename||'的工资是：'||emp_record.sal);
END;

```
* PLSQL的运算符 
```
--算术运算符
DECLARE
    v_num1   NUMBER(3):=10;
    v_num2   NUMBER(3):=2;
BEGIN
    DBMS_OUTPUT.PUT_LINE(v_num1+v_num2);
    DBMS_OUTPUT.PUT_LINE(v_num1-v_num2);
    DBMS_OUTPUT.PUT_LINE(v_num1*v_num2);
    DBMS_OUTPUT.PUT_LINE(v_num1/v_num2);
    DBMS_OUTPUT.PUT_LINE(v_num1**v_num2);
END;
```
* --“ &n1(替代变量) ”运行程序时，会提示用户输入值n1
```
DECLARE
    v_num1  NUMBER(2) := &n1;
    v_num2  NUMBER(2) := &n2;
BEGIN
    IF (v_num1 = v_num2) THEN
          DBMS_OUTPUT.PUT_LINE('num1等于num2');
    ELSIF(v_num1 < v_num2) THEN
          DBMS_OUTPUT.PUT_LINE('num1小于num2');
    ELSIF(v_num1 > v_num2) THEN
          DBMS_OUTPUT.PUT_LINE('num1大于num2');
    END IF;
    
    IF (v_num1 <> v_num2) THEN
          DBMS_OUTPUT.PUT_LINE('num1不等于num2');
    END IF;
END;
```
* 比较运算符
```
--比较运算符
DECLARE
	--&n1是替代变量，在执行程序时会提示输入值
    v_num1  NUMBER(2):= &n1;
BEGIN
    IF(v_num1 BETWEEN 5 AND 10 )THEN
              DBMS_OUTPUT.PUT_LINE('num1在5到10之间');
    ELSE
              DBMS_OUTPUT.PUT_LINE('num1不在5到10之间');
    END IF;
    
    IF(v_num1 IN(3,8,10) )THEN
              DBMS_OUTPUT.PUT_LINE('num1等于3,8,10中的一个值');
    ELSE
              DBMS_OUTPUT.PUT_LINE('num1不等于3,8,10中的一个值');
    END IF;
    
    IF(v_num1 IS NULL)THEN
              DBMS_OUTPUT.PUT_LINE('num1为空');
    ELSE
              DBMS_OUTPUT.PUT_LINE('num1不为空');
    END IF;
END;
```
* 逻辑运算符
```
--逻辑运算符
DECLARE
    v_b1  BOOLEAN  := &n1;
    v_b2  BOOLEAN  := &n2;
BEGIN
    IF(v_b1 AND v_b2)THEN
            DBMS_OUTPUT.PUT_LINE('AND--TRUE');
    END IF;
    
    IF(v_b1 OR v_b2)THEN
            DBMS_OUTPUT.PUT_LINE('OR--TRUE');
    END IF;
    
    IF(NOT v_b1)THEN
            DBMS_OUTPUT.PUT_LINE('v_b1取反为TRUE');
    END IF;
END;
```
* PLSQL的条件控制语句--IF语句
```
--输入员工号，判断员工工资, 显示工资小于3000的员工姓名及工资。
--简单的IF语句
DECLARE
      v_name  emp.ename%TYPE;
      v_sal   emp.sal%TYPE;
BEGIN
      SELECT ename,sal
      INTO   v_name,v_sal
      FROM   emp
      WHERE  empno = &no;
      IF   v_sal <3000 THEN
           DBMS_OUTPUT.PUT_LINE(v_name||'的工资是：'||v_sal);
      END IF;
END;
```
```
--输入员工号，判断员工工资,将工资小于3000的员工工资涨200，并显示涨工资的员工姓名，其他员工显示员工姓名及工资。
--二重分支语句
DECLARE
       v_name    empnew.ename%TYPE;
       v_sal     empnew.sal%TYPE;
       v_empno   empnew.empno%TYPE := &no;
BEGIN
       SELECT ename,sal
       INTO   v_name,v_sal
       FROM   empnew
       WHERE  empno = v_empno;
       IF  v_sal <3000 THEN
           UPDATE empnew set sal = sal + 200 where empno = v_empno;
           COMMIT;
           DBMS_OUTPUT.put_line(v_name||'涨工资了');
       ELSE
           DBMS_OUTPUT.put_line(v_name||'的工资是：'||v_sal);
       END IF;
END;
```
```
--输入员工号，判断员工工资, 工资小于2000，显示低收入，工资小于6000，显示中等收入，其它显示高收入。
--多重分支语句
DECLARE
       v_name    empnew.ename%TYPE;
       v_sal     empnew.sal%TYPE;
       
BEGIN
       SELECT ename,sal
       INTO   v_name,v_sal
       FROM   empnew
       WHERE  empno = &no;
       IF v_sal<2000  THEN
          DBMS_OUTPUT.PUT_LINE(v_name||'的工资是：'||v_sal||' 属于低收入');
       ELSIF v_sal<6000  THEN
          DBMS_OUTPUT.PUT_LINE(v_name||'的工资是：'||v_sal||' 属于中等收入');
       ELSE
          DBMS_OUTPUT.PUT_LINE(v_name||'的工资是：'||v_sal||' 属于高收入');
       END IF;       
END;
```
* PLSQL的条件控制语句--CASE语句
```
--输入成级等级，判断属于哪个层次，并打印输出
--CASE 等值比较
DECLARE 
       v_grade char(1) := '&no';
BEGIN
       CASE v_grade
            WHEN  'A'  THEN
                  DBMS_OUTPUT.PUT_LINE('优秀');
            WHEN  'B'  THEN
                  DBMS_OUTPUT.PUT_LINE('中等');     
            WHEN  'C'  THEN
                  DBMS_OUTPUT.PUT_LINE('一般'); 
            ELSE
                  DBMS_OUTPUT.PUT_LINE('输入有误');    
       END CASE;              
END;
```
* 例子2
```
--输入员工号，获取员工工资，判断工资，如果工资小于1500，补助加100，如果工资小于2500，补助加80，如果工资小于5000，补助加50.
--CASE 非等值比较
DECLARE       
       v_sal     empnew.sal%TYPE;
       v_empno   empnew.empno%TYPE := &no;
BEGIN
       SELECT sal
       INTO   v_sal
       FROM   empnew
       WHERE  empno = &no;
       CASE 
              WHEN v_sal<1500 THEN
                   UPDATE empnew set comm = nvl(comm,0)+100 WHERE empno = v_empno;
              WHEN v_sal<2500 THEN
                   UPDATE empnew set comm = nvl(comm,0)+80 WHERE empno = v_empno;     
              WHEN v_sal<5000 THEN
                   UPDATE empnew set comm = nvl(comm,0)+50 WHERE empno = v_empno;       
              --COMMIT; 
       END CASE;
END;
```
* PLSQL的循环语句
```
--基本循环
DECLARE
    v_cnt INT :=1;
BEGIN
    LOOP 
          DBMS_OUTPUT.PUT_LINE(v_cnt);
          EXIT WHEN v_cnt = 10;
          v_cnt := v_cnt+1;          
    END LOOP;
END;
```
* while循环
```
--while循环
DECLARE
    v_cnt INT :=1;
BEGIN
    WHILE v_cnt<=10 LOOP
          DBMS_OUTPUT.PUT_LINE(v_cnt);
          v_cnt := v_cnt+1;
    END LOOP;
END;
```

* for循环
*  IN  下限 1   上限10   “REVERSE” 代表递减  如果去掉“REVERSE”的话，就是打印1,2,3,。。。10；
```
 
BEGIN
    FOR i  IN REVERSE 1..10 LOOP
        DBMS_OUTPUT.PUT_LINE(i);
    END LOOP;
END;
```
*  PLSQL的嵌套循环与退出语句
```
--嵌套循环
DECLARE
    v_result INT;
BEGIN
    <<outter>>
    FOR i IN 1..5 LOOP
        <<inter>>
        FOR j IN 1..5 LOOP
          v_result:=i;
          EXIT outter WHEN i=4;
        END LOOP inter;
        DBMS_OUTPUT.PUT_LINE('内'||v_result);
     END LOOP outter;
     DBMS_OUTPUT.PUT_LINE('外'||v_result);
END;
```

```
--CONTINUE
DECLARE
    v_cnt INT :=0;
BEGIN
    LOOP 
             v_cnt := v_cnt+1;
             CONTINUE WHEN v_cnt = 5;
             DBMS_OUTPUT.PUT_LINE(v_cnt);
             EXIT WHEN v_cnt = 10;     
    END LOOP;
END;
```

* PLSQL的顺序语句
```
--GOTO语句
DECLARE
  v_cnt INT := 1;
BEGIN
  LOOP
    DBMS_OUTPUT.PUT_LINE(v_cnt);
    IF v_cnt=10 THEN
       --EXIT;
       GOTO end_loop;
    END IF;
    v_cnt := v_cnt + 1;
   END LOOP;
   <<end_loop>>
   DBMS_OUTPUT.PUT_LINE('循环结束');
END;
```
* NULL语句
```
--NULL语句
DECLARE
   v_sal empnew.sal%TYPE;
   v_name empnew.ename%TYPE;
BEGIN
  SELECT ename,sal 
  INTO v_name,v_sal
  FROM empnew
  WHERE empno = &no;
  IF v_sal<3000 THEN
    UPDATE empnew set comm = nvl(comm,0)+sal*0.2 WHERE ename=v_name;
    COMMIT;
    DBMS_OUTPUT.PUT_LINE(v_name||'的奖金更新了');
  ELSE
    NULL;
  END IF;
END;
```


