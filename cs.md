# C# 常用扩展

```
 #region 字符串
        public static string getIfStr(this int m, string other = "", int isYesI = 1, int isNoI = 0, string isyes = "是", string isNo = "否")
        {
            return m == isYesI ? isyes : m == isNoI ? isNo : other;
        }
        public static double ToDouble(this string m, double defaultV)
        {
            double result = 0.00;
            if (double.TryParse(m, out result))
            {
                return result;
            };
            return defaultV;
        }
        public static int ToInt(this string m, int defaultV)
        {
            int result = 0;
            if (int.TryParse(m, out result))
            {
                return result;
            };
            return defaultV;
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
        
        public static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };
        
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
```
