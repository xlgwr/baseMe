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
            where = !string.IsNullOrEmpty(where) ? string.Format(" where {0} ", where) : "";
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
            where = !string.IsNullOrEmpty(where) ? string.Format(" where {0} ", where) : "";
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
    }
}
