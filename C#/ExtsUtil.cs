using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ExtsUtil
    {
        public static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };
        #region 是否判断
        public static string getIfStr(this int m, string other = "", int isYesI = 1, int isNoI = 0, string isyes = "是", string isNo = "否")
        {
            return m == isYesI ? isyes : m == isNoI ? isNo : other;
        }
        public static string getIfStr2(this int m, string isyes = "█是 □否", string isNo = "□是 █否", string other = "□是 □否", int isYesI = 1, int isNoI = 0)
        {
            return m == isYesI ? isyes : m == isNoI ? isNo : other;
        }
        public static string getIfStr(this string m, string isyes = "█是 □否", string isNo = "□是 █否", string other = "□是 □否", string isYesI = "是", string isNoI = "否")
        {
            return m == isYesI ? isyes : m == isNoI ? isNo : other;
        }
        public static string getIfStr3(this string m)
        {
            string
                isyes = @"█归侨（含侨眷）
□ 原农场居民
□ 其他",
               isNo = @"□ 归侨（含侨眷）
█ 原农场居民
□ 其他",
               other = @"□ 归侨（含侨眷）
□ 原农场居民
█ 其他";
            return m == "1" ? isyes : m == "2" ? isNo : other;
        }

        public static string getContentToValue(this string m, string isyes = "侨", string isNo = "居民", string other = "其他")
        {
            if (m.isNull())
            {
                return "3";
            }
            return m.Contains(isyes) ? "1" : m.Contains(isNo) ? "2" : "3";
        }
        #endregion

        #region 字符串
        public static double ToDouble(this string m, double defaultV = 0)
        {
            double result = 0.00;
            if (double.TryParse(m, out result))
            {
                return result;
            };
            return defaultV;
        }
        public static int ToInt(this string m, int defaultV = 0)
        {
            int result = 0;
            if (int.TryParse(m, out result))
            {
                return result;
            };
            return defaultV;
        }
         public static bool Contains(this string m, List<string> checkV)
        {
            if (m.isNull() || checkV == null)
            {
                return false;
            }
            foreach (var item in checkV)
            {
                if (m.ContainsWithLower(item))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <param name="m"></param>
        /// <param name="checkV"></param>
        /// <returns></returns>
        public static bool ContainsWithLower(this string m, string checkV)
        {
            if (m.isNull() || checkV.isNull())
            {
                return false;
            }
            if (m.ToLower().Contains(checkV.ToLower()))
            {
                return true;
            }
            return false;
        }
        ///   <summary>
        ///   给一个字符串进行MD5加密
        ///   </summary>
        ///   <param   name="strText">待加密字符串</param>
        ///   <returns>加密后的字符串</returns>
        public static string MD5Encrypt(this string strText)
        {
            if (strText.isNull())
            {
                return strText;
            }
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(strText));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            return tmp.ToString();
        }

        /// <summary>
        /// 编码：Base64编码
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string ToBase64String(this string m)
        {
            if (m.isNull())
            {
                return "";
            }
            byte[] bytes = Encoding.Default.GetBytes(m);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// 解码：Base64编码
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string Base64GetString(this string m)
        {
            if (m.isNull())
            {
                return "";
            }
            byte[] outputb = Convert.FromBase64String(m);
            return Encoding.Default.GetString(outputb);
        }
        public static string Replace(this string m, List<string> toReplace, string foValue = "")
        {
            if (m.isNull())
            {
                return "";
            }
            foreach (var item in toReplace)
            {
                m.Replace(item, foValue);
            }
            return m;
        }
        public static bool isNull(this string m)
        {
            if (string.IsNullOrEmpty(m) || string.IsNullOrWhiteSpace(m))
            {
                return true;
            }
            return false;
        }
        public static string NullToStr(this object m, string defaultvalue = "", char trimChar = '、')
        {
            if (m == null)
            {
                return defaultvalue;
            }
            return m.ToString().Trim(trimChar);
        }
        /// <summary>
        /// 重复生成记录空白
        /// </summary>
        /// <param name="m"></param>
        /// <param name="genNum"></param>
        /// <param name="formatStr"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string StrFormatNum(this string m, int genNum = 3, string formatStr = "   {0}   {1}", char splitChar = '、')
        {
            if (m == null)
            {
                m = m.NullToStr();
            }
            var splitResult = m.SplitTrim(splitChar);
            string result = "";
            for (int i = 0; i < 3; i++)
            {
                string nameValue = "      ";
                if (i < splitResult.Count)
                {
                    nameValue = splitResult[i];
                }
                result += string.Format(formatStr, nameValue, splitChar);
            }
            return result.Trim(splitChar);

        }
        /// <summary>
        /// 去空，去空格
        /// </summary>
        /// <param name="m"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> SplitTrim(this string m, params char[] separator)
        {
            if (m == null)
            {
                return null;
            }
            var result = new List<string>();
            var resultSplit = m.Split(separator);
            foreach (var item in resultSplit)
            {
                if (!item.isNull())
                {
                    result.Add(item.Trim());
                }
            }
            return result;
        }

        #endregion

        #region 字典相关
        /// <summary>
        /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
        /// </summary>
        public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key) == false) dict.Add(key, value);
            return dict;
        }
        /// <summary>
        /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
        /// </summary>
        public static Dictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }
        /// <summary>
        /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
        /// </summary>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            return !key.NullToStr().isNull() && dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        #endregion
        #region 时间相关
        public static string toFormat(this DateTime m, string format = "yyyy-MM-dd HH:mm:ss")
        {
            if (m == null)
            {
                return "";
            }
            return m.ToString(format);
        }
        #endregion

        #region 数据转换
        public static string toEmptyStr(this double m, double defaultV = 0)
        {
            if (m <= defaultV)
            {
                return "";
            }
            return m.ToString();
        }
        #endregion

        #region SQL Getn
        /// <summary>
        /// 生成like sql 语句
        /// </summary>
        /// <param name="m"></param>
        /// <param name="isAnd">0: ,1:and,2:or</param>
        /// <param name="flag">0: XX%,1:%XX,other:%XX%</param>
        /// <returns></returns>
        public static string toLikeSql(this string m, string vaule, int isAnd = 0, int flag = 0)
        {
            if (m.isNull() || vaule.isNull())
            {
                return "";
            }
            string isOr = isAnd == 0 ? "" : isAnd == 1 ? "AND" : "OR";
            switch (flag)
            {
                case 0:
                    return string.Format(" {0} {1} like N'{2}%' ", isOr, m, vaule);
                case 1:
                    return string.Format(" {0} {1} like N'%{2}' ", isOr, m, vaule);
                default:
                    return string.Format(" {0} {1} like N'%{2}%' ", isOr, m, vaule);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="vaule"></param>
        /// <param name="isAnd">0:空,1:and,2:or</param>
        /// <param name="isEquleGeLt">3:=,4:>,5:<,6:!=<,7: is null,8:is NOT NULL /param>
        /// <returns></returns>
        public static string toAndOrSql(this string m, string vaule, int isAnd = 0, int isEquleGeLt = 3)
        {
            if (m.isNull())
            {
                return "";
            }
            string isOr = isAnd == 0 ? "" : isAnd == 1 ? "AND" : "OR";
            var strResult = "";
            switch (isEquleGeLt)
            {
                case 3:
                    strResult = string.Format(" {0} {1} = N'{2}' ", isOr, m, vaule);
                    break;
                case 4:
                    strResult = string.Format(" {0} {1} > '{2}' ", isOr, m, vaule);
                    break;
                case 5:
                    strResult = string.Format(" {0} {1} < '{2}' ", isOr, m, vaule);
                    break;
                case 6:
                    strResult = string.Format(" {0} {1} != N'{2}' ", isOr, m, vaule);
                    break;
                case 7:
                    strResult = string.Format(" {0} {1} is NULL ", isOr, m, vaule);
                    break;
                case 8:
                    strResult = string.Format(" {0} {1} is NOT NULL ", isOr, m, vaule);
                    break;
                default:
                    strResult = string.Format(" {0} {1} = N'{2}' ", isOr, m, vaule);
                    break;
            }

            return strResult;

        }
        #endregion
        public static T JsonTo<T>(this object m)
        {
            if (m == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(m.NullToStr(), settings);
        }
        public static string toJsonStr(this object m)
        {
            return JsonConvert.SerializeObject(m, settings);
        }
    }
}
