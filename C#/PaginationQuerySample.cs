public string sqlIndex
{
    get
    {
        return @" 
        with allData as( 
            SELECT Id,* FROM XX  
        )
       ";
    }
}
public async Task<JsonResult> Index(PaginationQuery pQuery)
{
    int total = 0;

    string where = "";
    
    string likeValue = pQuery.where;    
    var strSQlb = new StringBuilder();    
    
     if (!likeValue.isNull())
    {
        strSQlb.AppendLine("xxxx".toLikeSql(likeValue, 0, 0));
        strSQlb.AppendLine("ddddd".toLikeSql(likeValue, 2, 0)); 
    } 
    
    var getAndSql = BaseComm.GetSqls(pQuery.wherelist, likeValue);
    strSQlb.AppendLine(getAndSql);

    where = strSQlb.ToString();

    var houseList = await BaseComm.GetPageEntitiessync<ZS_CheckInfoDto>(
        pQuery.pagesize,
        pQuery.pageindex,
        sqlIndex,
        where,
        "Id"
        );
    if (houseList.Count() > 0)
    {
        total = houseList.First().total;
    }
    PaginationData data = new PaginationData()
    {
        pageindex = pQuery.pageindex,
        pagesize = pQuery.pagesize,
        total = total,
        rows = houseList
    };
    JsonMessage msg = JsonHandler.CreateMessage((int)MsgTypeEnum.成功, "", data);

    return ToJsonResult(msg);
}
