--给N赋初值为30    
    declare @n int set @n=30     
        
    ;with maco as     
    (       
        select top (@n)    
            plan_handle,    
            sum(total_worker_time) as total_worker_time ,    
            sum(execution_count) as execution_count ,    
            count(1) as sql_count    
        from sys.dm_exec_query_stats group by plan_handle    
        order by sum(total_worker_time) desc    
    )    
    select  t.text ,    
            a.total_worker_time ,    
            a.execution_count ,    
            a.sql_count    
    from    maco a    
            cross apply sys.dm_exec_sql_text(plan_handle) t    
                
    /* 结果格式如下    
    text     total_worker_time  execution_count   sql_count    
    -------- ------------------ ----------------- ---------    
    内容略    
    */
    
    SELECT top 10    
    (total_elapsed_time / execution_count)/1000 N'平均时间ms'    
    ,total_elapsed_time/1000 N'总花费时间ms'    
    ,total_worker_time/1000 N'所用的CPU总时间ms'    
    ,total_physical_reads N'物理读取总次数'    
    ,total_logical_reads/execution_count N'每次逻辑读次数'    
    ,total_logical_reads N'逻辑读取总次数'    
    ,total_logical_writes N'逻辑写入总次数'    
    ,execution_count N'执行次数'    
    ,creation_time N'语句编译时间'    
    ,last_execution_time N'上次执行时间'    
    ,SUBSTRING(    
        st.text,     
        (qs.statement_start_offset/2) + 1,     
        (    
            (CASE statement_end_offset WHEN -1 THEN DATALENGTH(st.text) ELSE qs.statement_end_offset END - qs.statement_start_offset)/2    
        ) + 1    
    ) N'执行语句'    
    ,qp.query_plan    
FROM  sys.dm_exec_query_stats AS qs   
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) st   
CROSS APPLY sys.dm_exec_query_plan(qs.plan_handle) qp    
WHERE    
    SUBSTRING(    
        st.text,     
        (qs.statement_start_offset/2) + 1,    
        (    
            (CASE statement_end_offset WHEN -1 THEN DATALENGTH(st.text) ELSE qs.statement_end_offset END - qs.statement_start_offset)/2    
        ) + 1    
    ) not like '%fetch%'    
ORDER BY  total_elapsed_time / execution_count DESC;
