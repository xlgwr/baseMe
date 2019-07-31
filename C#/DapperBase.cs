namespace System
{
    public static class BaseComm
    {
        public static JsonMessage GetPageEntitiesDetails<T>(string sqlIndex, int Id)
            where T : IEntityPage
        {
            JsonMessage msg = JsonHandler.CreateMessage((int)MsgTypeEnum.成功, "");
            if (Id <= 0)
            {
                msg = JsonHandler.CreateMessage((int)MsgTypeEnum.错误, "Id 不正确。");
                return msg;
            }
            int total = 0;

            string where = "";
            string likeValue = Id.NullToStr();
            if (!likeValue.isNull())
            {
                var strSQlb = new StringBuilder();
                strSQlb.AppendLine("Id".toAndOrSql(likeValue, 0));
                where = strSQlb.ToString();
            }
            var data = BaseComm.GetPageEntities<T>(
                1,
                1,
                sqlIndex,
                where,
                out total,
                "Id"
                );
            msg = JsonHandler.CreateMessage((int)MsgTypeEnum.成功, "", data);

            return msg;
        }
        /// <summary>
        ///  {0} 样例
        ///  with allData as(
        ///  Select * from XXX
        ///  ) 
        /// </summary>
        public static IEnumerable<T> GetPageEntities<T>(int pageSize, int pageIndex, string sqlFirst, string where, out int total, string order = "Id")
            where T : IEntityPage
        {
            total = 0;
            where = !where.isNull() ? string.Format(" where {0} ", where) : "";
            string wherepage = string.Format("where num between  {0} and {1}", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);

            var allSql = new StringBuilder();
            allSql.AppendFormat(@"
            {0}
            , oa as (   
            select distinct ROW_NUMBER() over (order by {1}) as num,* from allData
            {2}
            ),
            allcount AS(
	            SELECT COUNT(1) total FROM oa
            ),
            ob as (
                select oa.* from oa 
                    {3}
            )
            SELECT * FROM ob,allcount x; 

            ", sqlFirst, order, where, wherepage);

            IEnumerable<T> viewList = GetData<T>(allSql.ToString());

            if (viewList.Count() > 0)
            {
                total = viewList.First().total;
            }

            return viewList;
        }
        public static Task<IEnumerable<T>> GetPageEntitiessync<T>(int pageSize, int pageIndex, string sqlFirst, string where, string order = "Id")
          where T : IEntityPage
        { 
            where = !where.isNull() ? string.Format(" where {0} ", where) : "";
            string wherepage = string.Format("where num between  {0} and {1}", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);

            var allSql = new StringBuilder();
            allSql.AppendFormat(@"
            {0}
            , oa as (   
            select distinct ROW_NUMBER() over (order by {1}) as num,* from allData
            {2}
            ),
            allcount AS(
	            SELECT COUNT(1) total FROM oa
            ),
            ob as (
                select oa.* from oa 
                    {3}
            )
            SELECT * FROM ob,allcount x; 

            ", sqlFirst, order, where, wherepage);

              return GetDatasync<T>(allSql.ToString()); 
          
        }

        public static IEnumerable<T> GetData<T>(string sql)
        {
            IEnumerable<T> viewList = DapperHelper.GetAppMall.Query<T>(sql);
            return viewList;
        }
        public static Task<IEnumerable<T>> GetDatasync<T>(string sql)
        {
            return DapperHelper.GetAppMall.QueryAsync<T>(sql);
        }
	public static string GetSqls(List<WhereList> valuelist, string likeValue)
        {
            int firstFalg = 0;
            var strSQlb = new StringBuilder();
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
                        case 0:
                        case 1:
                        case 2:
                            strSQlb.AppendLine(item.colName.toLikeSql(item.colValue, hasAnd, item.andLikeFlag));
                            break;
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            strSQlb.AppendLine(item.colName.toAndOrSql(item.colValue, hasAnd, item.andLikeFlag));
                            break;
                        default:
                            strSQlb.AppendLine(item.colName.toLikeSql(item.colValue, hasAnd, item.andLikeFlag));
                            break;
                    }
                    firstFalg++;
                }

            }
            return strSQlb.ToString();
        }
    }
}
