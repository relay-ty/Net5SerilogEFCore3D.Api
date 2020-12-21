using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;

namespace Net5SerilogEFCore3D.Infrastructure.Extensions
{
    public static class UtilExtension
    {
        #region 常用转换
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static int ObjToInt(this object thisValue)
        {
            int reval = 0;
            if (thisValue == null) return 0;
            if (thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return reval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static int ObjToInt(this object thisValue, int errorValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && int.TryParse(thisValue.ToString(), out int reval))
            {
                return reval;
            }
            return errorValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static double ObjToMoney(this object thisValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out double reval))
            {
                return reval;
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static double ObjToMoney(this object thisValue, double errorValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && double.TryParse(thisValue.ToString(), out double reval))
            {
                return reval;
            }
            return errorValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static string ObjToString(this object thisValue)
        {
            if (thisValue != null) return thisValue.ToString().Trim();
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static string ObjToString(this object thisValue, string errorValue)
        {
            if (thisValue != null) return thisValue.ToString().Trim();
            return errorValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static Decimal ObjToDecimal(this object thisValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out decimal reval))
            {
                return reval;
            }
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static Decimal ObjToDecimal(this object thisValue, decimal errorValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && decimal.TryParse(thisValue.ToString(), out decimal reval))
            {
                return reval;
            }
            return errorValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static DateTime ObjToDate(this object thisValue)
        {
            DateTime reval = DateTime.MinValue;
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out reval))
            {
                reval = Convert.ToDateTime(thisValue);
            }
            return reval;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <param name="errorValue"></param>
        /// <returns></returns>
        public static DateTime ObjToDate(this object thisValue, DateTime errorValue)
        {
            if (thisValue != null && thisValue != DBNull.Value && DateTime.TryParse(thisValue.ToString(), out DateTime reval))
            {
                return reval;
            }
            return errorValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static bool ObjToBool(this object thisValue)
        {
            bool reval = false;
            if (thisValue != null && thisValue != DBNull.Value && bool.TryParse(thisValue.ToString(), out reval))
            {
                return reval;
            }
            return reval;
        }
        #endregion

        #region CookieContainer 序列化 反序列化 Base64
        public static string Serialize(this CookieContainer cookieContainer)
        {
            var listCookieCollection = cookieContainer.GetAllCookieCollections();
            var listBase64String = new List<string>();
            foreach (var cookieCollection in listCookieCollection)
            {
                var cookieCollectionSerializer = new DataContractSerializer(typeof(CookieCollection));
                using var memoryStream = new MemoryStream();
                cookieCollectionSerializer.WriteObject(memoryStream, cookieCollection);
                listBase64String.Add(Convert.ToBase64String(memoryStream.ToArray()));
            }
            var listSerializer = new DataContractSerializer(typeof(List<string>));
            using var stream = new MemoryStream();
            listSerializer.WriteObject(stream, listBase64String);
            return Convert.ToBase64String(stream.ToArray());
        }

        public static CookieContainer DeSerialize(this string base64String)
        {
            var cookieContainer = new CookieContainer();
            var listBase64String = new List<string>();

            var listSerializer = new DataContractSerializer(typeof(List<string>));
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(base64String)))
            {
                listBase64String = (List<string>)listSerializer.ReadObject(memoryStream);
            }
            foreach (var base64 in listBase64String)
            {
                var cookieCollectionSerializer = new DataContractSerializer(typeof(CookieCollection));
                using var memoryStream = new MemoryStream(Convert.FromBase64String(base64));
                cookieContainer.Add((CookieCollection)cookieCollectionSerializer.ReadObject(memoryStream));
            }

            return cookieContainer;
        }

        public static List<Cookie> GetAllCookies(this CookieContainer cookieContainer)
        {
            List<Cookie> listCookie = new List<Cookie>();

            Hashtable hashtable = (Hashtable)cookieContainer.GetType().
                InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cookieContainer, Array.Empty<object>());
            foreach (object pathList in hashtable.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().
                    InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, Array.Empty<object>());
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie cookie in colCookies)
                        listCookie.Add(cookie);
            }
            return listCookie;
        }
        public static List<CookieCollection> GetAllCookieCollections(this CookieContainer cookieContainer)
        {
            var listCookieCollection = new List<CookieCollection>();

            Hashtable hashtable = (Hashtable)cookieContainer.GetType().
                InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cookieContainer, Array.Empty<object>());
            foreach (object pathList in hashtable.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().
                    InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, Array.Empty<object>());
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    listCookieCollection.Add(colCookies);
            }
            return listCookieCollection;
        }
        #endregion

        #region 常用字符串操作
        /// <summary>
        /// 截取 startStr endStr 中间的字符串
        /// </summary>
        /// <param name="sourse"></param>
        /// <param name="startStr"></param>
        /// <param name="endStr"></param>
        /// <returns></returns>
        public static string MidStrEx(this string sourse, string startStr, string endStr)
        {
            string result = string.Empty;

            try
            {
                int startindex = 0;
                if (!string.IsNullOrEmpty(startStr))
                    startindex = sourse.IndexOf(startStr);
                if (startindex == -1)
                    return result;
                string tmpstr = sourse[(startindex + startStr.Length)..];//string tmpstr = sourse.Substring(startindex + startStr.Length);
                int endindex;
                if (!string.IsNullOrEmpty(endStr))
                    endindex = tmpstr.IndexOf(endStr);
                else
                    return tmpstr;
                if (endindex == -1)
                    return result;
                result = tmpstr.Remove(endindex);
            }
            catch
            {
            }
            return result;
        }
        #endregion
    }

}
