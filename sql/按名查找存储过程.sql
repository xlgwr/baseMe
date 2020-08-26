按名查找存储过程： 
EXEC Sp_HelpText '存储过程名'; 

按内容查找存储过程： 
select b.name 
from kbmp.dbo.syscomments a,kbmp.dbo.sysobjects b 
where a.id=b.id  and b.xtype='p' and a.text like '%if_check%';
