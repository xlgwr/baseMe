using Dapper;
using FHCollection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FHCollection.Model.AllEnum;

namespace System
{
    public static class BaseComm
    {
        public static JsonMessage GetPageEntitiesDetails<T>(string sqlIndex, List<int> Ids, string keyId = "Id")
            where T : IEntityPage
        {
            JsonMessage msg = JsonHandler.CreateMessage((int)MsgTypeEnum.成功, "");
            if (!Ids.Any())
            {
                msg = JsonHandler.CreateMessage((int)MsgTypeEnum.错误, "Id 不正确。");
                return msg;
            }
            int total = 0;

            string where = string.Format(" {0} in @{0}", keyId);

            var data = BaseComm.GetPageEntities<T>(
                1,
                1,
                sqlIndex,
                where,
                out total,
                keyId,
                new Dictionary<string, object>() { { keyId, Ids } }
                );
            msg = JsonHandler.CreateMessage((int)MsgTypeEnum.成功, "", data);

            return msg;
        }

        public static async Task<JsonMessage> GetPageEntitiesDetailsSync<T>(string sqlIndex, List<int> Ids, string keyId = "Id")
            where T : IEntityPage
        {
            JsonMessage msg = JsonHandler.CreateMessage((int)MsgTypeEnum.成功, "");
            if (Ids.Count <= 0)
            {
                msg = JsonHandler.CreateMessage((int)MsgTypeEnum.错误, "Id 不正确。");
                return msg;
            }
            int total = 0;

            string where = string.Format(" {0} in @{0}", keyId);

            var data = await BaseComm.GetPageEntitiessync<T>(
                100,
                1,
                sqlIndex,
                where,
                keyId,
                new Dictionary<string, object>() { { keyId, Ids } }
                );
            if (data.Count() > 0)
            {
                total = data.First().total;
            }
            msg = JsonHandler.CreateMessage((int)MsgTypeEnum.成功, "", data);

            return msg;
        }
        /// <summary>
        ///  {0} 样例
        ///  with allData as(
        ///  Select * from XXX
        ///  ) 
        /// 获取房屋分页列表数据
        /// </summary>
        public static IEnumerable<T> GetPageEntities<T>(int pageSize, int pageIndex, string sqlFirst, string where, out int total, string order = "Id", object param = null)
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

            IEnumerable<T> viewList = GetData<T>(allSql.ToString(), param);

            if (viewList.Count() > 0)
            {
                total = viewList.First().total;
            }

            return viewList;
        }
        public static Task<IEnumerable<T>> GetPageEntitiessync<T>(int pageSize, int pageIndex, string sqlFirst, string where, string order = "Id", object param = null)
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

            return GetDatasync<T>(allSql.ToString(), param);
        }

        public static IEnumerable<T> GetData<T>(string sql, object param = null)
        {
            IEnumerable<T> viewList = DapperHelper.GetAppMall.Query<T>(sql, param);
            return viewList;
        }
        public static Task<IEnumerable<T>> GetDatasync<T>(string sql, object param = null)
        {
            return DapperHelper.GetAppMall.QueryAsync<T>(sql, param);
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

        /// <summary>
        /// 获取审核信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public static List<ZS_Audit> CheckAudit(int id, BusinessTypeEnum businessType)
        {
            var sql = "SELECT * FROM ZS_Audit WHERE DataId=@DataId AND BusinessType=@BusinessType";
            return DapperHelper.GetAppMall.Query<ZS_Audit>(sql, new { BusinessType = businessType, DataId = id }).ToList();
        }
        public static List<ZS_Audit> CheckAudit(List<int> ids, BusinessTypeEnum businessType)
        {
            if (ids == null || ids.Count <= 0)
            {
                return new List<ZS_Audit>();
            }
            var sql = "SELECT * FROM ZS_Audit WHERE DataId in @DataIds AND BusinessType=@BusinessType";
            return DapperHelper.GetAppMall.Query<ZS_Audit>(sql, new { BusinessType = businessType, DataIds = ids }).ToList();
        }
        public static ZS_Audit CheckAuditFirst(int id, BusinessTypeEnum businessType)
        {
            return CheckAudit(id, businessType).FirstOrDefault();
        }
        /// <summary>
        /// 是否已审,已审无法修改
        /// 增加三审，
        /// 有三审角色可改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="businessType"></param>
        /// <returns></returns>
        public static bool CheckAuditEditIs(int id, BusinessTypeEnum businessType, UserRight userRight)
        {
            if (userRight == null)
            {
                return false;
            }
            //管理员可修改，无需验正
            if (userRight.User.UName == "admin")
            {
                return true;
            }

            var getAuid = CheckAuditFirst(id, businessType);
            var roleInfo = userRight.RoleList;

            if (getAuid == null)
            {
                return true;
            }
            if (!getAuid.RecordState.HasValue)
            {
                getAuid.RecordState = 0;
            }
            switch (getAuid.RecordState.Value)
            {
                case (short)HasRolesAuditEnum.默认:
                    //一审人可修改
                    if (roleInfo.Where(a => a.RoleName.Contains("审核人1")).Count() > 0)
                    {
                        return true;
                    };
                    break;
                case (short)HasRolesAuditEnum.审核人1:
                    if (roleInfo.Where(a => a.RoleName.Contains("审核人2")).Count() > 0)
                    {
                        return true;
                    };
                    break;
                case (short)HasRolesAuditEnum.审核人2:
                    if (roleInfo.Where(a => a.RoleName.Contains("审核人3")).Count() > 0)
                    {
                        return true;
                    };
                    break;
                case (short)HasRolesAuditEnum.审核人3:
                    if (roleInfo.Where(a => a.RoleName.Contains("最终审核人")).Count() > 0)
                    {
                        return true;
                    };
                    break;
                case (short)HasRolesAuditEnum.最终审核人:
                default:
                    break;
            }
            return false;
        }

        #region CRUD
        public static Task<int> Remove(int id, string deleTableName)
        {
            string sql = string.Format("DELETE FROM [dbo].[{0}] WHERE Id={1}", deleTableName, id);
            return DapperHelper.GetAppMall.ExecuteAsync(sql);
        }
        public static Task<int> ExecSql(string sql)
        {
            return DapperHelper.GetAppMall.ExecuteAsync(sql);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deleTableName"></param>
        /// <param name="setStr">XX=aa,YY=bb</param>
        /// <param name="where">and xx=dd</param>
        /// <returns></returns>
        public static Task<int> Update(List<int> id, string deleTableName, string setStr, string where = "")
        {
            string sql = string.Format("UPDATE [dbo].[{0}]  SET {1} WHERE Id in @Id {2}", deleTableName, setStr, where);
            return DapperHelper.GetAppMall.ExecuteAsync(sql, id);
        }
        #endregion
    }
}
