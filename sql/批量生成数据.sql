--1.测试合约10个随机
--2.测试客户2w
--3.测试委托10w，测试成交10w
--其中有成交客户4000，
--每个客户成交记录：10w/4000=25

--随机方法
CREATE or alter FUNCTION Scalar_CheckSumNEWIDQ  
(  
    @From int,  
    @To int,  
    @Keep int,  
    @RAND float 
)  
RETURNS float  
BEGIN    
    RETURN @From+round((@To-@From)*@RAND,@Keep)  
END  

GO

--select * from [ORDER] order by account_no;
--select * from [TRADE] order by account_no;

select count(1) from  [ORDER]
select count(1) from Trade


GO 

SET NOCOUNT on /* 关掉 (X 行受影响消息)*/

--SELECT dbo.Scalar_CheckSumNEWIDQ(20,30,0,RAND())

--生成总数+1,
--需要生成客户数
--**********************##############
--**********************##############
--客户
declare @AllMaxNumber_account int =20000
--委托
declare @AllMaxNumber_order int =10000*10
--成交
declare @AllMaxNumber_trade int =10000*10
--获取随机客户数
declare @getRandNum_account int = 4000;
--合约随机数
declare @getRandNum_contract int = 10;

--**********************##############
--**********************##############

--样例客户
declare @curr_account as varchar(10)='998266'
--生成下客户编号
declare @next_account as varchar(10)

--样例查询--取最大客户号，非字母
select @curr_account=MAX(account_no) from [dbo].[ACCOUNT] where account_no like '[0-9]%'

--获取客户
--生成2w客户
select * into #tmp_account from [dbo].[ACCOUNT] where account_no=@curr_account;

--记数-客户
declare @i_account int 
--剩下生成数据
declare @GenNumber_account int 


--表中已有记录数
declare @HasGenNumer_account int 


--已有数取值 
--客户
select @HasGenNumer_account=count(1) from [ACCOUNT]

--客户需要生成数
set @GenNumber_account=@AllMaxNumber_account-@HasGenNumer_account



print concat('需要生成客户条数：', @GenNumber_account,'样例账号：',@curr_account)


----################################
----###############需要生成客户#################
----################################
set @i_account=1
--大小
while @i_account<=@GenNumber_account
begin

set @next_account=@curr_account+@i_account
--
insert into [ACCOUNT]([account_no]
      ,[margin_class]
      ,[fee_class]
      ,[trading_status]
      ,[data_status]
      ,[trading_num_limit]
      ,[data_num_limit]
      ,[trading_pwd]
      ,[data_pwd]
      ,[commodity_limit_flag]
      ,[sms_flag]
      ,[meid])
select @next_account
      ,[margin_class]
      ,[fee_class]
      ,[trading_status]
      ,[data_status]
      ,[trading_num_limit]
      ,[data_num_limit]
      ,[trading_pwd]
      ,[data_pwd]
      ,[commodity_limit_flag]
      ,[sms_flag]
      ,[meid] from #tmp_account where [account_no]=@curr_account
--写入明细
INSERT INTO [dbo].[ACCOUNT_INFO]
           ([account_no]
           ,[account_email]
           ,[account_tel]
           ,[account_en_name]
           ,[account_cn_name]
           ,[account_id_no]
           ,[account_id_type]
           ,[open_date]
           ,[account_type]
           ,[account_birthday])
select  @next_account
           ,[account_email]
           ,[account_tel]
           ,[account_en_name]
           ,[account_cn_name]
           ,[account_id_no]
           ,[account_id_type]
           ,[open_date]
           ,[account_type]
           ,[account_birthday] 
from [dbo].[ACCOUNT_INFO]  where [account_no]=@curr_account

--写用户组别
insert into ACCOUNT_GROUP_AC([group_no],[account_no])
select [group_no],@next_account from [dbo].[ACCOUNT_GROUP_AC] where [account_no]=@curr_account

set @i_account=@i_account+1
end 

----################################
----################################
----################################
 
---获取合约
---###################
--合约10个随机
--declare @getRandNum_contract int = 10;
declare @getMaxCount_contract int =0;
declare @allCount_contract int
declare @curr_rand_contract int=0;
select ROW_NUMBER() over (order by [exchange_code],[commodity_code],[contract_code]) as num,* into #tmp_FUT_CONTRACT from FUT_CONTRACT;
select @allCount_contract=MAX(num) from #tmp_FUT_CONTRACT;
--
create table #tmp_getRand_contract(num int);

--生成10个随机数
while @getMaxCount_contract<=@getRandNum_contract
begin
 
	SELECT @curr_rand_contract=dbo.Scalar_CheckSumNEWIDQ(1,@allCount_contract,0,RAND()) 

	if(not exists( select 1 from #tmp_getRand_contract where num=@curr_rand_contract))
	begin 
	insert into #tmp_getRand_contract(num) values (@curr_rand_contract)
	end 

	select @getMaxCount_contract=COUNT(1) from #tmp_getRand_contract
end;
-- 
 
---客户4000
---###################
-- 随机生成
--declare @getRandNum_account int = 4000;
declare @getMaxCount_account int =0;
declare @allCount_account int
declare @curr_rand_account int=0;
select ROW_NUMBER() over (order by [account_no]) as num,* into #tmp_rand_ACCOUNT from ACCOUNT;
select @allCount_account=MAX(num) from #tmp_rand_ACCOUNT;
--
create table #tmp_getRand_account(num int);

--生成随机数
while @getMaxCount_account<=@getRandNum_account
begin
 
	SELECT @curr_rand_account=dbo.Scalar_CheckSumNEWIDQ(1,@allCount_account,0,RAND()) 

	if(not exists( select 1 from #tmp_getRand_account where num=@curr_rand_account))
	begin 
	insert into #tmp_getRand_account(num) values (@curr_rand_account)
	end 

	select @getMaxCount_account=COUNT(1) from #tmp_getRand_account
end;
--

---生成委托
---生成生交

declare @i_order int
declare @orderno int
declare @tradeno int

declare @maxOrder_order int =0
declare @maxOrder_trade int =0
declare @HasGenNumer_order int =0
declare @HasGenNumer_trade int =0
--生成数据
declare @GenNumber_order int 
declare @GenNumber_trade int 

select @maxOrder_order=isnull(MAX(order_no),0) from [ORDER] 
select @maxOrder_trade=isnull(MAX([trade_no]),0) from Trade 

--样例委托
select * from [ORDER] where order_no=@maxOrder_order;
select * from [TRADE] where [trade_no]=@maxOrder_trade; 

--已有数
select @HasGenNumer_order=count(1) from  [ORDER]
select @HasGenNumer_trade=count(1) from  [TRADE]


set @GenNumber_order=@AllMaxNumber_order-@HasGenNumer_order
set @GenNumber_trade=@AllMaxNumber_trade-@HasGenNumer_trade

select * into #tmpA_order from [ORDER] where order_no=@maxOrder_order
select * into #tmpA_trade from [TRADE] where [trade_no]=@maxOrder_trade

print concat('需要生成委托条数：', @GenNumber_order)
print concat('需要生成成交条数：', @GenNumber_trade)

set @i_order=1

declare @nosort_rand_account int ;
declare @nosort_rand_contract int ;
declare @curr_rand_account_no varchar(10);

declare @product_type char(1);
declare @exchange_code varchar(10);
declare @commodity_code varchar(10);
declare @contract_code varchar(50);

declare @buy_sell char(1)=0;

--初始化
set @curr_rand_account_no=@allCount_account
 
select @product_type='0',@exchange_code=exchange_code,
	   @commodity_code=commodity_code,@contract_code=contract_code 
from #tmpA_order where order_no=@maxOrder_order;
--
if @maxOrder_order<=0
begin 
 print concat('系统中没有委托记录',@maxOrder_order); 
 set @i_order=@GenNumber_order+1;
end;

--大小
while @i_order<=@GenNumber_order
begin
 
set @orderno=@maxOrder_order+@i_order
set @tradeno=@maxOrder_trade+@i_order

select @nosort_rand_account=dbo.Scalar_CheckSumNEWIDQ(1,@getRandNum_account,0,RAND());
set @nosort_rand_account=(@orderno+@nosort_rand_account)%@getRandNum_account

if(@nosort_rand_account<=0)
begin
 set @nosort_rand_account=@getRandNum_account;
end

set @nosort_rand_contract=(@orderno+@nosort_rand_account)%@getRandNum_contract;

if(@nosort_rand_contract<=0)
begin
	set @nosort_rand_contract=@getRandNum_contract
end;


--获取客户
with cte_rand as(
select ROW_NUMBER() over (order by num) as nosort ,num from #tmp_getRand_account
)
select @curr_rand_account_no=account_no from #tmp_rand_ACCOUNT where num = (select top 1 num from cte_rand where nosort=@nosort_rand_account)
;

--获取合约
with cte_rand as(
select ROW_NUMBER() over (order by num) as nosort ,num from #tmp_getRand_contract
)
select @product_type='0',@exchange_code=exchange_code,
	   @commodity_code=commodity_code,@contract_code=contract_code 
from #tmp_FUT_CONTRACT where num = (select top 1 num from cte_rand where nosort=@nosort_rand_contract)
;
--print concat('客户随之数：',@nosort_rand_account,':',@curr_rand_account_no); 
--print concat('合约随之数：',@nosort_rand_contract,':',@exchange_code,@commodity_code,@contract_code); 

--买卖方向
select @buy_sell=dbo.Scalar_CheckSumNEWIDQ(0,1,0,RAND());

--写入委托
insert into [ORDER]([init_date]
      ,[router_no]
      ,[operator_no]
      ,[order_person]
      ,[update_person]
      ,[proxy_id]
      ,[account_no]
      ,[product_type]
      ,[exchange_code]
      ,[commodity_code]
      ,[contract_code]
      ,[call_put]
      ,[strike_price]
      ,[open_close]
      ,[buy_sell]
      ,[order_price]
      ,[order_num]
      ,[order_type]
      ,[order_life]
      ,[order_insert_type]
      ,[del_tag]
      ,[order_from]
      ,[order_sub_num_min]
      ,[order_sub_num_max]
      ,[order_expiry_date]
      ,[trigger_price]
      ,[order_status]
      ,[filled_num]
      ,[order_date]
      ,[order_time]
      ,[update_date]
      ,[update_time]
      ,[exchange_date]
      ,[exchange_time]
      ,[order_remain]
      ,[order_no]
      ,[order_jour_no]
      ,[broker_order_no]
      ,[broker_order_jour_no]
      ,[exchange_order_no]
      ,[carry_no]
      ,[error_no]
      ,[error_info]
      ,[remark_cus]
      ,[info_s1]
      ,[info_s2]
      ,[local_no])
select [init_date]
      ,[router_no]
      ,[operator_no]
      ,[order_person]
      ,[update_person]
      ,[proxy_id]
      ,@curr_rand_account_no
      ,@product_type
      ,@exchange_code
      ,@commodity_code
      ,@contract_code
      ,[call_put]
      ,[strike_price]
      ,[open_close]
      ,@buy_sell
      ,[order_price]
      ,[order_num]
      ,[order_type]
      ,[order_life]
      ,[order_insert_type]
      ,[del_tag]
      ,[order_from]
      ,[order_sub_num_min]
      ,[order_sub_num_max]
      ,[order_expiry_date]
      ,[trigger_price]
      ,[order_status]
      ,[filled_num]
      ,[order_date]
      ,[order_time]
      ,[update_date]
      ,[update_time]
      ,[exchange_date]
      ,[exchange_time]
      ,[order_remain]
      ,@orderno
      ,[order_jour_no]
      ,[broker_order_no]
      ,[broker_order_jour_no]
      ,[exchange_order_no]
      ,[carry_no]
      ,[error_no]
      ,[error_info]
      ,[remark_cus]
      ,[info_s1]
      ,[info_s2]
      ,left(REPLACE(NEWID(),'-',''),20) from #tmpA_order where order_no=@maxOrder_order
--写入成交
insert into [TRADE]([init_date]
      ,[account_no]
      ,[order_date]
      ,[order_time]
      ,[order_no]
      ,[trade_date]
      ,[trade_no]
      ,[trade_time]
      ,[trade_price]
      ,[trade_vol]
      ,[currency_code]
      ,[is_tplusone]
      ,[open_close]
      ,[proxy_id]
      ,[broker_trade_no]
      ,[exchange_trade_no]
      ,[product_type]
      ,[exchange_code]
      ,[commodity_code]
      ,[contract_code]
      ,[buy_sell]
      ,[trade_fee]
      ,[operator_no]
      ,[info1]
      ,[info2]
      ,[info3]
      ,[order_insert_type]
      ,[order_from])
select [init_date]
      ,@curr_rand_account_no
      ,[order_date]
      ,[order_time]
      ,@orderno
      ,[trade_date]
      ,@tradeno
      ,[trade_time]
      ,[trade_price]
      ,[trade_vol]
      ,[currency_code]
      ,[is_tplusone]
      ,[open_close]
      ,[proxy_id]
      ,[broker_trade_no]
      ,[exchange_trade_no]
      ,@product_type
      ,@exchange_code
      ,@commodity_code
      ,@contract_code
      ,@buy_sell
      ,[trade_fee]
      ,[operator_no]
      ,[info1]
      ,[info2]
      ,[info3]
      ,[order_insert_type]
      ,[order_from] from #tmpA_trade where [trade_no]=@maxOrder_trade

set @i_order=@i_order+1
end


---
--select * from #tmp_account;
--select * from #tmp_FUT_CONTRACT;
select * from #tmp_getRand_contract;
select * from #tmp_getRand_account;

--查询数量
select count(1) from [ACCOUNT]
--select * from [ACCOUNT]

 
--清理临时表
drop table #tmp_account;
drop table #tmp_FUT_CONTRACT;
drop table #tmp_getRand_contract;
drop table #tmp_getRand_account;
drop table #tmp_rand_ACCOUNT;
drop table #tmpA_order;
drop table #tmpA_trade;
