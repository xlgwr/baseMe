with cte_trade as (
select a.name as TableName,b.name as ColumnsName,t.name as ColumnType,b.max_length
from  Trade.sys.sysobjects a left join  Trade.sys.columns b on a.id=b.object_id
				  left outer join Trade.sys.systypes t  on b.system_type_id=t.xtype 
where a.xtype='u' 
),
cte_trade_his as ( 
select a.name as TableName,b.name as ColumnsName,t.name as ColumnType,b.max_length,left(a.name,len(a.name)-4) as tradeTableName
from  Trade_His.sys.sysobjects a left join  Trade_His.sys.columns b on a.id=b.object_id
				  left outer join Trade_His.sys.systypes t  on b.system_type_id=t.xtype 
where a.xtype='u' 
)
select * from cte_trade_his a inner join cte_trade b on a.tradeTableName=b.TableName and a.ColumnsName=b.ColumnsName
where a.max_length<>b.max_length 
order by a.TableName;
