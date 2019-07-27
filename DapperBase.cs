public class BaseEntityPage
{
	 public int num { get; set; }
	 public int total { get; set; }
}
    
public static class BaseComm
{
    
        /// <summary>
        ///  {0} 样例
        ///  with allData as(
        ///  Select * from XXX
        ///  ) 
        /// 获取分页列表数据
        /// </summary>
        public static IEnumerable<T> GetPageEntities<T>(int pageSize, int pageIndex, string where, string order="Id")
            where T : BaseEntityPage
        { 
            where = !string.IsNullOrEmpty(where) ? string.Format(" where {0} ", where) : "";
            string wherepage = string.Format("where num between  {0} and {1}", (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);

            var allSql = new StringBuilder();
            allSql.AppendFormat(@"
            {0}
            , oa as (   
            select distinct ROW_NUMBER() over (order by x.{1} desc) as num,x.* from allData x
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

            ", allSql.ToString(), order, where, wherepage); 

            IEnumerable<T> viewList = GetData<T>(allSql.ToString());

            return viewList;
        }
        public static IEnumerable<T> GetData<T>(string sql)
            where T : BaseEntityPage
        {
            IEnumerable<T> viewList = DapperHelper.GetAppMall.Query<T>(sql);
            return viewList;
        }
}
