using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class PaginationQuery
    {
        public PaginationQuery()
        {
            wherelist = new List<WhereList>();
        }
        public static List<string> removeChar = "`!@#$%^&*()_+=-~[]<>/?".Split().ToList();
        public int pagesize { get; set; }
        public int pageindex { get; set; }
        string _where;
        string _and;
        public string where
        {
            get { return _where; }
            set
            {
                if (value.isNull())
                {
                    _where = "";
                }
                _where = value.Replace(removeChar, "");
            }
        }
        /// <summary>
        /// string, string, int,int
        /// 字段，值，标记1,标记2
        /// 标记1：1:and,2:or,
        /// 标记2：1:and,2:like%,3:%like,4:%like% 
        /// </summary>
        public List<WhereList> wherelist { get; set; }

        public string orderby { get; set; }
    }
    [Serializable]
    public class PaginationData
    {
        public int pagesize { get; set; }
        public int pageindex { get; set; }
        public int total { get; set; }
        public object rows { get; set; }
    }
    public class WhereList
    {
        string _colValue = "";
        string _colName = "";
        /// <summary>
        /// 字段名
        /// </summary>
        public String colName
        {
            get
            {
                return _colName;
            }
            set
            {
                if (value.isNull())
                {
                    _colName = "";
                }
                _colName = value.Replace(PaginationQuery.removeChar, "");
            }
        }
        /// <summary>
        /// 值
        /// </summary>
        public String colValue {
            get
            {
                return _colValue;
            }
            set
            {
                if (value.isNull())
                {
                    _colValue = "";
                }
                _colValue = value.Replace(PaginationQuery.removeChar, "");
            }
        }
        /// <summary>
        /// 1:and,2:or,
        /// </summary>
        public int andorFlag { get; set; }
        /// <summary>
        /// 0:like%,1:%like,2:%like%,3:and
        /// </summary>
        public int andLikeFlag { get; set; }

    }
}
