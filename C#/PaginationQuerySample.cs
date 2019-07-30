 public async Task<JsonResult> Index(PaginationQuery pQuery)
        {
            int total = 0;

            string where = "";
            string likeValue = pQuery.where;
            var valuelist = pQuery.wherelist;
            var strSQlb = new StringBuilder();        
            int firstFalg = 0;
            if (valuelist.Count > 0)
            {
                foreach (var item in valuelist)
                {
                    int hasAnd = 0;
                    if (likeValue.isNull() && firstFalg == 0)
                    {
                        hasAnd = 0;
                    }
                    else
                    {
                        hasAnd = item.andorFlag <= 0 ? 1 : item.andorFlag;
                    }

                    switch (item.andLikeFlag)
                    {
                        case 3:
                            strSQlb.AppendLine(item.colName.toAndOrSql(item.colValue, hasAnd));
                            break;
                        default:
                            strSQlb.AppendLine(item.colName.toLikeSql(item.colValue, hasAnd, item.andLikeFlag));
                            break;
                    }
                    firstFalg++;
                }

            }

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
